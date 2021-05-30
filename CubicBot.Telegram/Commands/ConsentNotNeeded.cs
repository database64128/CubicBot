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

        public static string[] ForcedToDo => new string[]
        {
            "give up",
            "eat 💩",
            "surrender",
            "strip naked",
        };

        public CubicBotCommand[] Commands => new CubicBotCommand[]
        {
            new("cook", "😋 Who cooks the best food in the world? Me!", CookAsync),
            new("force", "☮️ Use of force not recommended.", ForceAsync),
            new("touch", "🥲 No touching.", TouchAsync),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync),
        };

        private readonly Random _random;

        public ConsentNotNeeded(Random random) => _random = random;

        public Task CookAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
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

            var cooksAndFoodIndex = _random.Next(CooksAndFood.Length);
            var cookOrFood = CooksAndFood[cooksAndFoodIndex];

            return DoActionAsync(botClient, message, argument, actionMiddle, actionEnd, cookOrFood, cancellationToken);
        }

        public Task ForceAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var index = _random.Next(ForcedToDo.Length);
            var forcedToDo = argument ?? ForcedToDo[index];

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} forced {targetMessage.From.FirstName} to {forcedToDo}.",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} was forced to {forcedToDo}.",
                                                      cancellationToken: cancellationToken);
            }
        }

        public Task TouchAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var actionIndex = _random.Next(6);
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

            var selfEmoji = _random.Next(4) switch
            {
                0 => "💦",
                1 => "💧",
                2 => "👋",
                _ => "🍆",
            };

            return DoActionAsync(botClient, message, argument, actionMiddle, actionEnd, selfEmoji, cancellationToken);
        }

        public Task FuckAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var symbol = _random.Next(3) switch
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

        private static Task DoActionAsync(ITelegramBotClient botClient, Message message, string? argument, string actionMiddle, string actionEnd, string selfEmoji, CancellationToken cancellationToken = default)
        {
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
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      selfEmoji,
                                                      replyToMessageId: message.MessageId,
                                                      cancellationToken: cancellationToken);
            }
        }
    }
}
