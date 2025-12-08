namespace CubicBot.Telegram.Commands;

public sealed class CommandsConfig
{
    public bool EnablePersonal { get; set; } = true;
    public bool EnableCommon { get; set; } = true;
    public bool EnableDice { get; set; } = true;
    public bool EnableConsentNotNeeded { get; set; } = true;
    public bool EnableNonVegan { get; set; } = true;
    public bool EnableLawEnforcement { get; set; } = true;
    public bool EnablePublicServices { get; set; } = true;
    public bool EnableChinese { get; set; } = true;
    public bool EnableChineseTasks { get; set; } = true;
    public bool EnableSystemd { get; set; } = true;
}
