﻿using CubicBot.Telegram.Commands;
using CubicBot.Telegram.Stats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram;

public class UpdateHandler
{
    private readonly Config _config;
    private readonly Data _data;
    private readonly List<IDispatch> _dispatches = new();

    public ReadOnlyCollection<CubicBotCommand> Commands { get; }

    public UpdateHandler(string botUsername, Config config, Data data)
    {
        _config = config;
        _data = data;

        if (config.EnableCommands)
        {
            var commandsDispatch = new CommandsDispatch(config, data, botUsername);
            Commands = commandsDispatch.Commands;
            _dispatches.Add(commandsDispatch);
        }
        else
        {
            Commands = Array.Empty<CubicBotCommand>().AsReadOnly();
        }

        if (config.EnableStats)
        {
            var statsDispatch = new StatsDispatch(config.Stats, data);
            _dispatches.Add(statsDispatch);
        }
    }

    public async Task HandleUpdateStreamAsync(ITelegramBotClient botClient, IAsyncEnumerable<Update> updates, CancellationToken cancellationToken = default)
    {
        await foreach (var update in updates.WithCancellation(cancellationToken))
        {
            if (update.Type == UpdateType.Message && update.Message is not null)
            {
                var messageContext = new MessageContext(botClient, update.Message, _data);

                foreach (var dispatch in _dispatches)
                {
                    _ = Task.Run(() => dispatch.HandleAsync(messageContext, cancellationToken), cancellationToken)
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

    public static void HandleError(Exception ex)
    {
        var errorMessage = ex switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error: [{apiRequestException.ErrorCode}] {apiRequestException.Message}",
            _ => ex.ToString(),
        };

        Console.WriteLine(errorMessage);
    }

    public static Task HandleErrorAsync(Exception ex, CancellationToken _ = default)
    {
        HandleError(ex);
        return Task.CompletedTask;
    }
}
