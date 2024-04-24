using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CubicBot.Telegram.Utils;

public static partial class ChatHelper
{
    /// <summary>
    /// Sends a text message with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendTextMessageAsync(ITelegramBotClient, ChatId, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task<Message> SendTextMessageWithRetryAsync(
        this MessageContext messageContext,
        string text,
        int? messageThreadId = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => messageContext.RetryAsync(
            (messageContext, cancellationToken) => messageContext.BotClient.SendTextMessageAsync(
                messageContext.Message.Chat.Id,
                text,
                messageThreadId,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken),
            cancellationToken);

    /// <summary>
    /// Replies with a text message and auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="SendTextMessageWithRetryAsync(MessageContext, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task<Message> ReplyWithTextMessageAndRetryAsync(
        this MessageContext messageContext,
        string text,
        int? messageThreadId = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => messageContext.RetryAsync(
            (messageContext, cancellationToken) => messageContext.BotClient.SendTextMessageAsync(
                messageContext.Message.Chat.Id,
                text,
                messageThreadId,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                protectContent,
                messageContext.Message.MessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken),
            cancellationToken);

    /// <summary>
    /// Sends a file with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDocumentAsync(ITelegramBotClient, ChatId, InputOnlineFile, InputMedia?, string?, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task<Message> SendDocumentWithRetryAsync(
        this MessageContext messageContext,
        InputFile document,
        int? messageThreadId = null,
        InputFile? thumbnail = default,
        string? caption = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? captionEntities = null,
        bool? disableContentTypeDetection = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => messageContext.RetryAsync(
            (messageContext, cancellationToken) => messageContext.BotClient.SendDocumentAsync(
                messageContext.Message.Chat.Id,
                document,
                messageThreadId,
                thumbnail,
                caption,
                parseMode,
                captionEntities,
                disableContentTypeDetection,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken),
            cancellationToken);

    /// <summary>
    /// Edits a text message with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.EditMessageTextAsync(ITelegramBotClient, ChatId, int, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, InlineKeyboardMarkup?, CancellationToken)"/>
    public static Task<Message> EditMessageTextWithRetryAsync(
        this MessageContext messageContext,
        int messageId,
        string text,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        InlineKeyboardMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => messageContext.RetryAsync(
            (messageContext, cancellationToken) => messageContext.BotClient.EditMessageTextAsync(
                messageContext.Message.Chat.Id,
                messageId,
                text,
                parseMode,
                entities,
                disableWebPagePreview,
                replyMarkup,
                cancellationToken),
            cancellationToken);

    /// <summary>
    /// Sends an animated emoji with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDiceAsync(ITelegramBotClient, ChatId, Emoji?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task<Message> SendDiceWithRetryAsync(
        this MessageContext messageContext,
        Emoji emoji,
        int? messageThreadId = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => messageContext.RetryAsync(
            (messageContext, cancellationToken) => messageContext.BotClient.SendDiceAsync(
                messageContext.Message.Chat.Id,
                messageThreadId,
                emoji,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken),
            cancellationToken);

    /// <summary>
    /// Sends a possibly long text message.
    /// Short messages are sent as text messages.
    /// Long messages are sent as text files.
    /// Automatically retries when hitting the rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendTextMessageAsync(ITelegramBotClient, ChatId, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task<Message> SendPossiblyLongTextMessageWithRetryAsync(
        this MessageContext messageContext,
        string text,
        int? messageThreadId = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => text.Length switch
        {
            <= 4096 => messageContext.SendTextMessageWithRetryAsync(
                text,
                messageThreadId,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken),
            _ => messageContext.SendTextFileFromStringWithRetryAsync(
                parseMode switch
                {
                    ParseMode.Markdown => "long-message.md",
                    ParseMode.Html => "long-message.html",
                    ParseMode.MarkdownV2 => "long-message.md",
                    _ => "long-message.txt",
                },
                text,
                messageThreadId,
                null,
                null,
                parseMode,
                entities,
                null,
                disableNotification,
                protectContent,
                replyToMessageId,
                null,
                replyMarkup,
                cancellationToken)
        };

    /// <summary>
    /// Sends a string as a text file.
    /// Automatically retries when hitting the rate limit.
    /// </summary>
    /// <param name="filename">Filename.</param>
    /// <param name="text">The string to send.</param>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDocumentAsync(ITelegramBotClient, ChatId, InputOnlineFile, InputMedia?, string?, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static async Task<Message> SendTextFileFromStringWithRetryAsync(
        this MessageContext messageContext,
        string filename,
        string text,
        int? messageThreadId = null,
        InputFile? thumbnail = default,
        string? caption = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? captionEntities = null,
        bool? disableContentTypeDetection = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return await messageContext.SendDocumentWithRetryAsync(
            InputFile.FromStream(stream, filename),
            messageThreadId,
            thumbnail,
            caption,
            parseMode,
            captionEntities,
            disableContentTypeDetection,
            disableNotification,
            protectContent,
            replyToMessageId,
            allowSendingWithoutReply,
            replyMarkup,
            cancellationToken);
    }

    /// <summary>
    /// Gets the proper interval to wait before retrying.
    /// Use with `catch (ApiRequestException ex) when (ex.ErrorCode == 429)`.
    /// </summary>
    /// <param name="apiRequestException">The 429 Too Many Requests exception.</param>
    /// <returns>Wait time in milliseconds.</returns>
    public static int GetRetryWaitTimeMs(ApiRequestException apiRequestException)
    {
        // "Too Many Requests: retry after 11"
        ReadOnlySpan<char> message = apiRequestException.Message;
        if (message.Length > 31 && int.TryParse(message[31..], out var timeSec))
        {
            var jitter = Random.Shared.Next(1, 6);
            return (timeSec + jitter) * 1000;
        }

        return 15 * 1000;
    }

    /// <summary>
    /// Executes an action and automatically retries when hitting the rate limit.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TResult">The return type of the action.</typeparam>
    /// <returns>The result of the action.</returns>
    public static async Task<TResult> RetryAsync<TResult>(
        this MessageContext messageContext,
        Func<MessageContext, CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                return await action(messageContext, cancellationToken);
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                await Task.Delay(GetRetryWaitTimeMs(ex), cancellationToken);
            }
        }
    }

    [GeneratedRegex("[_*[\\]()~`>#+\\-=|{}.!]")]
    private static partial Regex EscapeMarkdownV2PlaintextRegex();

    [GeneratedRegex("[`\\\\]")]
    private static partial Regex EscapeMarkdownV2CodeBlockRegex();

    /// <summary>
    /// Escapes the plaintext per the MarkdownV2 requirements.
    /// This method does not handle markdown entities.
    /// </summary>
    /// <param name="plaintext">The plaintext to be escaped.</param>
    /// <returns>An escaped string.</returns>
    public static string EscapeMarkdownV2Plaintext(string plaintext)
        => EscapeMarkdownV2PlaintextRegex().Replace(plaintext, @"\$&");

    /// <summary>
    /// Escapes the code per the MarkdownV2 requirements.
    /// </summary>
    /// <param name="code">The code to be escaped.</param>
    /// <returns>The escaped code.</returns>
    public static string EscapeMarkdownV2CodeBlock(string code)
        => EscapeMarkdownV2CodeBlockRegex().Replace(code, @"\$&");

    /// <summary>
    /// Parses a text message into a command and an argument if applicable.
    /// </summary>
    /// <param name="text">The text message to process.</param>
    /// <param name="botUsername">The bot username. Does not start with '@'.</param>
    /// <returns>
    /// A ValueTuple of the parsed command and argument.
    /// Command is null if the text message is not a command to the bot.
    /// Argument can be null.
    /// </returns>
    public static (string? command, string? argument) ParseMessageIntoCommandAndArgument(ReadOnlySpan<char> text, string botUsername)
    {
        // Empty message
        if (text.IsEmpty)
            return (null, null);

        // Not a command
        if (text[0] != '/' || text.Length < 2)
            return (null, null);

        // Remove the leading '/'
        text = text[1..];

        // Split command and argument
        ReadOnlySpan<char> command, argument;
        var spacePos = text.IndexOf(' ');
        if (spacePos == -1)
        {
            command = text;
            argument = [];
        }
        else if (spacePos == text.Length - 1)
        {
            command = text[..spacePos];
            argument = [];
        }
        else
        {
            command = text[..spacePos];
            argument = text[(spacePos + 1)..];
        }

        // Verify and remove trailing '@bot' from command
        var atSignIndex = command.IndexOf('@');
        if (atSignIndex != -1)
        {
            if (atSignIndex != command.Length - 1)
            {
                var atUsername = command[(atSignIndex + 1)..];
                if (!atUsername.SequenceEqual(botUsername))
                {
                    return (null, null);
                }
            }

            command = command[..atSignIndex];
        }

        // Trim leading and trailing spaces from argument
        argument = argument.Trim();

        // Convert back to string
        string? commandString = null;
        string? argumentString = null;
        if (!command.IsEmpty)
        {
            commandString = command.ToString();

            if (!argument.IsEmpty)
            {
                argumentString = argument.ToString();
            }
        }

        return (commandString, argumentString);
    }

    /// <summary>
    /// Gets the message's sender ID.
    /// If the message was sent to a channel, Telegram's "default" ID (777000) is returned.
    /// </summary>
    /// <param name="message">A message.</param>
    /// <returns>The message's sender ID.</returns>
    public static long GetMessageSenderId(Message message) => message.From?.Id ?? 777000L;

    /// <summary>
    /// Gets a text message's text, or a picture/video's caption, otherwise returns an empty string.
    /// </summary>
    /// <param name="message">A message.</param>
    /// <returns>A non-null string.</returns>
    public static string GetMessageText(Message message) => message.Text ?? message.Caption ?? "";
}
