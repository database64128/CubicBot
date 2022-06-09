using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public class Controls
{
    public List<CubicBotCommand> Commands { get; } = new();

    public Controls(Config config)
    {
        if (config.Stats.EnableParenthesisEnclosure)
        {
            Commands.Add(new("toggle_pea", "🔘 Toggle Parenthesis Enclosure Assurance in this chat.", ToggleParenthesisEnclosureAssuranceAsync));
        }
    }

    public static Task ToggleParenthesisEnclosureAssuranceAsync(ITelegramBotClient botClient, Message message, string? argument, Config config, Data data, CancellationToken cancellationToken = default)
    {
        var userId = ChatHelper.GetMessageSenderId(message);

        IParenthesisEnclosureControl control = message.Chat.Type switch
        {
            ChatType.Private => data.GetOrCreateUserData(userId),
            _ => data.GetOrCreateGroupData(message.Chat.Id),
        };

        control.EnsureParenthesisEnclosure ^= true;

        var responseMarkdownV2 = control.EnsureParenthesisEnclosure switch
        {
            true => @"✅ *Parenthesis Enclosure Assurance* is now _enabled_ in this chat\.",
            false => @"❌ *Parenthesis Enclosure Assurance* is now _disabled_ in this chat\.",
        };

        return botClient.SendTextMessageWithRetryAsync(message.Chat.Id,
                                                       responseMarkdownV2,
                                                       ParseMode.MarkdownV2,
                                                       replyToMessageId: message.MessageId,
                                                       cancellationToken: cancellationToken);
    }
}
