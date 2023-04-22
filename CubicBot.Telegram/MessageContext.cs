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
    private readonly Data _data;
    private MessageContext? _replyToMessageContext;

    /// <summary>
    /// Gets the bot client.
    /// </summary>
    public ITelegramBotClient BotClient { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public Message Message { get; }

    /// <summary>
    /// Gets the message's text.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the user's ID.
    /// </summary>
    public long UserId { get; }

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
    /// Gets the <see cref="MessageContext"/> of the message replied by the current message.
    /// Null if the message was not a reply.
    /// </summary>
    public MessageContext? ReplyToMessageContext
    {
        get
        {
            if (_replyToMessageContext is null && Message.ReplyToMessage is not null)
            {
                _replyToMessageContext = new(BotClient, Message.ReplyToMessage, _data);
            }
            return _replyToMessageContext;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageContext"/> class.
    /// </summary>
    public MessageContext(ITelegramBotClient botClient, Message message, Data data)
    {
        _data = data;
        BotClient = botClient;
        Message = message;
        Text = ChatHelper.GetMessageText(message);
        UserId = ChatHelper.GetMessageSenderId(message);
        UserData = data.GetOrCreateUserData(UserId);

        if (message.Chat.Type is not ChatType.Private)
        {
            GroupData = data.GetOrCreateGroupData(message.Chat.Id);
            MemberData = GroupData.GetOrCreateUserData(UserId);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageContext"/> class
    /// from an existing <see cref="MessageContext"/> object.
    /// </summary>
    public MessageContext(MessageContext messageContext)
    {
        _data = messageContext._data;
        _replyToMessageContext = messageContext._replyToMessageContext;
        BotClient = messageContext.BotClient;
        Message = messageContext.Message;
        Text = messageContext.Text;
        UserId = messageContext.UserId;
        UserData = messageContext.UserData;
        GroupData = messageContext.GroupData;
        MemberData = messageContext.MemberData;
    }
}
