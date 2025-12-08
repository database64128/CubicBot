using CubicBot.Telegram.App;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<LongPollingBotService>();

var host = builder.Build();
host.Run();
