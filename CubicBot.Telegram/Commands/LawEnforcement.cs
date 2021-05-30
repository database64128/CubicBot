using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class LawEnforcement
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

        public static string[] ReasonsForArrest => new string[]
        {
            "trespassing ⛔",
            "shoplifting 🛍️",
            "stealing a vibrator 📳",
            "masturbating in public 💦",
            "making too much noise during sex 💋",
        };

        public BotCommandWithHandler[] Commands => new BotCommandWithHandler[]
        {
            new("call_cops", "📞 Hello, this is 911. What's your emergency?", CallCopsAsync),
            new("arrest", "🚓 Do I still have the right to remain silent?", ArrestAsync),
            new("guilty_or_not", "🧑‍⚖️ Yes, your honor.", GuiltyOrNotAsync),
        };

        private readonly Random _random;

        public LawEnforcement(Random random) => _random = random;

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

        public Task ArrestAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var reasonIndex = _random.Next(ReasonsForArrest.Length);
            var reason = argument ?? ReasonsForArrest[reasonIndex];

            if (message.ReplyToMessage is Message targetMessage)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{targetMessage.From.FirstName} has been arrested for {reason}.",
                                                      replyToMessageId: targetMessage.MessageId,
                                                      cancellationToken: cancellationToken);
            }
            else
            {
                return botClient.SendTextMessageAsync(message.Chat.Id,
                                                      $"{message.From.FirstName} has been arrested for {reason}.",
                                                      cancellationToken: cancellationToken);
            }
        }

        public Task GuiltyOrNotAsync(ITelegramBotClient botClient, Message message, string? argument, CancellationToken cancellationToken = default)
        {
            var verdict = _random.Next(3) switch
            {
                0 => "Not guilty.",
                1 => "Guilty on all counts.",
                _ => "The jury failed to reach a consensus.",
            };

            return botClient.SendTextMessageAsync(message.Chat.Id,
                                                  verdict,
                                                  replyToMessageId: message.ReplyToMessage?.MessageId ?? 0,
                                                  cancellationToken: cancellationToken);
        }
    }
}
