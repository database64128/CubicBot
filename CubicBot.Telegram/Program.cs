using CubicBot.Telegram.CLI;
using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram
{
    internal class Program
    {
        private static Task Main(string[] args)
        {
            var botTokenOption = new Option<string?>("--bot-token", "Telegram bot token.");
            var enableCommandsModuleOption = new Option<bool?>("--enable-commands-mod", "Whether to enable the commands module.");
            var enableStatsModuleOption = new Option<bool?>("--enable-stats-mod", "Whether to enable the stats module.");

            var enablePersonalCommandsOption = new Option<bool?>("--enable-personal-commands", "Whether to enable personal commands.");
            var enableCommonCommandsOption = new Option<bool?>("--enable-common-commands", "Whether to enable common commands.");
            var enableDiceCommandsOption = new Option<bool?>("--enable-dice-commands", "Whether to enable dice commands.");
            var enableConsentNotNeededCommandsOption = new Option<bool?>("--enable-consent-not-needed-commands", "Whether to enable consent not needed commands.");
            var enableNonVeganCommandsOption = new Option<bool?>("--enable-non-vegan-commands", "Whether to enable non-vegan commands.");
            var enableLawEnforcementCommandsOption = new Option<bool?>("--enable-law-enforcement-commands", "Whether to enable law enforcement commands.");
            var enablePublicServicesCommandsOption = new Option<bool?>("--enable-public-services-commands", "Whether to enable public services commands.");
            var enableChineseCommandsOption = new Option<bool?>("--enable-chinese-commands", "Whether to enable Chinese commands.");
            var enableChineseTasksCommandsOption = new Option<bool?>("--enable-chinese-tasks-commands", "Whether to enable Chinese tasks commands.");
            var enableSystemdCommandsOption = new Option<bool?>("--enable-systemd-commands", "Whether to enable systemd commands.");

            var enableGrassStatsOption = new Option<bool?>("--enable-grass-stats", "Whether to enable grass stats.");
            var enableCommandStatsOption = new Option<bool?>("--enable-command-stats", "Whether to enable command stats.");
            var enableMessageCounterOption = new Option<bool?>("--enable-message-counter", "Whether to enable message counter.");

            var configGetCommand = new Command("get", "Print config.");

            var configSetCommand = new Command("set", "Change config.")
            {
                botTokenOption,
                enableCommandsModuleOption,
                enableStatsModuleOption,
                enablePersonalCommandsOption,
                enableCommonCommandsOption,
                enableDiceCommandsOption,
                enableConsentNotNeededCommandsOption,
                enableNonVeganCommandsOption,
                enableLawEnforcementCommandsOption,
                enablePublicServicesCommandsOption,
                enableChineseCommandsOption,
                enableChineseTasksCommandsOption,
                enableSystemdCommandsOption,
                enableGrassStatsOption,
                enableCommandStatsOption,
                enableMessageCounterOption,
            };

            configGetCommand.SetHandler<CancellationToken>(ConfigCommand.Get);
            configSetCommand.Handler = CommandHandler.Create(ConfigCommand.Set);

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
            rootCommand.SetHandler<string?, CancellationToken>(BotRunner.RunBot, botTokenOption);

            Console.OutputEncoding = Encoding.UTF8;
            return rootCommand.InvokeAsync(args);
        }
    }
}
