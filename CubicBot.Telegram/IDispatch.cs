using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram
{
    public interface IDispatch
    {
        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default);
    }
}
