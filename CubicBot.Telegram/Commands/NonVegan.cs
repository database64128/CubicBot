using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;

namespace CubicBot.Telegram.Commands;

public static class NonVegan
{
    private static readonly string[] s_food =
    [
        "🍇", "🍈", "🍉", "🍊", "🍋", "🍌", "🍍", "🥭", "🍎", "🍏",
        "🍐", "🍑", "🍒", "🍓", "🫐", "🥝", "🍅", "🫒", "🥥", "🥑",
        "🍆", "🥔", "🥕", "🌽", "🌶", "🫑", "🥒", "🥬", "🥦", "🧄",
        "🧅", "🥜", "🫘", "🌰", "🫚", "🫛", "🍞", "🥐", "🥖", "🫓",
        "🥨", "🥯", "🥞", "🧇", "🧀", "🍖", "🍗", "🥩", "🥓", "🍔",
        "🍟", "🍕", "🌭", "🥪", "🌮", "🌯", "🫔", "🥙", "🧆", "🥚",
        "🍳", "🥘", "🍲", "🫕", "🥣", "🥗", "🍿", "🧈", "🧂", "🥫",
        "🍱", "🍘", "🍙", "🍚", "🍛", "🍜", "🍝", "🍠", "🍢", "🍣",
        "🍤", "🍥", "🥮", "🍡", "🥟", "🥠", "🥡", "🦀", "🦞", "🦐",
        "🦑", "🦪", "🍦", "🍧", "🍨", "🍩", "🍪", "🎂", "🍰", "🧁",
        "🥧", "🍫", "🍬", "🍭", "🍮", "🍯", "🦴", "💩", "🤮", "🥵",
    ];

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("eat", "☃️ Do you want to eat a snowman?", EatAsync, statsCollector: CountEats));
    }

    public static Task EatAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            return replyToMessageContext.ReplyWithTextMessageAsync($"{commandContext.Message.From?.FirstName} ate {replyToMessageContext.Message.From?.FirstName}! 🍴😋", cancellationToken: cancellationToken);
        }
        else if (commandContext.Argument is string targetName)
        {
            return commandContext.SendTextMessageAsync($"{commandContext.Message.From?.FirstName} ate {targetName}! 🍴😋", cancellationToken: cancellationToken);
        }
        else
        {
            var foodIndex = Random.Shared.Next(s_food.Length);
            var food = s_food[foodIndex];
            return commandContext.ReplyWithTextMessageAsync(food, cancellationToken: cancellationToken);
        }
    }

    public static void CountEats(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.FoodEaten++;

        if (commandContext.ReplyToMessageContext?.MemberOrUserData is UserData replyToMemberOrUserData)
            replyToMemberOrUserData.EatenByOthers++;
    }
}
