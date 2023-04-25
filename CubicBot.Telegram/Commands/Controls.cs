using CubicBot.Telegram.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public sealed class Controls
{
    public ReadOnlyCollection<CubicBotCommand> Commands { get; }

    public Controls(Config config)
    {
        if (config.Stats.EnableParenthesisEnclosure)
        {
            Commands = new(new CubicBotCommand[]
            {
                new("toggle_pea", "🔘 Toggle Parenthesis Enclosure Assurance in this chat.", ToggleParenthesisEnclosureAssuranceAsync),
            });
        }
        else
        {
            Commands = Array.Empty<CubicBotCommand>().AsReadOnly();
        }
    }

    public static Task ToggleParenthesisEnclosureAssuranceAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        var ensureParenthesisEnclosure = commandContext.ChatData.EnsureParenthesisEnclosure;
        ensureParenthesisEnclosure ^= true;
        commandContext.ChatData.EnsureParenthesisEnclosure = ensureParenthesisEnclosure;

        var responseMarkdownV2 = ensureParenthesisEnclosure switch
        {
            true => @"✅ *Parenthesis Enclosure Assurance* is now _enabled_ in this chat\.",
            false => @"❌ *Parenthesis Enclosure Assurance* is now _disabled_ in this chat\.",
        };

        return commandContext.ReplyWithTextMessageAndRetryAsync(responseMarkdownV2, ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }
}
