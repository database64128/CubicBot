using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace CubicBot.Telegram.Webhook;

public sealed partial class WebhookBotService(
    ILogger<WebhookBotService> logger,
    IOptions<WebhookOptions> options,
    IHttpClientFactory httpClientFactory) : BotService(logger, httpClientFactory.CreateClient())
{
    private readonly WebhookOptions _options = options.Value;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        (TelegramBotClient bot, CancellationTokenSource _) = await StartBotAsync(cancellationToken);

        // Enable webhook.
        while (true)
        {
            try
            {
                await bot.SetWebhook(
                    _options.Url,
                    allowedUpdates: [UpdateType.Message],
                    secretToken: _options.SecretToken,
                    cancellationToken: cancellationToken);
                break;
            }
            catch (RequestException ex)
            {
                logger.LogWarning(ex, "Failed to enable webhook, retrying in 30 seconds");
                await Task.Delay(s_startupRetryInterval, cancellationToken);
            }
        }
    }
}
