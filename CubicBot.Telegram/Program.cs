using CubicBot.Telegram.CLI;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram
{
    internal class Program
    {
        private static Task Main(string[] args)
        {
            var botTokenOption = new Option<string>("--bot-token", "Telegram bot token.");
            var enableCommandsModuleOption = new Option<bool?>("--enable-commands-mod", "Whether to enable the commands module.");
            var enableStatsModuleOption = new Option<bool?>("--enable-stats-mod", "Whether to enable the stats module.");
            var enableCommonCommandsOption = new Option<bool?>("--enable-common-commands", "Whether to enable common commands.");
            var enableDiceCommandsOption = new Option<bool?>("--enable-dice-commands", "Whether to enable dice commands.");
            var enableChineseCommandsOption = new Option<bool?>("--enable-chinese-commands", "Whether to enable Chinese commands.");
            var enableGrassStatsOption = new Option<bool?>("--enable-grass-stats", "Whether to enable grass stats.");

            var configGetCommand = new Command("get", "Print config.")
            {
                Handler = CommandHandler.Create<CancellationToken>(ConfigCommand.Get),
            };

            var configSetCommand = new Command("set", "Change config.")
            {
                botTokenOption,
                enableCommandsModuleOption,
                enableStatsModuleOption,
                enableCommonCommandsOption,
                enableDiceCommandsOption,
                enableChineseCommandsOption,
                enableGrassStatsOption,
            };

            configSetCommand.Handler = CommandHandler.Create<string, bool?, bool?, bool?, bool?, bool?, bool?, CancellationToken>(ConfigCommand.Set);

            var configCommand = new Command("config", "Print or change config.")
            {
                configGetCommand,
                configSetCommand,
            };

            var rootCommand = new RootCommand("A stupid and annoying chatbot for your group chats.")
            {
                configCommand,
            };

            rootCommand.AddOption(botTokenOption);
            rootCommand.Handler = CommandHandler.Create<string, CancellationToken>(BotRunner.RunBot);

            Console.OutputEncoding = Encoding.UTF8;
            return rootCommand.InvokeAsync(args);
        }
    }
}
