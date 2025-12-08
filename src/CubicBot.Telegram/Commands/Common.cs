using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Common
{
    private static readonly string[] s_beverages =
    [
        "🍼", "🥛", "☕️", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹",
        "🍺", "🍻", "🥂", "🥃", "🧉", "🏺", "🚰", "🥤", "🧋", "🧃",
    ];

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("apologize", "🥺 Sorry about last night.", ApologizeAsync, statsCollector: CountApologies));
        commands.Add(new("chant", "🗣 Say it out loud!", ChantAsync, statsCollector: CountChants));
        commands.Add(new("drink", "🥤 I'm thirsty!", DrinkAsync, statsCollector: CountDrinks));
        commands.Add(new("me", "🤳 What the hell am I doing?", MeAsync, statsCollector: CountMes));
        commands.Add(new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync, statsCollector: CountThankYous));
        commands.Add(new("thanks", "😊 Say thanks to me!", SayThanksAsync, statsCollector: CountThanks));
        commands.Add(new("vax", "💉 This ain't Space Needle!", VaccinateAsync, statsCollector: CountVaccinations));
    }

    public static Task ApologizeAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var apologyStart = Random.Shared.Next(5) switch
        {
            0 => "Sorry",
            1 => "I'm sorry",
            2 => "I'm so sorry",
            3 => "I apologize",
            _ => "I want to apologize",
        };

        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            var argument = commandContext.Argument switch
            {
                null => null,
                _ => $" for {commandContext.Argument}",
            };

            return replyToMessageContext.ReplyWithTextMessageAsync($"{message.From?.FirstName}: {apologyStart}{argument}, {replyToMessageContext.Message.From?.FirstName}. 🥺", cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageAsync($"{message.From?.FirstName}: {apologyStart}, {targetName}. 🥺", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.ReplyWithTextMessageAsync($"{apologyStart}, {message.From?.FirstName}. 🥺", cancellationToken: cancellationToken);
        }
    }

    public static void CountApologies(CommandContext commandContext)
    {
        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
        {
            commandContext.MemberOrUserData.ApologiesSent++;
            replyToMemberOrUserData.ApologiesReceived++;
        }
        else if (commandContext.Argument is not null)
        {
            commandContext.MemberOrUserData.ApologiesSent++;
        }
        else
        {
            commandContext.MemberOrUserData.ApologiesReceived++;
        }
    }

    public static Task ChantAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        // Assign default sentence if empty
        var argument = commandContext.Argument ?? Random.Shared.Next(9) switch
        {
            0 => "Make it happen!",
            1 => "Do it now!",
            2 => "Love wins!",
            3 => "My body, my choice!",
            4 => "No justice, no peace!",
            5 => "No Hate! No Fear! Immigrants are welcome here!",
            6 => "Climate Change is not a lie, do not let our planet die!",
            7 => "Waters rise, hear our cries, no more lies for business ties!",
            _ => "No more secrets, no more lies! No more silence that money buys!",
        };

        // Make sure it ends with '!'
        if (!argument.EndsWith('!'))
            argument = $"{argument}!";

        // CONVERT TO UPPERCASE and escape
        argument = ChatHelper.EscapeMarkdownV2Plaintext(argument.ToUpper());

        // Apply bold format and repeat
        argument = $"*{argument}*{Environment.NewLine}*{argument}*{Environment.NewLine}*{argument}*";

        return commandContext.SendTextMessageAsync(argument, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }

    public static void CountChants(CommandContext commandContext) => commandContext.MemberOrUserData.ChantsUsed++;

    public static Task DrinkAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return commandContext.ReplyWithTextMessageAsync(
                $"{commandContext.Message.From?.FirstName} drank {replyToMessageContext.Message.From?.FirstName}! 🥤🤤",
                cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageAsync(
                $"{commandContext.Message.From?.FirstName} drank {targetName}! 🥤🤤",
                cancellationToken: cancellationToken);
        }
        else
        {
            var beverageIndex = Random.Shared.Next(s_beverages.Length);
            var beverage = s_beverages[beverageIndex];
            return commandContext.ReplyWithTextMessageAsync(beverage, cancellationToken: cancellationToken);
        }
    }

    public static void CountDrinks(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.DrinksTaken++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.DrankByOthers++;
    }

    public static Task MeAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var message = commandContext.Message;
        var pronouns = commandContext.GetPronounsToUse();

        var argument = commandContext.Argument ?? Random.Shared.Next(4) switch
        {
            0 => "did nothing and fell asleep. 😴",
            1 => $"is showing off this new command {pronouns.Subject} just learned. 😎",
            2 => "got coffee for everyone in this chat. ☕",
            _ => "invoked this command by mistake. 🤪",
        };

        var text = $"* {message.From?.FirstName} {argument}";

        var entities = new MessageEntity[]
        {
            new()
            {
                Type = MessageEntityType.TextMention,
                Offset = 2,
                Length = message.From?.FirstName?.Length ?? 0,
                User = message.From,
            },
        };

        return commandContext.SendTextMessageAsync(text, entities: entities, cancellationToken: cancellationToken);
    }

    public static void CountMes(CommandContext commandContext) => commandContext.MemberOrUserData.MesUsed++;

    public static Task SayThankAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAsync($"Thank you so much, {replyToMessageContext.Message.From?.FirstName}! 😊", cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageAsync($"Thank you so much, {targetName}! 😊", cancellationToken: cancellationToken);
        }
        else
        {
            return commandContext.ReplyWithTextMessageAsync("You must either reply to a message or specify someone to thank!", cancellationToken: cancellationToken);
        }
    }

    public static void CountThankYous(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.ThankYousSent++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.ThankYousReceived++;
    }

    public static Task SayThanksAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => commandContext.ReplyWithTextMessageAsync("You're welcome! 🦾", cancellationToken: cancellationToken);

    public static void CountThanks(CommandContext commandContext) => commandContext.MemberOrUserData.ThanksSaid++;

    public static Task VaccinateAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
        => commandContext.ReplyToGrandparentWithTextMessageAsync("💉", cancellationToken: cancellationToken);

    public static void CountVaccinations(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.VaccinationShotsAdministered++;

        if (commandContext.GroupData is GroupData groupData)
            groupData.VaccinationShotsAdministered++;

        var targetUserData = commandContext.ReplyToMessageContext?.MemberOrUserData ?? commandContext.MemberOrUserData;
        targetUserData.VaccinationShotsReceived++;
    }
}
