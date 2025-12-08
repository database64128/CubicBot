using CubicBot.Telegram.Webhook;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WebhookOptions>(builder.Configuration.GetSection(WebhookOptions.SectionName));
builder.Services.ConfigureTelegramBot<JsonOptions>(opt => opt.SerializerOptions);
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WebhookBotService>();
builder.Services.AddHostedService(p => p.GetRequiredService<WebhookBotService>());

var app = builder.Build();

var options = app.Services.GetRequiredService<IOptions<WebhookOptions>>().Value;

app.MapPost(options.RoutePattern, HandleUpdateAsync);

app.Run();

static async Task<IResult> HandleUpdateAsync(Update update, WebhookBotService botService, CancellationToken cancellationToken = default)
{
    await botService.UpdateWriter.WriteAsync(update, cancellationToken);
    return TypedResults.Ok();
}
