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

        public static string[] Firefighters => new string[]
        {
            "👩‍🚒", "👩🏻‍🚒", "👩🏼‍🚒", "👩🏽‍🚒", "👩🏾‍🚒", "👩🏿‍🚒",
            "🧑‍🚒", "🧑🏻‍🚒", "🧑🏼‍🚒", "🧑🏽‍🚒", "🧑🏾‍🚒", "🧑🏿‍🚒",
            "👨‍🚒", "👨🏻‍🚒", "👨🏼‍🚒", "👨🏽‍🚒", "👨🏾‍🚒", "👨🏿‍🚒",
        };

        public static string[] FireTrucks => new string[]
        {
            "🚒", "🧯", "🔥", "❤️‍🔥", "💥", "🚨",
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

        public static string[] CooksAndFood => new string[]
        {
            "👩‍🍳", "🧑‍🍳", "👨‍🍳", "🍳", "🥘", "🍕",
        };

        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("call_cops", "📞 Hello, this is 911. What's your emergency?", CallCopsAsync),
            new("call_fire_dept", "🚒 The flames! Beautiful, aren't they?", CallFireDeptAsync),
            new("chant", "🗣 Say it out loud!", ChantAsync),
            new("cook", "😋 Who cooks the best food in the world? Me!", CookAsync),
            new("drink", "🥤 I'm thirsty!", DrinkAsync),
            new("eat", "☃️ Do you want to eat a snowman?", EatAsync),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync),
            new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync),
            new("thanks", "😊 Say thanks to me!", SayThanksAsync),
            new("vax", "💉 Gen Z also got the vax!", VaccinateAsync),
        };

        private readonly Random _random;

        public Common(Random random) => _random = random;

        public Task CallCopsAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = _random.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = _random.Next(4);
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

        public Task CallFireDeptAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = _random.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = _random.Next(4);
                switch (type)
                {
                    case 0:
                        var firefighterIndex = _random.Next(Firefighters.Length);
                        sb.Append(Firefighters[firefighterIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = _random.Next(FireTrucks.Length);
                        sb.Append(FireTrucks[presenceIndex]);
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

        public Task CookAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var actionIndex = _random.Next(11);
            var actionMiddle = actionIndex switch
            {
                0 => " cooked ",
                1 => " cooked ",
                2 => " cooked ",
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

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName}{actionMiddle}{targetMessage.From.FirstName}{actionEnd}",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName}{actionMiddle}{targetName}{actionEnd}",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                var cooksAndFoodIndex = _random.Next(CooksAndFood.Length);
                var cookOrFood = CooksAndFood[cooksAndFoodIndex];
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      cookOrFood,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public Task DrinkAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} drank {targetMessage.From.FirstName}! 🥤🤤",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} drank {targetName}! 🥤🤤",
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
                                                      $"{message.From.FirstName} ate {targetMessage.From.FirstName}! 🍴😋",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} ate {targetName}! 🍴😋",
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
            var index = _random.Next(2);
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
                                                      $"🖕 {targetName}",
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
                                                      $"Thank you so much, {targetMessage.From.FirstName}! 😊",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"Thank you so much, {targetName}! 😊",
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      "You must either reply to a message or specify someone to thank!",
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }

        public static Task SayThanksAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "You're welcome! 🦾",
                                              replyToMessageId: message.MessageId,
                                              cancellationToken: cancellationToken);

        public static Task VaccinateAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageAsync(message.Chat.Id,
                                              "💉",
                                              replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                              cancellationToken: cancellationToken);
    }
}
