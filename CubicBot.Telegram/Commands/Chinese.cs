using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Commands;

public static class Chinese
{
    private static readonly string[] s_questions =
    [
        "你发这些什么目的？",
        "谁指使你的？",
        "你的动机是什么？",
        "你取得有关部门许可了吗？",
        "法律法规容许你发了吗？",
        "你背后是谁？",
        "发这些想干什么？",
        "你想颠覆什么？",
        "你要破坏什么？",
        "你最好收回！",
        "你在影射谁？",
        "你在影射什么？",
        "互联网不是法外之地！",
        "你的言论很危险！",
        "寻衅滋事？",
        "我们要移交法办！",
        "你被举报了！",
        "境外势力！",
        "这是不是你发的？",
        "我劝你谨言慎行！",
        "谁让你发的？",
        "你要打倒谁？",
    ];

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("interrogate", "🔫 开门，查水表！", InterrogateAsync, statsCollector: CountInterrogations));
    }

    public static Task InterrogateAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var randomIndex = Random.Shared.Next(s_questions.Length);
        var randomQuestion = s_questions[randomIndex];

        return commandContext.SendTextMessageWithRetryAsync(
            randomQuestion,
            replyToMessageId: commandContext.Message.ReplyToMessage?.MessageId,
            cancellationToken: cancellationToken);
    }

    public static void CountInterrogations(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.InterrogationsInitiated++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.InterrogationsInitiated++;

        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
            CountInterrogatedByOthers(replyToMessageContext);
    }

    private static void CountInterrogatedByOthers(MessageContext messageContext)
    {
        messageContext.MemberOrUserData.InterrogatedByOthers++;

        if (messageContext.GroupData is GroupData groupData)
            groupData.InterrogatedByOthers++;
    }
}
