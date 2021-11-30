using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public static class LawEnforcement
    {
        public static readonly string[] PoliceOfficers = new string[]
        {
            "👮‍♀️", "👮🏻‍♀️", "👮🏼‍♀️", "👮🏽‍♀️", "👮🏾‍♀️", "👮🏿‍♀️",
            "👮", "👮🏻", "👮🏼", "👮🏽", "👮🏾", "👮🏿",
            "👮‍♂️", "👮🏻‍♂️", "👮🏼‍♂️", "👮🏽‍♂️", "👮🏾‍♂️", "👮🏿‍♂️",
        };

        public static readonly string[] PolicePresence = new string[]
        {
            "🚓", "🚔", "🚨",
        };

        public static readonly string[] ReasonsForArrest = new string[]
        {
            "trespassing ⛔",
            "shoplifting 🛍️",
            "stealing a vibrator 📳",
            "masturbating in public 💦",
            "making too much noise during sex 💋",
        };

        public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
        {
            new("call_cops", "📞 Hello, this is 911. What's your emergency?", CallCopsAsync, userOrMemberStatsCollector: CountCopCalls),
            new("arrest", "🚓 Do I still have the right to remain silent?", ArrestAsync, userOrMemberStatsCollector: CountArrests),
            new("guilty_or_not", "🧑‍⚖️ Yes, your honor.", GuiltyOrNotAsync, userOrMemberStatsCollector: CountLawsuits),
        };

        public static Task CallCopsAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = Random.Shared.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = Random.Shared.Next(4);
                switch (type)
                {
                    case 0:
                        var officerIndex = Random.Shared.Next(PoliceOfficers.Length);
                        sb.Append(PoliceOfficers[officerIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = Random.Shared.Next(PolicePresence.Length);
                        sb.Append(PolicePresence[presenceIndex]);
                        break;
                }
            }

            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
        }

        public static void CountCopCalls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.CopCallsMade++;
            if (groupData is not null)
            {
                groupData.CopCallsMade++;
            }
        }

        public static Task ArrestAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var reasonIndex = Random.Shared.Next(ReasonsForArrest.Length);
            var reason = argument ?? ReasonsForArrest[reasonIndex];

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{targetMessage.From?.FirstName} has been arrested for {reason}.",
                                                               replyToMessageId: targetMessage.MessageId,
                                                               cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                               $"{message.From?.FirstName} has been arrested for {reason}.",
                                                               cancellationToken: cancellationToken);
            }
        }

        public static void CountArrests(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.ArrestsMade++;
            if (groupData is not null)
            {
                groupData.ArrestsMade++;
                if (replyToUserData is not null)
                {
                    replyToUserData.ArrestsReceived++;
                }
                else
                {
                    userData.ArrestsReceived++;
                }
            }
        }

        public static Task GuiltyOrNotAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var verdict = Random.Shared.Next(3) switch
            {
                0 => "Not guilty.",
                1 => "Guilty on all counts.",
                _ => "The jury failed to reach a consensus.",
            };

            return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                           verdict,
                                                           replyToMessageId: message.ReplyToMessage?.MessageId,
                                                           cancellationToken: cancellationToken);
        }

        public static void CountLawsuits(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
        {
            userData.VerdictsGiven++;
            if (groupData is not null)
            {
                groupData.VerdictsGiven++;
                if (replyToUserData is not null)
                {
                    replyToUserData.VerdictsReceived++;
                }
            }
        }
    }
}
