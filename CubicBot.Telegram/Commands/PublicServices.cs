using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Commands;

public static class PublicServices
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

    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("call_ambulance", "🚑 Busy saving lives?", CallAmbulance, statsCollector: CountAmbulanceCalls));
        commands.Add(new("call_fire_dept", "🚒 The flames! Beautiful, aren't they?", CallFireDeptAsync, statsCollector: CountFireCalls));
    }

    public static Task CallAmbulance(CommandContext commandContext, CancellationToken cancellationToken = default)
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

        return commandContext.SendTextMessageWithRetryAsync(sb.ToString(), cancellationToken: cancellationToken);
    }

    public static void CountAmbulanceCalls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.AmbulancesCalled++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.AmbulancesCalled++;
        }
    }

    public static Task CallFireDeptAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
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

        return commandContext.SendTextMessageWithRetryAsync(sb.ToString(), cancellationToken: cancellationToken);
    }

    public static void CountFireCalls(CommandContext commandContext)
    {
        commandContext.MemberOrUserData.FiresReported++;
        if (commandContext.GroupData is GroupData groupData)
        {
            groupData.FiresReported++;
        }
    }
}
