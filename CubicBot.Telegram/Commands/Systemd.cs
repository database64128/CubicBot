﻿using CubicBot.Telegram.Utils;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Systemd
{
    private static readonly string[] s_states =
    [
        "[***   ]",
        "[ ***  ]",
        "[  *** ]",
        "[   ***]",
        "[*   **]",
        "[**   *]",
    ];

    private const string WaitState = "[ WAIT ]";

    private const string OkState = "[  OK  ]";

    private const string HelpMarkdownV2 = @"
Usage: `/systemctl <command> [unit]`
Supported commands: *start*, *stop*, *restart*, *reload*\.
Reply to a message to use the sender's name as the unit\.";

    private enum SystemdUnitType
    {
        Service,
        Timer,
        Target,
    }

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("systemctl", "➡️ systemctl <command> [unit]", SystemctlAsync, statsCollector: CountSystemctlCalls));
    }

    public static Task SystemctlAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        if (commandContext.Argument is not string argument)
        {
            return SendHelpAsync(commandContext, @"Missing command\.", cancellationToken);
        }

        ReadOnlySpan<char> command;
        string unit;
        SystemdUnitType unitType;

        var spacePos = argument.IndexOf(' ');
        if (spacePos == -1 && commandContext.Message.ReplyToMessage?.From?.FirstName is string firstname)
        {
            command = argument;
            unit = firstname;
            unitType = SystemdUnitType.Service;
        }
        else if (spacePos > 0 && spacePos < argument.Length - 1)
        {
            command = argument.AsSpan()[..spacePos];
            unit = argument[(spacePos + 1)..];
            var dotPos = unit.LastIndexOf('.');
            unitType = dotPos switch
            {
                -1 => SystemdUnitType.Service,
                _ => unit.AsSpan()[dotPos..] switch
                {
                    ".timer" => SystemdUnitType.Timer,
                    ".target" => SystemdUnitType.Target,
                    _ => SystemdUnitType.Service,
                },
            };
        }
        else
        {
            return SendHelpAsync(commandContext, @"Missing unit\.", cancellationToken);
        }

        var unsupportedErrMsgMarkdownV2 = unitType switch
        {
            SystemdUnitType.Target => command switch
            {
                "stop" => @"Target units do not support stopping.\",
                "reload" => @"Target units do not support reloading\.",
                "restart" => @"Target units do not support restarting\.",
                _ => null,
            },
            _ => null,
        };
        if (unsupportedErrMsgMarkdownV2 is not null)
        {
            return SendHelpAsync(commandContext, unsupportedErrMsgMarkdownV2, cancellationToken);
        }

        return command switch
        {
            "start" => SystemctlExecAsync(commandContext, unit, 3, 13, " Starting ", unitType is SystemdUnitType.Target ? " Reached " : " Started ", cancellationToken),
            "stop" => SystemctlExecAsync(commandContext, unit, 3, 13, " Stopping ", " Stopped ", cancellationToken),
            "reload" => SystemctlExecAsync(commandContext, unit, 3, 6, " Reloading ", " Reloaded ", cancellationToken),
            "restart" => SystemctlRestartAsync(commandContext, unit, cancellationToken),
            _ => SendHelpAsync(commandContext, @"Invalid command\.", cancellationToken),
        };
    }

    private static async Task SystemctlRestartAsync(CommandContext commandContext, string unit, CancellationToken cancellationToken = default)
    {
        await SystemctlExecAsync(commandContext, unit, 3, 13, " Stopping ", " Stopped ", cancellationToken);
        await SystemctlExecAsync(commandContext, unit, 3, 13, " Starting ", " Started ", cancellationToken);
    }

    private static async Task SystemctlExecAsync(
        CommandContext commandContext,
        string unit,
        int roundsMin,
        int roundsMax,
        string ing,
        string ed,
        CancellationToken cancellationToken = default)
    {
        unit = ChatHelper.EscapeMarkdownV2CodeBlock(unit);
        var rounds = Random.Shared.Next(roundsMin, roundsMax);
        var sent = await commandContext.SendTextMessageAsync(
            $"`{WaitState}{ing}{unit}...`",
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        for (var i = 0; i < rounds; i++)
        {
            await Task.Delay(2000, cancellationToken);
            await commandContext.EditMessageTextWithRetryAsync(
                sent.Id,
                $"`{s_states[i % s_states.Length]}{ing}{unit}...`",
                ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }

        await commandContext.EditMessageTextWithRetryAsync(
            sent.Id,
            $"`{OkState}{ed}{unit}.`",
            ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private static Task<Message> SendHelpAsync(CommandContext commandContext, string errMsgMarkdownV2, CancellationToken cancellationToken = default)
        => commandContext.ReplyWithTextMessageAsync(errMsgMarkdownV2 + HelpMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);

    public static void CountSystemctlCalls(CommandContext commandContext) => commandContext.MemberOrUserData.SystemctlCommandsUsed++;
}
