using CubicBot.Telegram.Utils;
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
