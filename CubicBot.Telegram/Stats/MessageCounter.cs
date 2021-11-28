using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats;

public class MessageCounter : UserStatsCollector
{
    public override bool IsCollectable(Message message) => true;

    public override void CollectUser(Message message, UserData userData, GroupData? groupData)
    {
        userData.MessagesProcessed++;
        if (groupData is not null)
        {
            groupData.MessagesProcessed++;
        }
    }
}
