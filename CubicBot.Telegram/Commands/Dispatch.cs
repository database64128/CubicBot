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
    public class Dispatch : IDispatch
    {
        private readonly string _botUsername;

        public List<BotCommandWithHandler> Commands { get; } = new();

        public Dispatch(Config config, string botUsername)
        {
            _botUsername = botUsername;

            var random = new Random();

            if (config.EnableCommon)
            {
                var common = new Common(random);
                Commands.AddRange(common.Commands);
            }

            if (config.EnableDice)
            {
                var dice = new Dice(random);
                Commands.AddRange(dice.Commands);
            }

            if (config.EnableChinese)
            {
                var chinese = new Chinese(random);
                Commands.AddRange(chinese.Commands);
            }
        }

        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default)
        {
            (var command, var argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message.Text, _botUsername);
            if (command is null)
                return Task.CompletedTask;

            var filteredCommands = Commands.Where(x => x.Command == command);
            if (filteredCommands.Any())
                return filteredCommands.First().Handler(botClient, message, argument, cancellationToken);
            else
                return Task.CompletedTask;
        }
    }
}
