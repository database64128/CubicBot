using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

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

            var random = new Random();

            if (config.Commands.EnableCommon)
            {
                var common = new Common(random);
                Commands.AddRange(common.Commands);
            }

            if (config.Commands.EnableDice)
            {
                var dice = new Dice(random);
                Commands.AddRange(dice.Commands);
            }

            if (config.Commands.EnableConsentNotNeeded)
            {
                var consentNotNeeded = new ConsentNotNeeded(random);
                Commands.AddRange(consentNotNeeded.Commands);
            }

            if (config.Commands.EnableNonVegan)
            {
                var nonVegan = new NonVegan(random);
                Commands.AddRange(nonVegan.Commands);
            }

            if (config.Commands.EnableLawEnforcement)
            {
                var lawEnforcement = new LawEnforcement(random);
                Commands.AddRange(lawEnforcement.Commands);
            }

            if (config.Commands.EnablePublicServices)
            {
                var publicServices = new PublicServices(random);
                Commands.AddRange(publicServices.Commands);
            }

            if (config.Commands.EnableChinese)
            {
                var chinese = new Chinese(random);
                Commands.AddRange(chinese.Commands);
            }

            if (config.Commands.EnableChineseTasks)
            {
                var chineseTasks = new ChineseTasks(random);
                Commands.AddRange(chineseTasks.Commands);
            }

            if (config.EnableStats)
            {
                var queryStats = new QueryStats(config.Stats);
                Commands.AddRange(queryStats.Commands);
            }
        }

        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default)
        {
            (var command, var argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message.Text, _botUsername);
            if (command is null)
                return Task.CompletedTask;

            var filteredCommands = Commands.Where(x => x.Command == command);
            if (filteredCommands.Any())
            {
                var filteredCommand = filteredCommands.First();
                if (_config.EnableStats && _config.Stats.EnableCommandStats && filteredCommand.StatsCollector is not null)
                {
                    return Task.WhenAll(filteredCommand.Handler(botClient, message, argument, _config, _data, cancellationToken),
                                        filteredCommand.StatsCollector(botClient, message, argument, _config, _data, cancellationToken));
                }
                else
                {
                    return filteredCommand.Handler(botClient, message, argument, _config, _data, cancellationToken);
                }
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}
