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
            new("throw", "🥺 Throw me a bone.", ThrowAsync, userOrMemberStatsCollector: CountThrows),
            new("catch", "😏 Catch me if you can.", CatchAsync, userOrMemberStatsCollector: CountCatches),
            new("force", "☮️ Use of force not recommended.", ForceAsync, userOrMemberStatsCollector: CountForceUsed),
            new("touch", "🥲 No touching.", TouchAsync, userOrMemberStatsCollector: CountTouches),
            new("fuck", "😍 Feeling horny as fuck?", FuckAsync, userOrMemberStatsCollector: CountSex),
        };

        public static Task CookAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
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

        public static Task ThrowAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var firstname = message.ReplyToMessage?.From?.FirstName ?? argument ?? message.From?.FirstName;
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
            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id, text, cancellationToken: cancellationToken);
        }

        public static void CountThrows(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.PersonsThrown++;
            if (replyToUserData is not null)
            {
                replyToUserData.ThrownByOthers++;
            }
        }

        public static Task CatchAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var userId = ChatHelper.GetMessageSenderId(message);
            var groupId = ChatHelper.GetChatGroupId(message.Chat);
            var pronounSubject = data.GetPronounSubject(userId, groupId);
            var pronounObject = data.GetPronounObject(userId, groupId);
            var pronounPossessiveDeterminer = data.GetPronounPossessiveDeterminer(userId, groupId);
            var catchTargetFirstname = message.ReplyToMessage?.From?.FirstName;
            var catchPhrase = catchTargetFirstname switch
            {
                null => "was caught",
                _ => $"caught {catchTargetFirstname}",
            };

            argument ??= Random.Shared.Next(7) switch
            {
                0 => "by surprise. 😲",
                1 => "in a box and launched into space. 🚀",
                2 => "eating food picked up from the floor. 🍽️",
                3 => $"stalking {pronounObject} on Instagram. 📷",
                4 => $"sexting {pronounPossessiveDeterminer} best friend. 💋",
                5 => $"naked in {pronounPossessiveDeterminer} bed and was turned on by what {pronounSubject} saw. 😍",
                _ => $"masturbating to {pronounPossessiveDeterminer} profile picture. 💦",
            };

            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id, $"{message.From?.FirstName} {catchPhrase} {argument}", cancellationToken: cancellationToken);
        }

        public static void CountCatches(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.PersonsCaught++;
            if (replyToUserData is not null)
            {
                replyToUserData.CaughtByOthers++;
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
