using CubicBot.Telegram.CLI;
using System;
using System.CommandLine;
using System.Text;
using System.Threading.Tasks;

namespace CubicBot.Telegram;

internal class Program
{
    private static Task<int> Main(string[] args)
    {
        var botTokenOption = new CliOption<string?>("--bot-token")
        {
            Description = "Telegram bot token.",
        };
        var enableCommandsModuleOption = new CliOption<bool?>("--enable-commands-mod")
        {
            Description = "Whether to enable the commands module.",
        };
        var enableStatsModuleOption = new CliOption<bool?>("--enable-stats-mod")
        {
            Description = "Whether to enable the stats module.",
        };

        var enablePersonalCommandsOption = new CliOption<bool?>("--enable-personal-commands")
        {
            Description = "Whether to enable personal commands.",
        };
        var enableCommonCommandsOption = new CliOption<bool?>("--enable-common-commands")
        {
            Description = "Whether to enable common commands.",
        };
        var enableDiceCommandsOption = new CliOption<bool?>("--enable-dice-commands")
        {
            Description = "Whether to enable dice commands.",
        };
        var enableConsentNotNeededCommandsOption = new CliOption<bool?>("--enable-consent-not-needed-commands")
        {
            Description = "Whether to enable consent not needed commands.",
        };
        var enableNonVeganCommandsOption = new CliOption<bool?>("--enable-non-vegan-commands")
        {
            Description = "Whether to enable non-vegan commands.",
        };
        var enableLawEnforcementCommandsOption = new CliOption<bool?>("--enable-law-enforcement-commands")
        {
            Description = "Whether to enable law enforcement commands.",
        };
        var enablePublicServicesCommandsOption = new CliOption<bool?>("--enable-public-services-commands")
        {
            Description = "Whether to enable public services commands.",
        };
        var enableChineseCommandsOption = new CliOption<bool?>("--enable-chinese-commands")
        {
            Description = "Whether to enable Chinese commands.",
        };
        var enableChineseTasksCommandsOption = new CliOption<bool?>("--enable-chinese-tasks-commands")
        {
            Description = "Whether to enable Chinese tasks commands.",
        };
        var enableSystemdCommandsOption = new CliOption<bool?>("--enable-systemd-commands")
        {
            Description = "Whether to enable systemd commands.",
        };

        var enableGrassStatsOption = new CliOption<bool?>("--enable-grass-stats")
        {
            Description = "Whether to enable grass stats.",
        };
        var enableCommandStatsOption = new CliOption<bool?>("--enable-command-stats")
        {
            Description = "Whether to enable command stats.",
        };
        var enableMessageCounterOption = new CliOption<bool?>("--enable-message-counter")
        {
            Description = "Whether to enable message counter.",
        };
        var enableTwoTripleThreeOption = new CliOption<bool?>("--enable-two-triple-three")
        {
            Description = "Whether to enable two triple three (2333) counter.",
        };
        var enableParenthesisEnclosureOption = new CliOption<bool?>("--enable-parenthesis-enclosure")
        {
            Description = "Whether to enable parenthesis enclosure.",
        };

        var configGetCommand = new CliCommand("get", "Print config.");

        var configSetCommand = new CliCommand("set", "Change config.")
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
            enableParenthesisEnclosureOption,
        };

        configGetCommand.SetAction((_, cancellationToken) => ConfigCommand.GetAsync(cancellationToken));
        configSetCommand.SetAction((parseResult, cancellationToken) =>
        {
            var botToken = parseResult.GetValue(botTokenOption);
            var enableCommandsModule = parseResult.GetValue(enableCommandsModuleOption);
            var enableStatsModule = parseResult.GetValue(enableStatsModuleOption);
            var enablePersonalCommands = parseResult.GetValue(enablePersonalCommandsOption);
            var enableCommonCommands = parseResult.GetValue(enableCommonCommandsOption);
            var enableDiceCommands = parseResult.GetValue(enableDiceCommandsOption);
            var enableConsentNotNeededCommands = parseResult.GetValue(enableConsentNotNeededCommandsOption);
            var enableNonVeganCommands = parseResult.GetValue(enableNonVeganCommandsOption);
            var enableLawEnforcementCommands = parseResult.GetValue(enableLawEnforcementCommandsOption);
            var enablePublicServicesCommands = parseResult.GetValue(enablePublicServicesCommandsOption);
            var enableChineseCommands = parseResult.GetValue(enableChineseCommandsOption);
            var enableChineseTasksCommands = parseResult.GetValue(enableChineseTasksCommandsOption);
            var enableSystemdCommands = parseResult.GetValue(enableSystemdCommandsOption);
            var enableGrassStats = parseResult.GetValue(enableGrassStatsOption);
            var enableCommandStats = parseResult.GetValue(enableCommandStatsOption);
            var enableMessageCounter = parseResult.GetValue(enableMessageCounterOption);
            var enableTwoTripleThree = parseResult.GetValue(enableTwoTripleThreeOption);
            var enableParenthesisEnclosure = parseResult.GetValue(enableParenthesisEnclosureOption);
            return ConfigCommand.SetAsync(
                botToken,
                enableCommandsModule,
                enableStatsModule,
                enablePersonalCommands,
                enableCommonCommands,
                enableDiceCommands,
                enableConsentNotNeededCommands,
                enableNonVeganCommands,
                enableLawEnforcementCommands,
                enablePublicServicesCommands,
                enableChineseCommands,
                enableChineseTasksCommands,
                enableSystemdCommands,
                enableGrassStats,
                enableCommandStats,
                enableMessageCounter,
                enableTwoTripleThree,
                enableParenthesisEnclosure,
                cancellationToken);
        });

        var configCommand = new CliCommand("config", "Print or change config.")
        {
            configGetCommand,
            configSetCommand,
        };

        var rootCommand = new CliRootCommand("A stupid and annoying chatbot for your group chats.")
        {
            configCommand,
        };

        rootCommand.Options.Add(botTokenOption);
        rootCommand.SetAction((parseResult, cancellationToken) =>
        {
            var botToken = parseResult.GetValue(botTokenOption);
            return BotRunner.RunBotAsync(botToken, cancellationToken);
        });

        Console.OutputEncoding = Encoding.UTF8;
        return rootCommand.Parse(args).InvokeAsync();
    }
}
