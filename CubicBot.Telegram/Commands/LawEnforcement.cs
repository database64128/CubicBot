using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public static class LawEnforcement
{
    private static readonly string[] s_policeOfficers =
    {
        "👮‍♀️", "👮🏻‍♀️", "👮🏼‍♀️", "👮🏽‍♀️", "👮🏾‍♀️", "👮🏿‍♀️",
        "👮", "👮🏻", "👮🏼", "👮🏽", "👮🏾", "👮🏿",
        "👮‍♂️", "👮🏻‍♂️", "👮🏼‍♂️", "👮🏽‍♂️", "👮🏾‍♂️", "👮🏿‍♂️",
    };

    private static readonly string[] s_policePresence =
    {
        "🚓", "🚔", "🚨",
    };

    private static readonly string[] s_reasonsForArrest =
    {
        "trespassing ⛔",
        "shoplifting 🛍️",
        "stealing a vibrator 📳",
        "masturbating in public 💦",
        "making too much noise during sex 💋",
    };

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("call_cops", "📞 Hello, this is 911. What's your emergency?", CallCopsAsync, statsCollector: CountCopCalls));
        commands.Add(new("arrest", "🚓 Do I still have the right to remain silent?", ArrestAsync, statsCollector: CountArrests));
        commands.Add(new("guilty_or_not", "🧑‍⚖️ Yes, your honor.", GuiltyOrNotAsync, statsCollector: CountLawsuits));
        commands.Add(new("overthrow", "🏛️ Welcome to Capitol Hill!", OverthrowAsync, statsCollector: CountOverthrows));
    }

    public static Task CallCopsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
        var count = Random.Shared.Next(24, 97);

        for (var i = 0; i < count; i++)
        {
            var type = Random.Shared.Next(4);
            switch (type)
            {
                case 0:
                    var officerIndex = Random.Shared.Next(s_policeOfficers.Length);
                    sb.Append(s_policeOfficers[officerIndex]);
                    break;
                case 1:
                case 2:
                case 3:
                    var presenceIndex = Random.Shared.Next(s_policePresence.Length);
                    sb.Append(s_policePresence[presenceIndex]);
                    break;
            }
        }

        return commandContext.SendTextMessageWithRetryAsync(sb.ToString(), cancellationToken: cancellationToken);
    }

    public static void CountCopCalls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.CopCallsMade++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.CopCallsMade++;
        }
    }

    public static Task ArrestAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var reason = commandContext.Argument ?? s_reasonsForArrest[Random.Shared.Next(s_reasonsForArrest.Length)];

        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAndRetryAsync($"{replyToMessageContext.Message.From?.FirstName} has been arrested for {reason}.", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.SendTextMessageWithRetryAsync($"{commandContext.Message.From?.FirstName} has been arrested for {reason}.", cancellationToken: cancellationToken);
        }
    }

    public static void CountArrests(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.ArrestsMade++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.ArrestsMade++;

        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;
        targetUserData.ArrestsReceived++;
    }

    public static Task GuiltyOrNotAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var verdict = Random.Shared.Next(3) switch
        {
            0 => "Not guilty.",
            1 => "Guilty on all counts.",
            _ => "The jury failed to reach a consensus.",
        };

        return commandContext.SendTextMessageWithRetryAsync(
            verdict,
            replyToMessageId: commandContext.Message.ReplyToMessage?.MessageId,
            cancellationToken: cancellationToken);
    }

    public static void CountLawsuits(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.VerdictsGiven++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.VerdictsGiven++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.VerdictsReceived++;
    }

    public static async Task OverthrowAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var targetMessageContext = commandContext.ReplyToMessageContext ?? commandContext;
        var targetMessage = targetMessageContext.Message;
        var pronouns = targetMessageContext.GetPronounsToUse();

        string title;

        try
        {
            title = await commandContext.BotClient.GetChatMemberAsync(message.Chat.Id, targetMessageContext.UserId, cancellationToken) switch
            {
                ChatMemberAdministrator admin => admin.CustomTitle ?? "admin",
                ChatMemberOwner owner => owner.CustomTitle ?? "owner",
                _ => "member",
            };
        }
        catch
        {
            title = "member";
        }

        var text = Random.Shared.Next(4) switch // 25% success rate
        {
            0 => $"{targetMessage.From?.FirstName} was overthrown by {message.From?.FirstName} and stripped of {pronouns.PossessiveDeterminer} {title} title. 🔫",
            _ => $"{message.From?.FirstName} failed to overthrow {targetMessage.From?.FirstName} and was taken into custody by the FBI. {s_policeOfficers[Random.Shared.Next(s_policeOfficers.Length)]}",
        };

        await commandContext.SendTextMessageWithRetryAsync(text, cancellationToken: cancellationToken);
    }

    public static void CountOverthrows(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.OverthrowAttempts++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.OverthrowAttempts++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.OverthrowAttemptsReceived++;
    }
}
