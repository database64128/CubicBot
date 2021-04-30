using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats
{
    public interface IStatsCollector
    {
        public Task CollectAsync(ITelegramBotClient botClient, Message message, Data.Data data, CancellationToken cancellationToken = default);
    }
}
