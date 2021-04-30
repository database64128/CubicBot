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

            Console.WriteLine($"{"Version",-28}{config.Version}");
            Console.WriteLine($"{"Bot Token",-28}{config.BotToken}");
            Console.WriteLine($"{"Enable Commands",-28}{config.EnableCommands}");
            Console.WriteLine($"{"Enable Stats",-28}{config.EnableStats}");
            Console.WriteLine($"{"Enable Common Commands",-28}{config.Commands.EnableCommon}");
            Console.WriteLine($"{"Enable Dice Commands",-28}{config.Commands.EnableDice}");
            Console.WriteLine($"{"Enable Chinese Commands",-28}{config.Commands.EnableChinese}");
            Console.WriteLine($"{"Enable Grass Stats",-28}{config.Stats.EnableGrass}");

            return 0;
        }

        public static async Task<int> Set(string botToken, bool? enableCommandsMod, bool? enableStatsMod, bool? enableCommonCommands, bool? enableDiceCommands, bool? enableChineseCommands, bool? enableGrassStats, CancellationToken cancellationToken = default)
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
            if (enableCommonCommands is bool eCC)
                config.Commands.EnableCommon = eCC;
            if (enableDiceCommands is bool eDC)
                config.Commands.EnableDice = eDC;
            if (enableChineseCommands is bool eCNC)
                config.Commands.EnableChinese = eCNC;
            if (enableGrassStats is bool eGS)
                config.Stats.EnableGrass = eGS;

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
