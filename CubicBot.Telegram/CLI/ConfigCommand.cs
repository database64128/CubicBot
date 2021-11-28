using System;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.CLI
{
    public static class ConfigCommand
    {
        public static async Task<int> Get(CancellationToken cancellationToken = default)
        {
            (var config, var loadConfigErrMsg) = await Config.LoadConfigAsync(cancellationToken);
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
            Console.WriteLine();
            Console.WriteLine($"{"Enable Grass Stats",-36}{config.Stats.EnableGrass}");
            Console.WriteLine($"{"Enable Command Stats",-36}{config.Stats.EnableCommandStats}");
            Console.WriteLine($"{"Enable Message Counter",-36}{config.Stats.EnableMessageCounter}");

            return 0;
        }

        public static async Task<int> Set(
            string? botToken,
            bool? enableCommandsMod,
            bool? enableStatsMod,
            bool? enablePersonalCommands,
            bool? enableCommonCommands,
            bool? enableDiceCommands,
            bool? enableConsentNotNeededCommands,
            bool? enableNonVeganCommands,
            bool? enableLawEnforcementCommands,
            bool? enablePublicServicesCommands,
            bool? enableChineseCommands,
            bool? enableChineseTasksCommands,
            bool? enableGrassStats,
            bool? enableCommandStats,
            bool? enableMessageCounter,
            CancellationToken cancellationToken = default)
        {
            (var config, var loadConfigErrMsg) = await Config.LoadConfigAsync(cancellationToken);
            if (loadConfigErrMsg is not null)
            {
                Console.WriteLine(loadConfigErrMsg);
                return 1;
            }

            if (!string.IsNullOrEmpty(botToken))
                config.BotToken = botToken;
            if (enableCommandsMod is bool eCM)
                config.EnableCommands = eCM;
            if (enableStatsMod is bool eSM)
                config.EnableStats = eSM;

            if (enablePersonalCommands is bool ePC)
                config.Commands.EnablePersonal = ePC;
            if (enableCommonCommands is bool eCC)
                config.Commands.EnableCommon = eCC;
            if (enableDiceCommands is bool eDC)
                config.Commands.EnableDice = eDC;
            if (enableConsentNotNeededCommands is bool eCNNC)
                config.Commands.EnableConsentNotNeeded = eCNNC;
            if (enableNonVeganCommands is bool eNVC)
                config.Commands.EnableNonVegan = eNVC;
            if (enableLawEnforcementCommands is bool eLEC)
                config.Commands.EnableLawEnforcement = eLEC;
            if (enablePublicServicesCommands is bool ePSC)
                config.Commands.EnablePublicServices = ePSC;
            if (enableChineseCommands is bool eCNC)
                config.Commands.EnableChinese = eCNC;
            if (enableChineseTasksCommands is bool eCNTC)
                config.Commands.EnableChineseTasks = eCNTC;

            if (enableGrassStats is bool eGS)
                config.Stats.EnableGrass = eGS;
            if (enableCommandStats is bool eCS)
                config.Stats.EnableCommandStats = eCS;
            if (enableMessageCounter is bool eMC)
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
