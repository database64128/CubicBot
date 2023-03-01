using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public class PublicServices
{
    private static readonly string[] s_medicalWorkers =
    {
        "👩‍⚕️", "👩🏻‍⚕️", "👩🏼‍⚕️", "👩🏽‍⚕️", "👩🏾‍⚕️", "👩🏿‍⚕️",
        "🧑‍⚕️", "🧑🏻‍⚕️", "🧑🏼‍⚕️", "🧑🏽‍⚕️", "🧑🏾‍⚕️", "🧑🏿‍⚕️",
        "👨‍⚕️", "👨🏻‍⚕️", "👨🏼‍⚕️", "👨🏽‍⚕️", "👨🏾‍⚕️", "👨🏿‍⚕️",
    };

    private static readonly string[] s_ambulances =
    {
        "🥼", "🩺", "😷", "🏥", "🚑", "🚨",
    };

    private static readonly string[] s_firefighters =
    {
        "👩‍🚒", "👩🏻‍🚒", "👩🏼‍🚒", "👩🏽‍🚒", "👩🏾‍🚒", "👩🏿‍🚒",
        "🧑‍🚒", "🧑🏻‍🚒", "🧑🏼‍🚒", "🧑🏽‍🚒", "🧑🏾‍🚒", "🧑🏿‍🚒",
        "👨‍🚒", "👨🏻‍🚒", "👨🏼‍🚒", "👨🏽‍🚒", "👨🏾‍🚒", "👨🏿‍🚒",
    };

    private static readonly string[] s_fireTrucks =
    {
        "🚒", "🧯", "🔥", "❤️‍🔥", "💥", "🚨", "⛑",
    };

    public static readonly ReadOnlyCollection<CubicBotCommand> Commands = new(new CubicBotCommand[]
    {
        new("call_ambulance", "🚑 Busy saving lives?", CallAmbulance, userOrMemberStatsCollector: CountAmbulanceCalls),
        new("call_fire_dept", "🚒 The flames! Beautiful, aren't they?", CallFireDeptAsync, userOrMemberStatsCollector: CountFireCalls),
    });

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
                    var medicalWorkerIndex = Random.Shared.Next(s_medicalWorkers.Length);
                    sb.Append(s_medicalWorkers[medicalWorkerIndex]);
                    break;
                case 1:
                case 2:
                case 3:
                    var presenceIndex = Random.Shared.Next(s_ambulances.Length);
                    sb.Append(s_ambulances[presenceIndex]);
                    break;
            }
        }

        return botClient.SendTextMessageWithRetryAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
    }

    public static void CountAmbulanceCalls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.AmbulancesCalled++;
        if (groupData is not null)
        {
            groupData.AmbulancesCalled++;
        }
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
                    var firefighterIndex = Random.Shared.Next(s_firefighters.Length);
                    sb.Append(s_firefighters[firefighterIndex]);
                    break;
                case 1:
                case 2:
                case 3:
                    var presenceIndex = Random.Shared.Next(s_fireTrucks.Length);
                    sb.Append(s_fireTrucks[presenceIndex]);
                    break;
            }
        }

        return botClient.SendTextMessageWithRetryAsync(message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);
    }

    public static void CountFireCalls(Message message, string? argument, UserData userData, GroupData? groupData, UserData? replyToUserData)
    {
        userData.FiresReported++;
        if (groupData is not null)
        {
            groupData.FiresReported++;
        }
    }
}
