using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace CubicBot.Telegram.Utils;

public static class ChatHelper
{
    /// <summary>
    /// Sends a text message with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendTextMessageAsync(ITelegramBotClient, ChatId, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static async Task<Message> SendTextMessageWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        string text,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        bool? disableNotification = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                return await botClient.SendTextMessageAsync(chatId,
                                                            text,
                                                            parseMode,
                                                            entities,
                                                            disableWebPagePreview,
                                                            disableNotification,
                                                            replyToMessageId,
                                                            allowSendingWithoutReply,
                                                            replyMarkup,
                                                            cancellationToken);
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                await Task.Delay(GetRetryWaitTimeMs(ex), cancellationToken);
            }
        }
    }

    /// <summary>
    /// Sends a file with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDocumentAsync(ITelegramBotClient, ChatId, InputOnlineFile, InputMedia?, string?, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static async Task<Message> SendDocumentWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        InputOnlineFile document,
        InputMedia? thumb = null,
        string? caption = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? captionEntities = null,
        bool? disableContentTypeDetection = null,
        bool? disableNotification = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                return await botClient.SendDocumentAsync(chatId,
                                                         document,
                                                         thumb,
                                                         caption,
                                                         parseMode,
                                                         captionEntities,
                                                         disableContentTypeDetection,
                                                         disableNotification,
                                                         replyToMessageId,
                                                         allowSendingWithoutReply,
                                                         replyMarkup,
                                                         cancellationToken);
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                await Task.Delay(GetRetryWaitTimeMs(ex), cancellationToken);
            }
        }
    }

    /// <summary>
    /// Edits a text message with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.EditMessageTextAsync(ITelegramBotClient, ChatId, int, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, InlineKeyboardMarkup?, CancellationToken)"/>
    public static async Task<Message> EditMessageTextWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        int messageId,
        string text,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        InlineKeyboardMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                return await botClient.EditMessageTextAsync(chatId,
                                                            messageId,
                                                            text,
                                                            parseMode,
                                                            entities,
                                                            disableWebPagePreview,
                                                            replyMarkup,
                                                            cancellationToken);
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                await Task.Delay(GetRetryWaitTimeMs(ex), cancellationToken);
            }
        }
    }

    /// <summary>
    /// Sends an animated emoji with auto retry to work around Telegram API's rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDiceAsync(ITelegramBotClient, ChatId, Emoji?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static async Task<Message> SendDiceWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        Emoji? emoji = null,
        bool? disableNotification = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                return await botClient.SendDiceAsync(chatId,
                                                     emoji,
                                                     disableNotification,
                                                     replyToMessageId,
                                                     allowSendingWithoutReply,
                                                     replyMarkup,
                                                     cancellationToken);
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                await Task.Delay(GetRetryWaitTimeMs(ex), cancellationToken);
            }
        }
    }

    /// <summary>
    /// Sends a possibly long text message.
    /// Short messages are sent as text messages.
    /// Long messages are sent as text files.
    /// Automatically retries when hitting the rate limit.
    /// </summary>
    /// <inheritdoc cref="TelegramBotClientExtensions.SendTextMessageAsync(ITelegramBotClient, ChatId, string, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static Task SendPossiblyLongTextMessageWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        string text,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? entities = null,
        bool? disableWebPagePreview = null,
        bool? disableNotification = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
        => text.Length switch
        {
            <= 4096 => botClient.SendTextMessageWithRetryAsync(chatId,
                                                               text,
                                                               parseMode,
                                                               entities,
                                                               disableWebPagePreview,
                                                               disableNotification,
                                                               replyToMessageId,
                                                               allowSendingWithoutReply,
                                                               replyMarkup,
                                                               cancellationToken),
            _ => botClient.SendTextFileFromStringWithRetryAsync(chatId,
                                                                parseMode switch
                                                                {
                                                                    ParseMode.Markdown => "long-message.md",
                                                                    ParseMode.Html => "long-message.html",
                                                                    ParseMode.MarkdownV2 => "long-message.md",
                                                                    _ => "long-message.txt",
                                                                },
                                                                text,
                                                                null,
                                                                null,
                                                                parseMode,
                                                                entities,
                                                                null,
                                                                disableNotification,
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
    /// <inheritdoc cref="TelegramBotClientExtensions.SendDocumentAsync(ITelegramBotClient, ChatId, InputOnlineFile, InputMedia?, string?, ParseMode?, IEnumerable{MessageEntity}?, bool?, bool?, int?, bool?, IReplyMarkup?, CancellationToken)"/>
    public static async Task<Message> SendTextFileFromStringWithRetryAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        string filename,
        string text,
        InputMedia? thumb = null,
        string? caption = null,
        ParseMode? parseMode = null,
        IEnumerable<MessageEntity>? captionEntities = null,
        bool? disableContentTypeDetection = null,
        bool? disableNotification = null,
        int? replyToMessageId = null,
        bool? allowSendingWithoutReply = null,
        IReplyMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return await botClient.SendDocumentWithRetryAsync(chatId,
                                                          new(stream, filename),
                                                          thumb,
                                                          caption,
                                                          parseMode,
                                                          captionEntities,
                                                          disableContentTypeDetection,
                                                          disableNotification,
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
        var length = Encoding.UTF8.GetByteCount(apiRequestException.Message);
        if (length > 31)
        {
            var array = ArrayPool<byte>.Shared.Rent(length);
            Span<byte> timeString = array;
            try
            {
                _ = Encoding.UTF8.GetBytes(apiRequestException.Message, timeString);
                timeString = timeString[31..];
                if (Utf8Parser.TryParse(timeString, out int timeSec, out _))
                {
                    var extra = Random.Shared.Next(1, 6);
                    return (timeSec + extra) * 1000;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        return 15 * 1000;
    }

    /// <summary>
    /// Escapes the plaintext per the MarkdownV2 requirements.
    /// This method does not handle markdown entities.
    /// </summary>
    /// <param name="plaintext">The plaintext to be escaped.</param>
    /// <returns>An escaped string.</returns>
    public static string EscapeMarkdownV2Plaintext(string plaintext)
        => Regex.Replace(plaintext, @"[_*[\]()~`>#+\-=|{}.!]", @"\$&");

    /// <summary>
    /// Escapes the code per the MarkdownV2 requirements.
    /// </summary>
    /// <param name="code">The code to be escaped.</param>
    /// <returns>The escaped code.</returns>
    public static string EscapeMarkdownV2CodeBlock(string code)
        => Regex.Replace(code, @"[`\\]", @"\$&");

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
            argument = ReadOnlySpan<char>.Empty;
        }
        else if (spacePos == text.Length - 1)
        {
            command = text[..spacePos];
            argument = ReadOnlySpan<char>.Empty;
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
}
