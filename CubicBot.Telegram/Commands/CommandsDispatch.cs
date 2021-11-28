using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Collections.Generic;
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

        public List<CubicBotCommand> Commands { get; } = new();

        public CommandsDispatch(Config config, Data data, string botUsername)
        {
            _config = config;
            _data = data;
            _botUsername = botUsername;

            if (config.Commands.EnablePersonal)
            {
                Commands.AddRange(Personal.Commands);
            }

            if (config.Commands.EnableCommon)
            {
                Commands.AddRange(Common.Commands);
            }

            if (config.Commands.EnableDice)
            {
                Commands.AddRange(Dice.Commands);
            }

            if (config.Commands.EnableConsentNotNeeded)
            {
                Commands.AddRange(ConsentNotNeeded.Commands);
            }

            if (config.Commands.EnableNonVegan)
            {
                Commands.AddRange(NonVegan.Commands);
            }

            if (config.Commands.EnableLawEnforcement)
            {
                Commands.AddRange(LawEnforcement.Commands);
            }

            if (config.Commands.EnablePublicServices)
            {
                Commands.AddRange(PublicServices.Commands);
            }

            if (config.Commands.EnableChinese)
            {
                Commands.AddRange(Chinese.Commands);
            }

            if (config.Commands.EnableChineseTasks)
            {
                Commands.AddRange(ChineseTasks.Commands);
            }

            if (config.EnableStats)
            {
                var queryStats = new QueryStats(config);
                Commands.AddRange(queryStats.Commands);
            }
        }

        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default)
        {
            (var command, var argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message.Text, _botUsername);
            if (command is null)
            {
                return Task.CompletedTask;
            }

            var filteredCommands = Commands.Where(x => x.Command == command);
            if (filteredCommands.Any())
            {
                var filteredCommand = filteredCommands.First();
                var handleTask = filteredCommand.HandlerAsync(botClient, message, argument, _config, _data, cancellationToken);
                if (_config.EnableStats && _config.Stats.EnableCommandStats)
                {
                    var userId = message.From?.Id ?? 777000L;
                    if (message.Chat.Type is ChatType.Private)
                    {
                        var userData = _data.GetOrCreateUserData(userId);
                        userData.CommandsHandled++;
                        filteredCommand.UserStatsCollector?.Invoke(message, argument, userData);
                        filteredCommand.UserOrMemberStatsCollector?.Invoke(message, argument, userData, null, null);
                        if (filteredCommand.UserOrMemberRespondAsync is not null)
                        {
                            return Task.WhenAll(handleTask, filteredCommand.UserOrMemberRespondAsync(botClient, message, argument, _config, _data, userData, null, null, cancellationToken));
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
                        filteredCommand.GroupStatsCollector?.Invoke(message, argument, groupData, userData);
                        filteredCommand.UserOrMemberStatsCollector?.Invoke(message, argument, userData, groupData, replyToUserData);
                        if (filteredCommand.UserOrMemberRespondAsync is not null)
                        {
                            return Task.WhenAll(handleTask, filteredCommand.UserOrMemberRespondAsync(botClient, message, argument, _config, _data, userData, groupData, replyToUserData, cancellationToken));
                        }
                    }
                }

                return handleTask;
            }

            return Task.CompletedTask;
        }
    }
}
