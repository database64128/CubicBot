using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats;

/// <summary>
/// General-purpose stats collector.
/// Use <see cref="UserStatsCollector"/> to collect stats from private chats and group members.
/// Use <see cref="GroupStatsCollector"/> to collect stats on group chats (not group members).
/// </summary>
public abstract class StatsCollector : IStatsCollector
{
    /// <summary>
    /// Controls whether the collector should act on the message.
    /// </summary>
    /// <param name="message">Input message.</param>
    /// <returns>
    /// True if the collector should act on the message.
    /// Otherwise, false.
    /// </returns>
    public abstract bool IsCollectable(Message message);

    public abstract Task CollectAsync(ITelegramBotClient botClient, Message message, Data data, CancellationToken cancellationToken = default);
}
