using CubicBot.Telegram.Utils;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public class Common
    {
        public static string[] PoliceOfficers => new string[]
        {
            "👮‍♀️", "👮🏻‍♀️", "👮🏼‍♀️", "👮🏽‍♀️", "👮🏾‍♀️", "👮🏿‍♀️",
            "👮", "👮🏻", "👮🏼", "👮🏽", "👮🏾", "👮🏿",
            "👮‍♂️", "👮🏻‍♂️", "👮🏼‍♂️", "👮🏽‍♂️", "👮🏾‍♂️", "👮🏿‍♂️",
        };

        public static string[] PolicePresence => new string[]
        {
            "🚓", "🚔", "🚨",
        };

        public static string[] Beverages => new string[]
        {
            "🍼", "🥛", "☕️", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹",
            "🍺", "🍻", "🥂", "🧉", "🏺", "🚰", "🧋", "🧃",
        };

        public static string[] Food => new string[]
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

        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("call_cops", "📞 Hello, this is 911. What's your emergency?", CallCopsAsync),
            new("chant", "🗣 Say it out loud!", ChantAsync),
            new("drink", "🥤 I'm thirsty!", DrinkAsync),
            new("eat", "☃️ Do you want to eat a snowman?", EatAsync),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync),
            new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync),
            new("thanks", "😊 Say thanks to me!", SayThanksAsync),
        };

        private readonly Random _random;

        public Common(Random random) => _random = random;

        public Task CallCopsAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = _random.Next(24, 96);

            for (var i = 0; i < count; i++)
            {
                var type = _random.Next(3);
                switch (type)
                {
                    case 0:
                        var officerIndex = _random.Next(PoliceOfficers.Length);
                        sb.Append(PoliceOfficers[officerIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = _random.Next(PolicePresence.Length);
                        sb.Append(PolicePresence[presenceIndex]);
                        break;
                }
            }

            return botClient.SendTextMessageAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
        }

        public static Task ChantAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            // Assign default sentence if empty
            if (string.IsNullOrEmpty(argument))
                argument = "Make it happen";

            // Make sure it ends with '!'
            if (!argument.EndsWith('!'))
                argument = $"{argument}!";

            // CONVERT TO UPPERCASE and escape
            argument = ChatHelper.EscapePlaintextAsMarkdownV2(argument.ToUpper());

            // Apply bold format and repeat
            argument = $"*{argument}*{Environment.NewLine}*{argument}*{Environment.NewLine}*{argument}*";

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  argument,
                                                  parseMode: ParseMode.MarkdownV2,
                                                  cancellationToken: cancellationToken);
        }

        public Task DrinkAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"{message.From.FirstName} drank {targetMessage.From.FirstName}! 🥤🤤"),
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"{message.From.FirstName} drank {targetName}! 🥤🤤"),
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                var beverageIndex = _random.Next(Beverages.Length);
                var beverage = Beverages[beverageIndex];
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      beverage,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public Task EatAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"{message.From.FirstName} ate {targetMessage.From.FirstName}! 🍴😋"),
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"{message.From.FirstName} ate {targetName}! 🍴😋"),
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                var foodIndex = _random.Next(Food.Length);
                var food = Food[foodIndex];
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      food,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public Task FuckAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var index = _random.Next(1);
            var symbol = index switch
            {
                0 => "🍑",
                1 => "🍆",
                _ => "🖕",
            };

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      symbol,
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"🖕 {targetName}"),
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      symbol,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task SayThankAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"Thank you so much, {targetMessage.From.FirstName}! 😊"),
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      ChatHelper.EscapePlaintextAsMarkdownV2($"Thank you so much, {targetName}! 😊"),
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      @"You must either reply to a message or specify someone to thank\!",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task SayThanksAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              @"You're welcome\! 🦾",
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);
    }
}
