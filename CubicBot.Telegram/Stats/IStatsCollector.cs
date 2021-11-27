using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats
{
    /// <summary>
    /// Used by stats dispatch for stats collection.
    /// All stats collectors must implement this interface.
    /// </summary>
    public interface IStatsCollector
    {
        /// <summary>
        /// Collects stats from the message.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="message">Input message.</param>
        /// <param name="data">The data object.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the respond operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CollectAsync(ITelegramBotClient botClient, Message message, Data data, CancellationToken cancellationToken = default);
    }
}
