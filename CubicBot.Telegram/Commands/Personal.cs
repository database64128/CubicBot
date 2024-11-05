using CubicBot.Telegram.Stats;
using CubicBot.Telegram.Utils;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Commands;

public static class Personal
{
    public static void AddCommands(List<CubicBotCommand> commands)
    {
        commands.Add(new("add_pronouns", "➕ Add pronouns.", AddPronounsAsync));
        commands.Add(new("remove_pronouns", "➖ Remove pronouns.", RemovePronounsAsync));
        commands.Add(new("get_pronouns", "❤️ Get someone's pronouns by replying to someone's message, or display your own pronoun settings.", GetPronounsAsync));
        commands.Add(new("set_default_pronouns", "📛 Set or unset default pronouns for all chats.", SetDefaultPronounsAsync));
        commands.Add(new("set_preferred_pronouns", "🕶️ Set or unset preferred pronouns for this chat.", SetPreferredPronounsAsync));
    }

    public static Task AddPronounsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        const string helpMarkdownV2 = @"This command accepts *subject* _\(they\)_ form, *subject/object* _\(they/them\)_ form, or *subject/object/possessive\_pronoun* _\(they/them/theirs\)_ form for commonly known pronouns\. For uncommon pronouns, you must use the full *subject/object/possessive\_determiner/possessive\_pronoun/reflexive* _\(they/them/their/theirs/themselves\)_ form\.";

        if (commandContext.Argument is string argument)
        {
            if (Pronouns.TryParse(argument, out var pronouns))
            {
                var pronounsList = commandContext.UserData.PronounList;
                if (!pronounsList.Contains(pronouns))
                {
                    pronounsList.Add(pronouns);
                    responseMarkdownV2 = $@"🥳 Successfully added *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* to your list of pronouns\!";
                }
                else
                {
                    responseMarkdownV2 = $@"❌ You already have *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* in your list of pronouns\!";
                }
            }
            else
            {
                responseMarkdownV2 = $@"❌ Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. {helpMarkdownV2}";
            }
        }
        else
        {
            responseMarkdownV2 = helpMarkdownV2;
        }

        return commandContext.ReplyWithTextMessageAsync(responseMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }

    public static Task RemovePronounsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        var argument = commandContext.Argument;
        var pronounsList = commandContext.UserData.PronounList;

        if (string.IsNullOrEmpty(argument))
        {
            if (pronounsList.Count == 1)
            {
                var pronouns = pronounsList[0];
                pronounsList.Clear();
                responseMarkdownV2 = $@"🚮 Successfully removed *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* from your list of pronouns\!";
            }
            else
            {
                responseMarkdownV2 = @"❌ You have more than one set of pronouns in your list\. Specify one to remove from the list\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns))
        {
            if (pronounsList.Remove(pronouns))
            {
                responseMarkdownV2 = $@"🚮 Successfully removed *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* from your list of pronouns\!";
            }
            else
            {
                responseMarkdownV2 = $@"4️⃣0️⃣4️⃣ Not found: *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}*";
            }
        }
        else
        {
            responseMarkdownV2 = $@"❌ Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return commandContext.ReplyWithTextMessageAsync(responseMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }

    public static Task GetPronounsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        // query someone else
        if (commandContext.ReplyToMessageContext is MessageContext replyToMessageContext)
        {
            var firstname = replyToMessageContext.Message.From?.FirstName ?? string.Empty;
            var firstnameEscaped = ChatHelper.EscapeMarkdownV2Plaintext(firstname);
            var pronouns = replyToMessageContext.MemberOrUserData.PreferredPronouns?.ToString()
                ?? replyToMessageContext.UserData.DefaultPronouns?.ToString()
                ?? string.Join(", ", replyToMessageContext.UserData.PronounList.Select(x => ChatHelper.EscapeMarkdownV2Plaintext(x.ToString())));

            var responseMarkdownV2 = pronouns.Length switch
            {
                0 => $@"*{firstnameEscaped}* has not set any pronouns yet\. You may address *{firstnameEscaped}* by *{Pronouns.Neutral}*\.",
                _ => $@"You may address *{firstnameEscaped}* by *{pronouns}*\.",
            };

            return commandContext.ReplyWithTextMessageAsync(responseMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
        }

        // self query
        var responseSB = new StringBuilder();
        var userData = commandContext.UserData;
        var pronounList = userData.PronounList;

        responseSB.AppendLine($"Pronouns: {pronounList.Count}");
        foreach (var p in pronounList)
        {
            responseSB.AppendLine($"- {p}");
        }

        var defaultPronouns = userData.DefaultPronouns;
        var defaultPronounsStatus = defaultPronouns switch
        {
            null => "Not set",
            _ => defaultPronouns.ToString(),
        };

        var preferredPronouns = commandContext.MemberOrUserData.PreferredPronouns;
        var preferredPronounsStatus = preferredPronouns switch
        {
            null => "Not set",
            _ => preferredPronouns.ToString(),
        };

        responseSB.AppendLine($"Default pronouns: {defaultPronounsStatus}");
        responseSB.AppendLine($"Preferred pronouns in this chat: {preferredPronounsStatus}");

        return commandContext.ReplyWithTextMessageAsync(responseSB.ToString(), cancellationToken: cancellationToken);
    }

    public static Task SetDefaultPronounsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        string? responseMarkdownV2;
        var argument = commandContext.Argument;
        var userData = commandContext.UserData;

        if (string.IsNullOrEmpty(argument)) // unset
        {
            if (userData.DefaultPronouns is not null)
            {
                var pronouns = userData.DefaultPronouns;
                userData.DefaultPronouns = null;
                responseMarkdownV2 = $@"➖ Successfully unset your previous default pronouns *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}*\!";
            }
            else
            {
                responseMarkdownV2 = @"❌ You don't already have default pronouns\. Specify one to set as default pronouns\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns)) // set
        {
            if (userData.PronounList.Contains(pronouns))
            {
                userData.DefaultPronouns = pronouns;
                responseMarkdownV2 = $@"🥳 Successfully set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your default pronouns\!";
            }
            else
            {
                userData.DefaultPronouns = pronouns;
                userData.PronounList.Add(pronouns);
                responseMarkdownV2 = $@"🥳 Successfully added and set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your default pronouns\!";
            }
        }
        else
        {
            responseMarkdownV2 = $@"❌ Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return commandContext.ReplyWithTextMessageAsync(responseMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }

    public static Task SetPreferredPronounsAsync(CommandContext commandContext, CancellationToken cancellationToken = default)
    {
        string responseMarkdownV2;
        var argument = commandContext.Argument;
        var memberOrUserData = commandContext.MemberOrUserData;

        if (string.IsNullOrEmpty(argument)) // unset
        {
            if (memberOrUserData.PreferredPronouns is Pronouns preferredPronouns)
            {
                memberOrUserData.PreferredPronouns = null;
                responseMarkdownV2 = $@"➖ Successfully unset your previous preferred pronouns *{ChatHelper.EscapeMarkdownV2Plaintext(preferredPronouns.ToString())}* in this chat\!";
            }
            else
            {
                responseMarkdownV2 = @"❌ You don't already have preferred pronouns in this chat\. Specify one to set as preferred pronouns in this chat\.";
            }
        }
        else if (Pronouns.TryParse(argument, out var pronouns)) // set
        {
            var userData = commandContext.UserData;
            if (userData.PronounList.Contains(pronouns))
            {
                memberOrUserData.PreferredPronouns = pronouns;
                responseMarkdownV2 = $@"🥳 Successfully set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your preferred pronouns in this chat\!";
            }
            else
            {
                memberOrUserData.PreferredPronouns = pronouns;
                userData.PronounList.Add(pronouns);
                responseMarkdownV2 = $@"🥳 Successfully added and set *{ChatHelper.EscapeMarkdownV2Plaintext(pronouns.ToString())}* as your preferred pronouns in this chat\!";
            }
        }
        else
        {
            responseMarkdownV2 = $@"❌ Failed to parse *{ChatHelper.EscapeMarkdownV2Plaintext(argument)}*\. Please follow the same format requirements as `/add\_pronouns`\.";
        }

        return commandContext.ReplyWithTextMessageAsync(responseMarkdownV2, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
    }
}
