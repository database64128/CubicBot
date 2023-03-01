using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram;

/// <summary>
/// Encapsulates message information.
/// </summary>
public class MessageContext
{
    /// <summary>
    /// Gets the bot client.
    /// </summary>
    public ITelegramBotClient BotClient { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public Message Message { get; }

    /// <summary>
    /// Gets the user's personal stats object.
    /// </summary>
    public UserData UserData { get; }

    /// <summary>
    /// Gets the group's stats object.
    /// Null if the message was from a private chat.
    /// </summary>
    public GroupData? GroupData { get; }

    /// <summary>
    /// Gets the group member's stats object.
    /// Null if the message was from a private chat.
    /// </summary>
    public UserData? MemberData { get; }

    /// <summary>
    /// Gets the user's stats object in the current chat.
    /// </summary>
    public UserData MemberOrUserData => MemberData ?? UserData;

    /// <summary>
    /// Gets the personal stats object of the user the message was replying to.
    /// Null if the message was not a reply.
    /// </summary>
    public UserData? ReplyToUserData { get; }

    /// <summary>
    /// Gets the stats object of the group member the message was replying to.
    /// Null if the message was from a private chat or if the message was not a reply.
    /// </summary>
    public UserData? ReplyToMemberData { get; }

    /// <summary>
    /// Gets the stats object of the user the message was replying to in the current chat.
    /// Null if the message was not a reply.
    /// </summary>
    public UserData? ReplyToMemberOrUserData => ReplyToMemberData ?? ReplyToUserData;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageContext"/> class.
    /// </summary>
    public MessageContext(ITelegramBotClient botClient, Message message, Data data)
    {
        BotClient = botClient;
        Message = message;

        var userId = ChatHelper.GetMessageSenderId(message);
        UserData = data.GetOrCreateUserData(userId);

        var replyToUserId = 0L;
        if (message.ReplyToMessage is not null)
        {
            replyToUserId = ChatHelper.GetMessageSenderId(message.ReplyToMessage);
            ReplyToUserData = data.GetOrCreateUserData(replyToUserId);
        }

        if (message.Chat.Type is not ChatType.Private)
        {
            GroupData = data.GetOrCreateGroupData(message.Chat.Id);
            MemberData = GroupData.GetOrCreateUserData(userId);
            if (message.ReplyToMessage is not null)
            {
                ReplyToMemberData = GroupData.GetOrCreateUserData(replyToUserId);
            }
        }
    }
}
