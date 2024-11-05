using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram;

public sealed partial class UpdateHandler
{
    private readonly Data _data;
    private readonly ILogger _logger;
    private readonly List<IDispatch> _dispatches = [];
    private readonly IReadOnlyList<CubicBotCommand> _commands;

    public UpdateHandler(string botUsername, Config config, Data data, ILogger logger)
    {
        _data = data;
        _logger = logger;

        if (config.EnableCommands)
        {
            var commandsDispatch = new CommandsDispatch(config, botUsername, logger);
            _commands = commandsDispatch.Commands;
            _dispatches.Add(commandsDispatch);
        }
        else
        {
            _commands = [];
        }

        if (config.EnableStats)
        {
            var statsDispatch = new StatsDispatch(config.Stats);
            _dispatches.Add(statsDispatch);
        }
    }

    public async Task RegisterCommandsAsync(ITelegramBotClient botClient, CancellationToken cancellationToken = default)
    {
        if (_commands.Count > 0)
        {
            await botClient.SetMyCommands(_commands, cancellationToken: cancellationToken);
            LogRegisteredCommands(_commands.Count);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Registered {CommandCount} bot commands")]
    private partial void LogRegisteredCommands(int commandCount);

    public async Task HandleUpdateStreamAsync(ITelegramBotClient botClient, IAsyncEnumerable<Update> updates, CancellationToken cancellationToken = default)
    {
        await foreach (var update in updates.WithCancellation(cancellationToken))
        {
            LogReceivedUpdate(update.Id, update.Type);

            if (update.Type == UpdateType.Message && update.Message is not null)
            {
                var messageContext = new MessageContext(botClient, update.Message, _data);

                foreach (var dispatch in _dispatches)
                {
                    _ = dispatch.HandleAsync(messageContext, cancellationToken)
                                .ContinueWith(t =>
                                {
                                    if (t?.Exception?.InnerException is not null)
                                    {
                                        HandleError(t.Exception.InnerException);
                                    }
                                }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }
    }

    [LoggerMessage(Level = LogLevel.Trace, Message = "Received update with ID {UpdateId} and type {UpdateType}")]
    private partial void LogReceivedUpdate(int updateId, UpdateType updateType);

    public Task HandleErrorAsync(Exception ex, CancellationToken _ = default)
    {
        HandleError(ex);
        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to handle update")]
    private partial void HandleError(Exception ex);
}
