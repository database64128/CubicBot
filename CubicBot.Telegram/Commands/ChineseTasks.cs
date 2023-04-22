using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

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
        new("ok", "👌 好的，没问题！", OKAsync, statsCollector: CountOKs),
        new("assign", "📛 交给你了！", AssignAsync, statsCollector: CountAssignments),
        new("unassign", "💢 不干了！", UnassignAsync, statsCollector: CountUnassign),
    });

    public static Task OKAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var randomIndex = Random.Shared.Next(s_okAnswers.Length);
        var randomOKAnswer = s_okAnswers[randomIndex];
        return commandContext.SendTextMessageWithRetryAsync(
            randomOKAnswer,
            replyToMessageId: commandContext.Message.ReplyToMessage?.MessageId,
            cancellationToken: cancellationToken);
    }

    public static void CountOKs(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.OkaysSaid++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.OkaysReceived++;
    }

    public static async Task AssignAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var replyToMessageContext = commandContext.ReplyToMessageContext;
        if (replyToMessageContext is null) // self assign
        {
            await commandContext.SendTextMessageWithRetryAsync($"{commandContext.Message.From?.FirstName}: 交  给  我  了", cancellationToken: cancellationToken);
        }
        else // assign to someone else
        {
            await replyToMessageContext.ReplyWithTextMessageAndRetryAsync("交  给  你  了", cancellationToken: cancellationToken);

            var randomIndex = Random.Shared.Next(s_okAnswers.Length);
            var randomOKAnswer = s_okAnswers[randomIndex];

            await commandContext.ReplyWithTextMessageAndRetryAsync($"{replyToMessageContext.Message.From?.FirstName}: {randomOKAnswer}", cancellationToken: cancellationToken);
        }
    }

    public static void CountAssignments(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.AssignmentsCreated++;

        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;
        targetUserData.AssignmentsReceived++;
    }

    public static Task UnassignAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var targetName = message.ReplyToMessage?.From?.FirstName ?? message.From?.FirstName;
        return commandContext.ReplyWithTextMessageAndRetryAsync($"{targetName}: 不  干  了", cancellationToken: cancellationToken);
    }

    public static void CountUnassign(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.UnassignInitiated++;

        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;
        targetUserData.UnassignReceived++;
    }
}
