using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

/// <summary>
/// Used by stats dispatch for stats collection.
/// All stats collectors must implement this interface.
/// </summary>
public interface IStatsCollector
{
    /// <summary>
    /// Collects stats from the message.
    /// </summary>
    /// <param name="messageContext">The <see cref="MessageContext"/> instance.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the collect operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CollectAsync(MessageContext messageContext, CancellationToken cancellationToken = default);
}
