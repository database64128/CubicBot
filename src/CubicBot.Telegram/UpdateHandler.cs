using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram;

public sealed partial class UpdateHandler
{
    private readonly Data _data;
    private readonly ILogger _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly List<IDispatch> _dispatches = [];
    private readonly ReadOnlyCollection<CubicBotCommand> _commands;

    public UpdateHandler(string botUsername, Config config, Data data, ILogger logger, ITelegramBotClient botClient)
    {
        _data = data;
        _logger = logger;
        _botClient = botClient;

        if (config.EnableCommands)
        {
            CommandsDispatch commandsDispatch = new(config, botUsername, logger);
            _commands = commandsDispatch.Commands;
            _dispatches.Add(commandsDispatch);
        }
        else
        {
            _commands = [];
        }

        if (config.EnableStats)
        {
            StatsDispatch statsDispatch = new(config.Stats);
            _dispatches.Add(statsDispatch);
        }
    }

    public async Task RegisterCommandsAsync(CancellationToken cancellationToken = default)
    {
        if (_commands.Count > 0)
        {
            await _botClient.SetMyCommands(_commands, cancellationToken: cancellationToken);
            LogRegisteredCommands(_commands.Count);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Registered {CommandCount} bot commands")]
    private partial void LogRegisteredCommands(int commandCount);

    public async Task RunAsync(ChannelReader<Update> reader, CancellationToken cancellationToken = default)
    {
        await foreach (Update update in reader.ReadAllAsync(cancellationToken))
        {
            LogReceivedUpdate(update.Id);

            if (update.Message is Message message)
            {
                MessageContext messageContext = new(_botClient, message, _data);

                foreach (IDispatch dispatch in _dispatches)
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

    [LoggerMessage(Level = LogLevel.Trace, Message = "Received update with ID {UpdateId}")]
    private partial void LogReceivedUpdate(int updateId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to handle update")]
    private partial void LogFailedToHandleUpdate(Exception ex);
}
