using CubicBot.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<BotService>();

var app = builder.Build();

await app.RunAsync();
