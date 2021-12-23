namespace CubicBot.Telegram.CLI;

public record ConfigChangeSet(
    string? BotToken,
    bool? EnableCommandsMod,
    bool? EnableStatsMod,
    bool? EnablePersonalCommands,
    bool? EnableCommonCommands,
    bool? EnableDiceCommands,
    bool? EnableConsentNotNeededCommands,
    bool? EnableNonVeganCommands,
    bool? EnableLawEnforcementCommands,
    bool? EnablePublicServicesCommands,
    bool? EnableChineseCommands,
    bool? EnableChineseTasksCommands,
    bool? EnableSystemdCommands,
    bool? EnableGrassStats,
    bool? EnableCommandStats,
    bool? EnableMessageCounter);
