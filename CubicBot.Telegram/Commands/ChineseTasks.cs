using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public static class ChineseTasks
{
    private static readonly string[] s_okAnswers =
    {
        "ok",
        "okay",
        "nice",
        "good",
        "great",
        "thanks",
        "got it",
        "all right",
        "好",
        "好的",
        "好吧",
        "吼",
        "吼的",
        "吼吧",
        "非常好",
        "非常吼",
        "行",
        "可",
        "可以",
        "没问题",
        "完全同意",
        "我觉得好",
        "我  好  了",
        "嗯",
        "嗯！",
        "嗯嗯",
        "嗯嗯！",
        "🉑",
        "👌",
        "🆗",
    };

    public static readonly ReadOnlyCollection<CubicBotCommand> Commands = new(new CubicBotCommand[]
    {
        new("ok", "👌 好的，没问题！", OKAsync, userOrMemberStatsCollector: CountOKs),
        new("assign", "📛 交给你了！", AssignAsync, userOrMemberStatsCollector: CountAssignments),
        new("unassign", "💢 不干了！", UnassignAsync, userOrMemberStatsCollector: CountUnassign),
    });

    public static Task OKAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        var randomIndex = Random.Shared.Next(s_okAnswers.Length);
        var randomOKAnswer = s_okAnswers[randomIndex];

        return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       randomOKAnswer,
                                                       replyToMessageId: message.ReplyToMessage?.MessageId,
                                                       cancellationToken: cancellationToken);
    }

    public static void CountOKs(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.OkaysSaid++;
        if (replyToUserData is not null)
        {
            replyToUserData.OkaysReceived++;
        }
    }

    public static async Task AssignAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        if (message.ReplyToMessage is null) // self assign
        {
            await botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                          $"{message.From?.FirstName}: 交  给  我  了",
                                                          cancellationToken: cancellationToken);
        }
        else // assign to someone else
        {
            await botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                          "交  给  你  了",
                                                          replyToMessageId: message.ReplyToMessage.MessageId,
                                                          cancellationToken: cancellationToken);

            var randomIndex = Random.Shared.Next(s_okAnswers.Length);
            var randomOKAnswer = s_okAnswers[randomIndex];

            await botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                          $"{message.ReplyToMessage.From?.FirstName}: {randomOKAnswer}",
                                                          replyToMessageId: message.MessageId,
                                                          cancellationToken: cancellationToken);
        }
    }

    public static void CountAssignments(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.AssignmentsCreated++;
        if (replyToUserData is not null)
        {
            replyToUserData.AssignmentsReceived++;
        }
        else
        {
            userData.AssignmentsReceived++;
        }
    }

    public static Task UnassignAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        var targetName = message.ReplyToMessage?.From?.FirstName ?? message.From?.FirstName;
        return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       $"{targetName}: 不  干  了",
                                                       replyToMessageId: message.MessageId,
                                                       cancellationToken: cancellationToken);
    }

    public static void CountUnassign(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.UnassignInitiated++;
        if (replyToUserData is not null)
        {
            replyToUserData.UnassignReceived++;
        }
        else
        {
            userData.UnassignReceived++;
        }
    }
}
