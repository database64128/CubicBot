using CubicBot.Telegram.Utils;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed class Grass : IStatsCollector
{
    private static readonly string[] s_grassSeeds =
    {
        "cao", "艹", "草", "c奥", "c嗷",
    };

    private static bool IsGrowingGrass(string msg) => msg.Length > 0 && s_grassSeeds.Any(seed => msg.Contains(seed));

    public Task CollectAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        if (IsGrowingGrass(messageContext.Text))
        {
            var grassGrown = messageContext.MemberOrUserData.GrassGrown;
            grassGrown++;
            messageContext.MemberOrUserData.GrassGrown = grassGrown;

            if (messageContext.GroupData is GroupData groupData)
                groupData.GrassGrown++;

            if ((grassGrown & (grassGrown - 1UL)) == 0UL && grassGrown > 4UL) // 8, 16, 32...
                return messageContext.ReplyWithTextMessageAndRetryAsync($"🏆 Achievement Unlocked: {grassGrown} Grass Grown", cancellationToken: cancellationToken); ;
        }

        return Task.CompletedTask;
    }
}
