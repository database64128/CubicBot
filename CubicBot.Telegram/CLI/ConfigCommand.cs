using System;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.CLI
{
    public static class ConfigCommand
    {
        public static async Task<int> Get(CancellationToken cancellationToken = default)
        {
            var (config, loadConfigErrMsg) = await Config.LoadConfigAsync(cancellationToken);
            if (loadConfigErrMsg is not null)
            {
                Console.WriteLine(loadConfigErrMsg);
                return 1;
            }

            Console.WriteLine($"{"Version",-36}{config.Version}");
            Console.WriteLine($"{"Bot Token",-36}{config.BotToken}");
            Console.WriteLine($"{"Enable Commands",-36}{config.EnableCommands}");
            Console.WriteLine($"{"Enable Stats",-36}{config.EnableStats}");
            Console.WriteLine();
            Console.WriteLine($"{"Enable Personal Commands",-36}{config.Commands.EnablePersonal}");
            Console.WriteLine($"{"Enable Common Commands",-36}{config.Commands.EnableCommon}");
            Console.WriteLine($"{"Enable Dice Commands",-36}{config.Commands.EnableDice}");
            Console.WriteLine($"{"Enable Consent Not Needed Commands",-36}{config.Commands.EnableConsentNotNeeded}");
            Console.WriteLine($"{"Enable Non-vegan Commands",-36}{config.Commands.EnableNonVegan}");
            Console.WriteLine($"{"Enable Law Enforcement Commands",-36}{config.Commands.EnableLawEnforcement}");
            Console.WriteLine($"{"Enable Public Services Commands",-36}{config.Commands.EnablePublicServices}");
            Console.WriteLine($"{"Enable Chinese Commands",-36}{config.Commands.EnableChinese}");
            Console.WriteLine($"{"Enable Chinese Tasks Commands",-36}{config.Commands.EnableChineseTasks}");
            Console.WriteLine($"{"Enable Systemd Commands",-36}{config.Commands.EnableSystemd}");
            Console.WriteLine();
            Console.WriteLine($"{"Enable Grass Stats",-36}{config.Stats.EnableGrass}");
            Console.WriteLine($"{"Enable Command Stats",-36}{config.Stats.EnableCommandStats}");
            Console.WriteLine($"{"Enable Message Counter",-36}{config.Stats.EnableMessageCounter}");

            return 0;
        }

        public static async Task<int> Set(ConfigChangeSet configChangeSet, CancellationToken cancellationToken = default)
        {
            var (config, loadConfigErrMsg) = await Config.LoadConfigAsync(cancellationToken);
            if (loadConfigErrMsg is not null)
            {
                Console.WriteLine(loadConfigErrMsg);
                return 1;
            }

            if (!string.IsNullOrEmpty(configChangeSet.BotToken))
                config.BotToken = configChangeSet.BotToken;
            if (configChangeSet.EnableCommandsMod is bool eCM)
                config.EnableCommands = eCM;
            if (configChangeSet.EnableStatsMod is bool eSM)
                config.EnableStats = eSM;

            if (configChangeSet.EnablePersonalCommands is bool ePC)
                config.Commands.EnablePersonal = ePC;
            if (configChangeSet.EnableCommonCommands is bool eCC)
                config.Commands.EnableCommon = eCC;
            if (configChangeSet.EnableDiceCommands is bool eDC)
                config.Commands.EnableDice = eDC;
            if (configChangeSet.EnableConsentNotNeededCommands is bool eCNNC)
                config.Commands.EnableConsentNotNeeded = eCNNC;
            if (configChangeSet.EnableNonVeganCommands is bool eNVC)
                config.Commands.EnableNonVegan = eNVC;
            if (configChangeSet.EnableLawEnforcementCommands is bool eLEC)
                config.Commands.EnableLawEnforcement = eLEC;
            if (configChangeSet.EnablePublicServicesCommands is bool ePSC)
                config.Commands.EnablePublicServices = ePSC;
            if (configChangeSet.EnableChineseCommands is bool eCNC)
                config.Commands.EnableChinese = eCNC;
            if (configChangeSet.EnableChineseTasksCommands is bool eCNTC)
                config.Commands.EnableChineseTasks = eCNTC;
            if (configChangeSet.EnableSystemdCommands is bool eSC)
                config.Commands.EnableSystemd = eSC;

            if (configChangeSet.EnableGrassStats is bool eGS)
                config.Stats.EnableGrass = eGS;
            if (configChangeSet.EnableCommandStats is bool eCS)
                config.Stats.EnableCommandStats = eCS;
            if (configChangeSet.EnableMessageCounter is bool eMC)
                config.Stats.EnableMessageCounter = eMC;

            var saveConfigErrMsg = await Config.SaveConfigAsync(config, cancellationToken);
            if (saveConfigErrMsg is not null)
            {
                Console.WriteLine(saveConfigErrMsg);
                return 1;
            }

            return 0;
        }
    }
}
