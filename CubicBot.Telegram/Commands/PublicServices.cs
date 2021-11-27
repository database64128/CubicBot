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
        public static readonly string[] MedicalWorkers = new string[]
        {
            "👩‍⚕️", "👩🏻‍⚕️", "👩🏼‍⚕️", "👩🏽‍⚕️", "👩🏾‍⚕️", "👩🏿‍⚕️",
            "🧑‍⚕️", "🧑🏻‍⚕️", "🧑🏼‍⚕️", "🧑🏽‍⚕️", "🧑🏾‍⚕️", "🧑🏿‍⚕️",
            "👨‍⚕️", "👨🏻‍⚕️", "👨🏼‍⚕️", "👨🏽‍⚕️", "👨🏾‍⚕️", "👨🏿‍⚕️",
        };

        public static readonly string[] Ambulances = new string[]
        {
            "🥼", "🩺", "😷", "🏥", "🚑", "🚨",
        };

        public static readonly string[] Firefighters = new string[]
        {
            "👩‍🚒", "👩🏻‍🚒", "👩🏼‍🚒", "👩🏽‍🚒", "👩🏾‍🚒", "👩🏿‍🚒",
            "🧑‍🚒", "🧑🏻‍🚒", "🧑🏼‍🚒", "🧑🏽‍🚒", "🧑🏾‍🚒", "🧑🏿‍🚒",
            "👨‍🚒", "👨🏻‍🚒", "👨🏼‍🚒", "👨🏽‍🚒", "👨🏾‍🚒", "👨🏿‍🚒",
        };

        public static readonly string[] FireTrucks = new string[]
        {
            "🚒", "🧯", "🔥", "❤️‍🔥", "💥", "🚨", "⛑",
        };

        public static readonly CubicBotCommand[] Commands = new CubicBotCommand[]
        {
            new("call_ambulance", "🚑 Busy saving lives?", CallAmbulance),
            new("call_fire_dept", "🚒 The flames! Beautiful, aren't they?", CallFireDeptAsync),
        };

        public static Task CallAmbulance(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = Random.Shared.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = Random.Shared.Next(4);
                switch (type)
                {
                    case 0:
                        var medicalWorkerIndex = Random.Shared.Next(MedicalWorkers.Length);
                        sb.Append(MedicalWorkers[medicalWorkerIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = Random.Shared.Next(Ambulances.Length);
                        sb.Append(Ambulances[presenceIndex]);
                        break;
                }
            }

            return botClient.SendTextMessageAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
        }

        public static Task CallFireDeptAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"📱9️⃣1️⃣1️⃣📲📞👌{Environment.NewLine}");
            var count = Random.Shared.Next(24, 97);

            for (var i = 0; i < count; i++)
            {
                var type = Random.Shared.Next(4);
                switch (type)
                {
                    case 0:
                        var firefighterIndex = Random.Shared.Next(Firefighters.Length);
                        sb.Append(Firefighters[firefighterIndex]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        var presenceIndex = Random.Shared.Next(FireTrucks.Length);
                        sb.Append(FireTrucks[presenceIndex]);
                        break;
                }
            }

            return botClient.SendTextMessageAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
        }
    }
}
