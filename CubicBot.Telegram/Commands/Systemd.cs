using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Systemd
{
    public static readonly CubicBotCommand[] Commands =
    {
        new("systemctl", "➡️ systemctl <command> [unit]", SystemctlAsync, userOrMemberStatsCollector: CountSystemctlCalls),
    };

    public static readonly string[] States =
    {
        "[***   ]",
        "[ ***  ]",
        "[  *** ]",
        "[   ***]",
        "[*   **]",
        "[**   *]",
    };

    public const string WaitState = "[ WAIT ]";

    public const string OkState = "[  OK  ]";

    public const string HelpMarkdownV2 = @"
Usage: `/systemctl <command> [unit]`
Supported commands: *start*, *stop*, *restart*, *reload*\.
Reply to a message to use the sender's name as the unit\.";

    public static Task SystemctlAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(argument))
        {
            return SendHelpAsync(botClient, message, @"Missing command\.", cancellationToken);
        }

        string command, unit;
        SystemdUnitType unitType = SystemdUnitType.Service;

        var spacePos = argument.IndexOf(' ');
        if (spacePos == -1 && message.ReplyToMessage?.From?.FirstName is string firstname)
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
            return SendHelpAsync(botClient, message, @"Missing unit\.", cancellationToken);
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
            return SendHelpAsync(botClient, message, unsupportedErrMsgMarkdownV2, cancellationToken);
        }

        return command switch
        {
            "start" => SystemctlExecAsync(botClient, message, unit, 3, 13, " Starting ", unitType is SystemdUnitType.Target ? " Reached " : " Started ", cancellationToken),
            "stop" => SystemctlExecAsync(botClient, message, unit, 3, 13, " Stopping ", " Stopped ", cancellationToken),
            "reload" => SystemctlExecAsync(botClient, message, unit, 3, 6, " Reloading ", " Reloaded ", cancellationToken),
            "restart" => SystemctlRestartAsync(botClient, message, unit, cancellationToken),
            _ => SendHelpAsync(botClient, message, @"Invalid command\.", cancellationToken),
        };
    }

    private static async Task SystemctlRestartAsync(ITelegramBotClient botClient, Message message, string unit, CancellationToken cancellationToken = default)
    {
        await SystemctlExecAsync(botClient, message, unit, 3, 13, " Stopping ", " Stopped ", cancellationToken);
        await SystemctlExecAsync(botClient, message, unit, 3, 13, " Starting ", " Started ", cancellationToken);
    }

    private static async Task SystemctlExecAsync(
        ITelegramBotClient botClient,
        Message message,
        string unit,
        int roundsMin,
        int roundsMax,
        string ing,
        string ed,
        CancellationToken cancellationToken = default)
    {
        unit = ChatHelper.EscapeMarkdownV2CodeBlock(unit);
        var rounds = Random.Shared.Next(roundsMin, roundsMax);
        var sent = await botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                                 $"`{WaitState}{ing}{unit}...`",
                                                                 ParseMode.MarkdownV2,
                                                                 cancellationToken: cancellationToken);

        for (var i = 0; i < rounds; i++)
        {
            await Task.Delay(2000, cancellationToken);
            await botClient.EditMessageTextWithRetryAsync(sent.Chat.Id,
                                                          sent.MessageId,
                                                          $"`{States[i % States.Length]}{ing}{unit}...`",
                                                          ParseMode.MarkdownV2,
                                                          cancellationToken: cancellationToken);
        }

        await botClient.EditMessageTextWithRetryAsync(sent.Chat.Id,
                                                      sent.MessageId,
                                                      $"`{OkState}{ed}{unit}.`",
                                                      ParseMode.MarkdownV2,
                                                      cancellationToken: cancellationToken);
    }

    private static Task SendHelpAsync(ITelegramBotClient botClient, Message message, string errMsgMarkdownV2, CancellationToken cancellationToken = default)
        => botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                   errMsgMarkdownV2 + HelpMarkdownV2,
                                                   ParseMode.MarkdownV2,
                                                   replyToMessageId: message.MessageId,
                                                   cancellationToken: cancellationToken);

    public static void CountSystemctlCalls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData) => userData.SystemctlCommandsUsed++;
}
