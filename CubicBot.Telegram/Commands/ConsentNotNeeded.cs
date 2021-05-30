using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class ConsentNotNeeded
    {
        public static string[] CooksAndFood => new string[]
        {
            "👩‍🍳", "🧑‍🍳", "👨‍🍳", "🍳", "🥘", "🍕",
        };

        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("cook", "😋 Who cooks the best food in the world? Me!", CookAsync),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync),
        };

        private readonly Random _random;

        public ConsentNotNeeded(Random random) => _random = random;

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
    }
}
