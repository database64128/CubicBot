using CubicBot.Telegram.Utils;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed partial class TwoTripleThree : IStatsCollector
{
    [GeneratedRegex("^233+|233+$")]
    private static partial Regex LeadingAndTrailingTwoDoubleThreeRegex();

    /// <summary>
    /// Checks whether the message starts with or ends with the pattern "233+".
    /// </summary>
    /// <param name="msg">The text message to check.</param>
    /// <returns>
    /// True if the message starts with or ends with the pattern "233+".
    /// Otherwise, false.
    /// </returns>
    private static bool ContainsTwoDoubleThree(string msg) => LeadingAndTrailingTwoDoubleThreeRegex().IsMatch(msg);

    public Task CollectAsync(MessageContext messageContext, CancellationToken cancellationToken = default)
    {
        if (ContainsTwoDoubleThree(messageContext.Text))
        {
            var twoTripleThreesUsed = messageContext.MemberOrUserData.TwoTripleThreesUsed;
            twoTripleThreesUsed++;
            messageContext.MemberOrUserData.TwoTripleThreesUsed = twoTripleThreesUsed;

            if (messageContext.GroupData is GroupData groupData)
                groupData.TwoTripleThreesUsed++;

            if ((twoTripleThreesUsed & (twoTripleThreesUsed - 1UL)) == 0UL && twoTripleThreesUsed > 4UL) // 8, 16, 32...
            {
                const string msgPrefix = "🏆 Achievement Unlocked: 2";
                var threes = BitOperations.Log2(twoTripleThreesUsed);
                var msg = string.Create(msgPrefix.Length + threes, threes, (buf, _) =>
                {
                    msgPrefix.CopyTo(buf);
                    for (var i = msgPrefix.Length; i < buf.Length; i++)
                    {
                        buf[i] = '3';
                    }
                });
                return messageContext.ReplyWithTextMessageAndRetryAsync(msg, cancellationToken: cancellationToken);
            }
        }

        return Task.CompletedTask;
    }
}
