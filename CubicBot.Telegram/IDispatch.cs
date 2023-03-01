using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram;

public interface IDispatch
{
    Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken = default);
}
