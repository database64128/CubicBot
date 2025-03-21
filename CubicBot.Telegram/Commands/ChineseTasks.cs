﻿using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;

namespace CubicBot.Telegram.Commands;

public static class ChineseTasks
{
    private static readonly string[] s_okAnswers =
    [
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
    ];

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("ok", "👌 好的，没问题！", OKAsync, statsCollector: CountOKs));
        commands.Add(new("assign", "📛 交给你了！", AssignAsync, statsCollector: CountAssignments));
        commands.Add(new("unassign", "💢 不干了！", UnassignAsync, statsCollector: CountUnassign));
    }

    public static Task OKAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var randomIndex = Random.Shared.Next(s_okAnswers.Length);
        var randomOKAnswer = s_okAnswers[randomIndex];
        return commandContext.ReplyToGrandparentWithTextMessageAsync(randomOKAnswer, cancellationToken: cancellationToken);
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
            await commandContext.SendTextMessageAsync($"{commandContext.Message.From?.FirstName}: 交  给  我  了", cancellationToken: cancellationToken);
        }
        else // assign to someone else
        {
            await replyToMessageContext.ReplyWithTextMessageAsync("交  给  你  了", cancellationToken: cancellationToken);

            var randomIndex = Random.Shared.Next(s_okAnswers.Length);
            var randomOKAnswer = s_okAnswers[randomIndex];

            await commandContext.ReplyWithTextMessageAsync($"{replyToMessageContext.Message.From?.FirstName}: {randomOKAnswer}", cancellationToken: cancellationToken);
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
        return commandContext.ReplyWithTextMessageAsync($"{targetName}: 不  干  了", cancellationToken: cancellationToken);
    }

    public static void CountUnassign(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.UnassignInitiated++;

        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;
        targetUserData.UnassignReceived++;
    }
}
