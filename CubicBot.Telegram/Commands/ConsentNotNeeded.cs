using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public static class ConsentNotNeeded
{
    private static readonly string[] s_cooksAndFood =
    {
        "👩‍🍳", "🧑‍🍳", "👨‍🍳", "🍳", "🥘", "🍕",
    };

    private static readonly string[] s_forcedToDo =
    {
        "give up",
        "eat 💩",
        "surrender",
        "strip naked",
    };

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("cook", "😋 Who cooks the best food in the world? Me!", CookAsync, statsCollector: CountCooks));
        commands.Add(new("throw", "🥺 Throw me a bone.", ThrowAsync, statsCollector: CountThrows));
        commands.Add(new("catch", "😏 Catch me if you can.", CatchAsync, statsCollector: CountCatches));
        commands.Add(new("force", "☮️ Use of force not recommended.", ForceAsync, statsCollector: CountForceUsed));
        commands.Add(new("touch", "🥲 No touching.", TouchAsync, statsCollector: CountTouches));
        commands.Add(new("fuck", "😍 Feeling horny as fuck?", FuckAsync, statsCollector: CountSex));
    }

    public static Task CookAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var actionIndex = Random.Shared.Next(11);
        var actionMiddle = actionIndex switch
        {
            0 or 1 or 2 => " cooked ",
            3 => " turned ",
            4 => " grilled ",
            5 => " squeezed juice out of ",
            6 => " mixed in ",
            7 => " milked ",
            8 => " made tea with ",
            9 => " fired ",
            10 => " introduced ",
            _ => $"❌ Error: Unexpected action index {actionIndex}",
        };
        var actionEnd = actionIndex switch
        {
            0 => " as breakfast! 🥣",
            1 => " as lunch! 🍴",
            2 => " as dinner! 🍽️",
            3 => " into dessert! 🍰",
            4 => " during the barbecue! 🍖",
            5 => "! 🍹",
            6 => " to make a smoothie! 🥤",
            7 => "! 🥛",
            8 => "! 🫖",
            9 => " from this chat! 🔥",
            10 => " to Tim Cook and they had a threesome. 💋",
            _ => $"❌ Error: Unexpected action index {actionIndex}",
        };

        var cooksAndFoodIndex = Random.Shared.Next(s_cooksAndFood.Length);
        var cookOrFood = s_cooksAndFood[cooksAndFoodIndex];

        return DoActionAsync(commandContext, actionMiddle, actionEnd, cookOrFood, cancellationToken);
    }

    public static void CountCooks(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.MealsCooked++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.CookedByOthers++;
    }

    public static Task ThrowAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var firstname = message.ReplyToMessage?.From?.FirstName ?? commandContext.Argument ?? message.From?.FirstName;
        var text = Random.Shared.Next(11) switch
        {
            0 => $"{firstname} was thrown into the trash and buried in a landfill. 🗑️",
            1 => $"{firstname} was thrown at a wall and smashed into pieces. 🧱",
            2 => $"{firstname} was thrown under a bus. 🚌",
            3 => $"{firstname} was thrown out of a window and got hit by a truck. 🚚",
            4 => $"{firstname} was thrown into an escape room and died from a panic attack. 🚪",
            5 => $"{firstname} was thrown into a volcano and burned to death. 🌋",
            6 => $"{firstname} was thrown into a black hole and disappeared. 🌌",
            7 => $"{firstname} was thrown into a pit of snakes and died from a snake bite. 🐍",
            8 => $"{firstname} was thrown into a pit of spiders and died from a spider bite. 🕷️",
            9 => $"{firstname} was thrown into a pit of crocodiles and died from a crocodile bite. 🐊",
            _ => $"{firstname} was thrown into a pit of sharks and died from a shark bite. 🦈",
        };
        return commandContext.SendTextMessageWithRetryAsync(text, cancellationToken: cancellationToken);
    }

    public static void CountThrows(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.PersonsThrown++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.ThrownByOthers++;
    }

    public static Task CatchAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var pronouns = commandContext.GetPronounsToUse();
        var catchPhrase = message.ReplyToMessage?.From switch
        {
            User replyToUser => $"caught {replyToUser.FirstName}",
            null => "was caught",
        };

        var argument = commandContext.Argument ?? Random.Shared.Next(7) switch
        {
            0 => "by surprise. 😲",
            1 => "in a box and launched into space. 🚀",
            2 => "eating food picked up from the floor. 🍽️",
            3 => $"stalking {pronouns.Object} on Instagram. 📷",
            4 => $"sexting {pronouns.PossessiveDeterminer} best friend. 💋",
            5 => $"naked in {pronouns.PossessiveDeterminer} bed and was turned on by what {pronouns.Subject} saw. 😍",
            _ => $"masturbating to {pronouns.PossessiveDeterminer} profile picture. 💦",
        };

        return commandContext.SendTextMessageWithRetryAsync($"{message.From?.FirstName} {catchPhrase} {argument}", cancellationToken: cancellationToken);
    }

    public static void CountCatches(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.PersonsCaught++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.CaughtByOthers++;
    }

    public static Task ForceAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var index = Random.Shared.Next(s_forcedToDo.Length);
        var forcedToDo = commandContext.Argument ?? s_forcedToDo[index];
        var message = commandContext.Message;

        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAndRetryAsync($"{message.From?.FirstName} forced {replyToMessageContext.Message.From?.FirstName} to {forcedToDo}.", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.SendTextMessageWithRetryAsync($"{message.From?.FirstName} was forced to {forcedToDo}.", cancellationToken: cancellationToken);
        }
    }

    public static void CountForceUsed(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.ForceUsed++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.ForcedByOthers++;
    }

    public static Task TouchAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var actionIndex = Random.Shared.Next(6);
        var actionMiddle = actionIndex switch
        {
            0 => " patted ",
            1 => " patted ",
            2 => " touched ",
            3 => " held ",
            4 => " was scared and held ",
            5 => " touched ",
            _ => $"❌ Error: Unexpected action index {actionIndex}",
        };
        var actionEnd = actionIndex switch
        {
            0 => " on the head. 😃",
            1 => " on the shoulder. 😃",
            2 => "'s hand in delight. 🖐️",
            3 => "'s hand in delight. 🤲",
            4 => "'s hand. 😨",
            5 => "'s body with passion. 😍",
            _ => $"❌ Error: Unexpected action index {actionIndex}",
        };

        var selfEmoji = Random.Shared.Next(4) switch
        {
            0 => "💦",
            1 => "💧",
            2 => "👋",
            _ => "🍆",
        };

        return DoActionAsync(commandContext, actionMiddle, actionEnd, selfEmoji, cancellationToken);
    }

    public static void CountTouches(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.TouchesGiven++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.TouchesReceived++;
    }

    public static Task FuckAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var symbol = Random.Shared.Next(3) switch
        {
            0 => "🍑",
            1 => "🍆",
            _ => "🖕",
        };

        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAndRetryAsync(symbol, cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageWithRetryAsync($"🖕 {targetName}", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.ReplyWithTextMessageAndRetryAsync(symbol, cancellationToken: cancellationToken);
        }
    }

    public static void CountSex(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.SexInitiated++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.SexInitiated++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.SexReceived++;
    }

    private static Task DoActionAsync(CommandContext commandContext, string actionMiddle, string actionEnd, string selfEmoji, CancellationToken cancellationToken = default)
    {
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAndRetryAsync($"{commandContext.Message.From?.FirstName}{actionMiddle}{replyToMessageContext.Message.From?.FirstName}{actionEnd}", cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageWithRetryAsync($"{commandContext.Message.From?.FirstName}{actionMiddle}{targetName}{actionEnd}", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.ReplyWithTextMessageAndRetryAsync(selfEmoji, cancellationToken: cancellationToken);
        }
    }
}
