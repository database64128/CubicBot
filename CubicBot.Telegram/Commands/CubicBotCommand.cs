using CubicBot.Telegram.Stats;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Commands;

public class CubicBotCommand : BotCommand
{
    public Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task> HandlerAsync { get; set; }

    /// <summary>
    /// Gets or sets the delegate that collects stats on individual users (outside of groups).
    /// </summary>
    public Action<Message, string?, UserData>? UserStatsCollector { get; set; }

    /// <summary>
    /// Gets or sets the delegate that collects stats on groups and group members.
    /// </summary>
    public Action<Message, string?, GroupData, UserData>? GroupStatsCollector { get; set; }

    /// <summary>
    /// Gets or sets the delegate that collects stats on users in and outside of groups.
    /// The first <see cref="UserData"/> is the user who sent the message.
    /// The first <see cref="GroupData"/> is the group where the message was sent. It's always null in private chats.
    /// The second <see cref="UserData"/> is the user the message was replying to in a group. It's always null in private chats.
    /// </summary>
    public Action<Message, string?, UserData, GroupData?, UserData?>? UserOrMemberStatsCollector { get; set; }

    public Func<ITelegramBotClient, Message, string?, UserData, GroupData?, UserData?, CancellationToken, Task>? UserOrMemberRespondAsync { get; set; }

    public CubicBotCommand(
        string command,
        string description,
        Func<ITelegramBotClient, Message, string?, Config, Data, CancellationToken, Task> handlerAsync,
        Action<Message, string?, UserData>? userStatsCollector = null,
        Action<Message, string?, GroupData, UserData>? groupStatsCollector = null,
        Action<Message, string?, UserData, GroupData?, UserData?>? userOrMemberStatsCollector = null,
        Func<ITelegramBotClient, Message, string?, UserData, GroupData?, UserData?, CancellationToken, Task>? userOrMemberRespondAsync = null)
    {
        Command = command;
        Description = description;
        HandlerAsync = handlerAsync;
        UserStatsCollector = userStatsCollector;
        GroupStatsCollector = groupStatsCollector;
        UserOrMemberStatsCollector = userOrMemberStatsCollector;
        UserOrMemberRespondAsync = userOrMemberRespondAsync;
    }
}
