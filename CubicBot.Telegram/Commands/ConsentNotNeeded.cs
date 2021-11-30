using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public static class ConsentNotNeeded
    {
        public static readonly string[] CooksAndFood = new string[]
        {
            "👩‍🍳", "🧑‍🍳", "👨‍🍳", "🍳", "🥘", "🍕",
        };

        public static readonly string[] ForcedToDo = new string[]
        {
            "give up",
            "eat 💩",
            "surrender",
            "strip naked",
        };

        public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
        {
            new("cook", "😋 Who cooks the best food in the world? Me!", CookAsync, userOrMemberStatsCollector: CountCooks),
            new("force", "☮️ Use of force not recommended.", ForceAsync, userOrMemberStatsCollector: CountForceUsed),
            new("touch", "🥲 No touching.", TouchAsync, userOrMemberStatsCollector: CountTouches),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync, userOrMemberStatsCollector: CountSex),
        };

        public static Task CookAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var actionIndex = Random.Shared.Next(11);
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

            var cooksAndFoodIndex = Random.Shared.Next(CooksAndFood.Length);
            var cookOrFood = CooksAndFood[cooksAndFoodIndex];

            return DoActionAsync(botClient, message, argument, actionMiddle, actionEnd, cookOrFood, cancellationToken);
        }

        public static void CountCooks(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.MealsCooked++;
            if (replyToUserData is not null)
            {
                replyToUserData.CookedByOthers++;
            }
        }

        public static Task ForceAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var index = Random.Shared.Next(ForcedToDo.Length);
            var forcedToDo = argument ?? ForcedToDo[index];

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName} forced {targetMessage.From?.FirstName} to {forcedToDo}.",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName} was forced to {forcedToDo}.",
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountForceUsed(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.ForceUsed++;
            if (replyToUserData is not null)
            {
                replyToUserData.ForcedByOthers++;
            }
        }

        public static Task TouchAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
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

            return DoActionAsync(botClient, message, argument, actionMiddle, actionEnd, selfEmoji, cancellationToken);
        }

        public static void CountTouches(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.TouchesGiven++;
            if (replyToUserData is not null)
            {
                replyToUserData.TouchesReceived++;
            }
        }

        public static Task FuckAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var symbol = Random.Shared.Next(3) switch
            {
                0 => "🍑",
                1 => "🍆",
                _ => "🖕",
            };

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               symbol,
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"🖕 {targetName}",
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               symbol,
                                                               replyToMessageId: message.MessageId,
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountSex(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.SexInitiated++;
            if (groupData is not null)
            {
                groupData.SexInitiated++;
                if (replyToUserData is not null)
                {
                    replyToUserData.SexReceived++;
                }
            }
        }

        private static Task DoActionAsync(ITelegramBotClient botClient, Message message, string? argument, string actionMiddle, string actionEnd, string selfEmoji, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName}{actionMiddle}{targetMessage.From?.FirstName}{actionEnd}",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName}{actionMiddle}{targetName}{actionEnd}",
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               selfEmoji,
                                                               replyToMessageId: message.MessageId,
                                                               cancellationToken: cancellationToken);
            }
        }
    }
}
