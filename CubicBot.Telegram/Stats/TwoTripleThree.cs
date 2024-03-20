using CubicBot.Telegram.Utils;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed class TwoTripleThree : IStatsCollector
{
    private const string TwoDoubleThree = "233";

    /// <summary>
    /// Checks whether the message starts with or ends with "233".
    /// </summary>
    /// <param name="msg">The text message to check.</param>
    /// <returns>
    /// True if the message starts with or ends with "233".
    /// Otherwise, false.
    /// </returns>
    private static bool ContainsTwoDoubleThree(ReadOnlySpan<char> msg) => msg.Length >= TwoDoubleThree.Length &&
        (msg[..TwoDoubleThree.Length] == TwoDoubleThree || msg[(msg.Length - TwoDoubleThree.Length)..] == TwoDoubleThree);

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
