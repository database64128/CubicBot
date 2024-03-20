using CubicBot.Telegram.Utils;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed partial class Grass : IStatsCollector
{
    [GeneratedRegex("(cao|c奥|c嗷|艹|草)")]
    private static partial Regex GrassSeedsRegex();

    private static bool IsGrowingGrass(string msg) => GrassSeedsRegex().IsMatch(msg);

    public Task CollectAsync(MessageContext messageContext, CancellationToken cancellationToken = default)
    {
        if (!IsGrowingGrass(messageContext.Text))
            return Task.CompletedTask;

        var grassGrown = messageContext.MemberOrUserData.GrassGrown;
        grassGrown++;
        messageContext.MemberOrUserData.GrassGrown = grassGrown;

        if (messageContext.GroupData is GroupData groupData)
            groupData.GrassGrown++;

        // Assign achievement on 8, 16, 32...
        if (!BitOperations.IsPow2(grassGrown) || grassGrown <= 4UL)
            return Task.CompletedTask;

        const string msgPrefix = "🏆 Achievement Unlocked: ";
        const string herb = "🌿";
        var herbCount = BitOperations.TrailingZeroCount(grassGrown);
        var msg = string.Create(msgPrefix.Length + herb.Length * herbCount, herbCount, (buf, _) =>
        {
            msgPrefix.CopyTo(buf);
            for (var i = msgPrefix.Length; i < buf.Length; i += herb.Length)
            {
                herb.CopyTo(buf[i..]);
            }
        });
        return messageContext.ReplyWithTextMessageAndRetryAsync(msg, cancellationToken: cancellationToken);
    }
}
