using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed class MessageCounter : IStatsCollector
{
    public Task CollectAsync(MessageContext messageContext, CancellationToken _ = default)
    {
        messageContext.MemberOrUserData.MessagesProcessed++;

        if (messageContext.GroupData is GroupData groupData)
            groupData.MessagesProcessed++;

        return Task.CompletedTask;
    }
}
