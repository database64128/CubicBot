using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats;

/// <summary>
/// Stats collector for group chats (not group members).
/// </summary>
public abstract class GroupStatsCollector : StatsCollector
{
    /// <summary>
    /// Collects stats for the group.
    /// </summary>
    /// <param name="message">Input message.</param>
    /// <param name="groupData">Group data object.</param>
    public abstract void CollectGroup(Message message, GroupData groupData);

    /// <summary>
    /// Optionally, responds to the message.
    /// Override this method to generate responses.
    /// </summary>
    /// <param name="botClient">The Telegram bot client.</param>
    /// <param name="message">Input message.</param>
    /// <param name="groupData">Group data object.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the respond operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public virtual Task Respond(ITelegramBotClient botClient, Message message, GroupData groupData, CancellationToken cancellationToken) => Task.CompletedTask;

    public override Task CollectAsync(ITelegramBotClient botClient, Message message, Data data, CancellationToken cancellationToken = default)
    {
        if (!IsCollectable(message))
        {
            return Task.CompletedTask;
        }

        var groupData = data.GetOrCreateGroupData(message.Chat.Id);
        CollectGroup(message, groupData);

        return Respond(botClient, message, groupData, cancellationToken);
    }
}
