using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Systemd
{
    private static readonly string[] s_states =
    {
        "[***   ]",
        "[ ***  ]",
        "[  *** ]",
        "[   ***]",
        "[*   **]",
        "[**   *]",
    };

    private const string WaitState = "[ WAIT ]";

    private const string OkState = "[  OK  ]";

    private const string HelpMarkdownV2 = @"
Usage: `/systemctl <command> [unit]`
Supported commands: *start*, *stop*, *restart*, *reload*\.
Reply to a message to use the sender's name as the unit\.";

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

        string command, unit;
        SystemdUnitType unitType = SystemdUnitType.Service;

        var spacePos = argument.IndexOf(' ');
        if (spacePos == -1 && commandContext.Message.ReplyToMessage?.From?.FirstName is string firstname)
        {
            command = argument;
            unit = firstname;
        }
        else if (spacePos > 0 && spacePos < argument.Length - 1)
        {
            command = argument[..spacePos];
            unit = argument[(spacePos + 1)..];
            var dotPos = unit.LastIndexOf('.');
            if (dotPos != -1)
            {
                unitType = unit[dotPos..] switch
                {
                    ".timer" => SystemdUnitType.Timer,
                    ".target" => SystemdUnitType.Target,
                    _ => SystemdUnitType.Service,
                };
            }
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
        var sent = await commandContext.SendTextMessageWithRetryAsync(
            $"`{WaitState}{ing}{unit}...`",
            ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        for (var i = 0; i < rounds; i++)
        {
            await Task.Delay(2000, cancellationToken);
            await commandContext.EditMessageTextWithRetryAsync(
                sent.MessageId,
                $"`{s_states[i % s_states.Length]}{ing}{unit}...`",
                ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }

        await commandContext.EditMessageTextWithRetryAsync(
            sent.MessageId,
            $"`{OkState}{ed}{unit}.`",
            ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private static Task SendHelpAsync(CommandContext commandContext, string errMsgMarkdownV2, CancellationToken cancellationToken = default)
        => commandContext.ReplyWithTextMessageAndRetryAsync(errMsgMarkdownV2 + HelpMarkdownV2, ParseMode.MarkdownV2, cancellationToken: cancellationToken);

    public static void CountSystemctlCalls(CommandContext commandContext) => commandContext.MemberOrUserData.SystemctlCommandsUsed++;
}
