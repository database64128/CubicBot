using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram;

public sealed partial class UpdateHandler
{
    private readonly Data _data;
    private readonly ILogger _logger;
    private readonly List<IDispatch> _dispatches = [];
    private readonly ReadOnlyCollection<CubicBotCommand> _commands;

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

    public async Task RunAsync(ITelegramBotClient botClient, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Update[] updates = await botClient.GetUpdates(allowedUpdates: [UpdateType.Message], cancellationToken: cancellationToken);
                foreach (Update update in updates)
                {
                    LogReceivedUpdate(update.Id, update.Type);

                    if (update.Message is Message message)
                    {
                        var messageContext = new MessageContext(botClient, message, _data);

                        foreach (var dispatch in _dispatches)
                        {
                            _ = dispatch.HandleAsync(messageContext, cancellationToken)
                                        .ContinueWith(t =>
                                        {
                                            if (t?.Exception?.InnerException is not null)
                                            {
                                                LogFailedToHandleUpdate(t.Exception.InnerException);
                                            }
                                        }, TaskContinuationOptions.OnlyOnFaulted);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                LogFailedToReceiveUpdates(ex);

                try
                {
                    await Task.Delay(s_delayOnError, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }

    private static readonly TimeSpan s_delayOnError = TimeSpan.FromSeconds(5);

    [LoggerMessage(Level = LogLevel.Trace, Message = "Received update with ID {UpdateId} and type {UpdateType}")]
    private partial void LogReceivedUpdate(int updateId, UpdateType updateType);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to receive updates")]
    private partial void LogFailedToReceiveUpdates(Exception ex);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to handle update")]
    private partial void LogFailedToHandleUpdate(Exception ex);
}
