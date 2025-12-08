using CubicBot.Telegram.LongPolling;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<LongPollingBotService>();

var host = builder.Build();
host.Run();
