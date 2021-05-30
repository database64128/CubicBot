using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands
{
    public class PublicServices
    {
        public static string[] MedicalWorkers => new string[]
        {
            "👩‍⚕️", "👩🏻‍⚕️", "👩🏼‍⚕️", "👩🏽‍⚕️", "👩🏾‍⚕️", "👩🏿‍⚕️",
            "🧑‍⚕️", "🧑🏻‍⚕️", "🧑🏼‍⚕️", "🧑🏽‍⚕️", "🧑🏾‍⚕️", "🧑🏿‍⚕️",
            "👨‍⚕️", "👨🏻‍⚕️", "👨🏼‍⚕️", "👨🏽‍⚕️", "👨🏾‍⚕️", "👨🏿‍⚕️",
        };

        public static string[] Ambulances => new string[]
        {
            "🥼", "🩺", "😷", "🏥", "🚑", "🚨",
        };

        public static string[] Firefighters => new string[]
        {
            "👩‍🚒", "👩🏻‍🚒", "👩🏼‍🚒", "👩🏽‍🚒", "👩🏾‍🚒", "👩🏿‍🚒",
            "🧑‍🚒", "🧑🏻‍🚒", "🧑🏼‍🚒", "🧑🏽‍🚒", "🧑🏾‍🚒", "🧑🏿‍🚒",
            "👨‍🚒", "👨🏻‍🚒", "👨🏼‍🚒", "👨🏽‍🚒", "👨🏾‍🚒", "👨🏿‍🚒",
        };

        public static string[] FireTrucks => new string[]
        {
            "🚒", "🧯", "🔥", "❤️‍🔥", "💥", "🚨", "⛑",
        };

        public CubicBotCommand[] Commands => new CubicBotCommand[]
        {
            new("call_ambulance", "🚑 Busy saving lives?", CallAmbulance),
            new("call_fire_dept", "🚒 The flames! Beautiful, aren't they?", CallFireDeptAsync),
        };

        private readonly Random _random;

        public PublicServices(Random random) => _random = random;

        public Task CallAmbulance(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = _random.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = _random.Next(4);
                switch (type)
                {
                    case 0:
                        var medicalWorkerIndex = _random.Next(MedicalWorkers.Length);
                        sb.Append(MedicalWorkers[medicalWorkerIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = _random.Next(Ambulances.Length);
                        sb.Append(Ambulances[presenceIndex]);
                        break;
                }
            }

            return botClient.SendTextMessageAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
        }

        public Task CallFireDeptAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
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
    }
}
