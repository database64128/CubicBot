namespace CubicBot.Telegram.Webhook;

public sealed class WebhookOptions
{
    public const string SectionName = "Webhook";

    /// <summary>
    /// Gets or sets the route pattern for receiving incoming webhook requests.
    /// </summary>
    public string RoutePattern { get; set; } = "/";

    /// <summary>
    /// Gets or sets the webhook URL.
    /// </summary>
    public string Url { get; set; } = "";

    /// <summary>
    /// Gets or sets an optional secret token for the webhook.
    /// </summary>
    public string? SecretToken { get; set; }
}
