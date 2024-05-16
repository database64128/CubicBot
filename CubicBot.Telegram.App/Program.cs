using CubicBot.Telegram;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BotService>();

var host = builder.Build();
host.Run();
