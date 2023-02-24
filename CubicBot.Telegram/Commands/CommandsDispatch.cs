using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public class CommandsDispatch : IDispatch
    {
        private readonly Config _config;
        private readonly Data _data;
        private readonly string _botUsername;
        private readonly Dictionary<string, CubicBotCommand> _commandDict;

        public ReadOnlyCollection<CubicBotCommand> Commands { get; }

        public CommandsDispatch(Config config, Data data, string botUsername)
        {
            _config = config;
            _data = data;
            _botUsername = botUsername;

            var commands = new List<CubicBotCommand>();

            if (config.Commands.EnablePersonal)
            {
                commands.AddRange(Personal.Commands);
            }

            if (config.Commands.EnableCommon)
            {
                commands.AddRange(Common.Commands);
            }

            if (config.Commands.EnableDice)
            {
                commands.AddRange(Dice.Commands);
            }

            if (config.Commands.EnableConsentNotNeeded)
            {
                commands.AddRange(ConsentNotNeeded.Commands);
            }

            if (config.Commands.EnableNonVegan)
            {
                commands.AddRange(NonVegan.Commands);
            }

            if (config.Commands.EnableLawEnforcement)
            {
                commands.AddRange(LawEnforcement.Commands);
            }

            if (config.Commands.EnablePublicServices)
            {
                commands.AddRange(PublicServices.Commands);
            }

            if (config.Commands.EnableChinese)
            {
                commands.AddRange(Chinese.Commands);
            }

            if (config.Commands.EnableChineseTasks)
            {
                commands.AddRange(ChineseTasks.Commands);
            }

            if (config.Commands.EnableSystemd)
            {
                commands.AddRange(Systemd.Commands);
            }

            if (config.EnableStats)
            {
                var queryStats = new QueryStats(config);
                commands.AddRange(queryStats.Commands);

                var controls = new Controls(config);
                commands.AddRange(controls.Commands);
            }

            Commands = commands.AsReadOnly();
            _commandDict = commands.ToDictionary(x => x.Command);
        }

        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default)
        {
            var (command, argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message.Text, _botUsername);
            if (command is null)
            {
                return Task.CompletedTask;
            }

            if (_commandDict.TryGetValue(command, out var botCommand))
            {
                var handleTask = botCommand.HandlerAsync(botClient, message, argument, _config, _data, cancellationToken);
                if (_config.EnableStats && _config.Stats.EnableCommandStats)
                {
                    var userId = ChatHelper.GetMessageSenderId(message);
                    if (message.Chat.Type is ChatType.Private)
                    {
                        var userData = _data.GetOrCreateUserData(userId);
                        userData.CommandsHandled++;
                        botCommand.UserStatsCollector?.Invoke(message, argument, userData);
                        botCommand.UserOrMemberStatsCollector?.Invoke(message, argument, userData, null, null);
                        if (botCommand.UserOrMemberRespondAsync is not null)
                        {
                            return Task.WhenAll(handleTask, botCommand.UserOrMemberRespondAsync(botClient, message, argument, _config, _data, userData, null, null, cancellationToken));
                        }
                    }
                    else
                    {
                        var groupData = _data.GetOrCreateGroupData(message.Chat.Id);
                        groupData.CommandsHandled++;
                        var userData = groupData.GetOrCreateUserData(userId);
                        userData.CommandsHandled++;
                        UserData? replyToUserData = null;
                        var replyToUserId = message.ReplyToMessage?.From?.Id;
                        if (replyToUserId is long id)
                        {
                            replyToUserData = groupData.GetOrCreateUserData(id);
                        }
                        botCommand.GroupStatsCollector?.Invoke(message, argument, groupData, userData);
                        botCommand.UserOrMemberStatsCollector?.Invoke(message, argument, userData, groupData, replyToUserData);
                        if (botCommand.UserOrMemberRespondAsync is not null)
                        {
                            return Task.WhenAll(handleTask, botCommand.UserOrMemberRespondAsync(botClient, message, argument, _config, _data, userData, groupData, replyToUserData, cancellationToken));
                        }
                    }
                }

                return handleTask;
            }

            return Task.CompletedTask;
        }
    }
}
