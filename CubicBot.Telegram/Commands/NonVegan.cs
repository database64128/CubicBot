using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Commands;

public static class NonVegan
{
    private static readonly string[] s_food =
    {
        "🍏", "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓", "🫐",
        "🍈", "🍒", "🍑", "🥭", "🍍", "🥥", "🥝", "🍅", "🍆", "🥑",
        "🥦", "🥬", "🥒", "🌶", "🫑", "🌽", "🥕", "🫒", "🧄", "🧅",
        "🥔", "🍠", "🥐", "🥯", "🍞", "🥖", "🥨", "🧀", "🥚", "🍳",
        "🧈", "🥞", "🧇", "🥓", "🥩", "🍗", "🍖", "🦴", "🌭", "🍔",
        "🍟", "🍕", "🫓", "🥪", "🥙", "🧆", "🌮", "🌯", "🫔", "🥗",
        "🥘", "🫕", "🥫", "🍝", "🍜", "🍲", "🍛", "🍣", "🍱", "🥟",
        "🦪", "🍤", "🍙", "🍚", "🍘", "🍥", "🥠", "🥮", "🍢", "🍡",
        "🍧", "🍨", "🍦", "🥧", "🧁", "🍰", "🎂", "🍮", "🍭", "🍬",
        "🍫", "🍿", "🍩", "🍪", "🌰", "🥜", "🍯",
    };

    public static readonly ReadOnlyCollection<CubicBotCommand> Commands = new(new CubicBotCommand[]
    {
        new("eat", "☃️ Do you want to eat a snowman?", EatAsync, statsCollector: CountEats),
    });

    public static Task EatAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAndRetryAsync($"{commandContext.Message.From?.FirstName} ate {replyToMessageContext.Message.From?.FirstName}! 🍴😋", cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageWithRetryAsync($"{commandContext.Message.From?.FirstName} ate {targetName}! 🍴😋", cancellationToken: cancellationToken);
        }
        else
        {
            var foodIndex = Random.Shared.Next(s_food.Length);
            var food = s_food[foodIndex];
            return commandContext.ReplyWithTextMessageAndRetryAsync(food, cancellationToken: cancellationToken);
        }
    }

    public static void CountEats(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.FoodEaten++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.EatenByOthers++;
    }
}
