using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram
{
    public class UpdateHandler
    {
        private readonly List<IDispatch> _dispatches = new();

        public IEnumerable<BotCommand> Commands { get; } = Array.Empty<BotCommand>();

        public UpdateHandler(string botUsername, Config config, Data data)
        {
            if (config.EnableCommands)
            {
                var commandsDispatch = new CommandsDispatch(config, data, botUsername);
                Commands = commandsDispatch.Commands;
                _dispatches.Add(commandsDispatch);
            }

            if (config.EnableStats)
            {
                var statsDispatch = new StatsDispatch(config.Stats, data);
                _dispatches.Add(statsDispatch);
            }
        }

        public async Task HandleUpdateStreamAsync(ITelegramBotClient botClient, IAsyncEnumerable<Update> updates, CancellationToken cancellationToken = default)
        {
            await foreach (var update in updates)
            {
                try
                {
                    if (update.Type == UpdateType.Message)
                    {
                        var tasks = _dispatches.Select(dispatch => dispatch.HandleAsync(botClient, update.Message, cancellationToken));
                        await Task.WhenAll(tasks);
                    }
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(ex, cancellationToken);
                }
            }
        }

        public static Task HandleErrorAsync(Exception ex, CancellationToken _ = default)
        {
            var errorMessage = ex switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error: [{apiRequestException.ErrorCode}] {apiRequestException.Message}",
                _ => ex.ToString(),
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}
