using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands
{
    public static class Common
    {
        public static readonly string[] Beverages = new string[]
        {
            "🍼", "🥛", "☕️", "🫖", "🍵", "🍶", "🍾", "🍷", "🍸", "🍹",
            "🍺", "🍻", "🥂", "🧉", "🏺", "🚰", "🧋", "🧃",
        };

        public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
        {
            new("apologize", "🥺 Sorry about last night.", ApologizeAsync, userOrMemberStatsCollector: CountApologies),
            new("chant", "🗣 Say it out loud!", ChantAsync, userOrMemberStatsCollector: CountChants),
            new("drink", "🥤 I'm thirsty!", DrinkAsync, userOrMemberStatsCollector: CountDrinks),
            new("me", "🤳 What the hell am I doing?", MeAsync, userOrMemberStatsCollector: CountMes),
            new("thank", "🦃 Reply to or mention the name of the person you would like to thank.", SayThankAsync, userOrMemberStatsCollector: CountThankYous),
            new("thanks", "😊 Say thanks to me!", SayThanksAsync, userOrMemberStatsCollector: CountThanks),
            new("vax", "💉 Gen Z also got the vax!", VaccinateAsync, userOrMemberStatsCollector: CountVaccinations),
        };

        public static Task ApologizeAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var apologyStart = Random.Shared.Next(5) switch
            {
                0 => "Sorry",
                1 => "I'm sorry",
                2 => "I'm so sorry",
                3 => "I apologize",
                _ => "I want to apologize",
            };

            if (message.ReplyToMessage is Message targetMessage)
            {
                if (!string.IsNullOrEmpty(argument))
                    argument = $" for {argument}";

                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName}: {apologyStart}{argument}, {targetMessage.From?.FirstName}. 🥺",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName}: {apologyStart}, {targetName}. 🥺",
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{apologyStart}, {message.From?.FirstName}. 🥺",
                                                               replyToMessageId: message.MessageId,
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountApologies(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            if (replyToUserData is not null)
            {
                userData.ApologiesSent++;
                replyToUserData.ApologiesReceived++;
            }
            else if (argument is not null)
            {
                userData.ApologiesSent++;
            }
            else
            {
                userData.ApologiesReceived++;
            }
        }

        public static Task ChantAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            // Assign default sentence if empty
            if (string.IsNullOrEmpty(argument))
            {
                argument = Random.Shared.Next(9) switch
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
            }

            // Make sure it ends with '!'
            if (!argument.EndsWith('!'))
                argument = $"{argument}!";

            // CONVERT TO UPPERCASE and escape
            argument = ChatHelper.EscapeMarkdownV2Plaintext(argument.ToUpper());

            // Apply bold format and repeat
            argument = $"*{argument}*{Environment.NewLine}*{argument}*{Environment.NewLine}*{argument}*";

            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           argument,
                                                           parseMode: ParseMode.MarkdownV2,
                                                           cancellationToken: cancellationToken);
        }

        public static void CountChants(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData) => userData.ChantsUsed++;

        public static Task DrinkAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName} drank {targetMessage.From?.FirstName}! 🥤🤤",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName} drank {targetName}! 🥤🤤",
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                var beverageIndex = Random.Shared.Next(Beverages.Length);
                var beverage = Beverages[beverageIndex];
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               beverage,
                                                               replyToMessageId: message.MessageId,
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountDrinks(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.DrinksTaken++;
            if (replyToUserData is not null)
            {
                replyToUserData.DrankByOthers++;
            }
        }

        public static Task MeAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var userId = ChatHelper.GetMessageSenderId(message);
            var groupId = ChatHelper.GetChatGroupId(message.Chat);
            var pronounSubject = data.GetPronounSubject(userId, groupId);

            argument ??= Random.Shared.Next(4) switch
            {
                0 => "did nothing and fell asleep. 😴",
                1 => $"is showing off this new command {pronounSubject} just learned. 😎",
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

            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           text,
                                                           entities: entities,
                                                           cancellationToken: cancellationToken);
        }

        public static void CountMes(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData) => userData.MesUsed++;

        public static Task SayThankAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"Thank you so much, {targetMessage.From?.FirstName}! 😊",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else if (argument is string targetName)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"Thank you so much, {targetName}! 😊",
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               "You must either reply to a message or specify someone to thank!",
                                                               replyToMessageId: message.MessageId,
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountThankYous(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.ThankYousSent++;
            if (replyToUserData is not null)
            {
                replyToUserData.ThankYousReceived++;
            }
        }

        public static Task SayThanksAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       "You're welcome! 🦾",
                                                       replyToMessageId: message.MessageId,
                                                       cancellationToken: cancellationToken);

        public static void CountThanks(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData) => userData.ThanksSaid++;

        public static Task VaccinateAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
            => botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       "💉",
                                                       replyToMessageId: message.ReplyToMessage?.MessageId,
                                                       cancellationToken: cancellationToken);

        public static void CountVaccinations(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.VaccinationShotsAdministered++;
            if (groupData is not null)
            {
                groupData.VaccinationShotsAdministered++;
                if (replyToUserData is not null)
                {
                    replyToUserData.VaccinationShotsGot++;
                }
                else
                {
                    userData.VaccinationShotsGot++;
                }
            }
        }
    }
}
