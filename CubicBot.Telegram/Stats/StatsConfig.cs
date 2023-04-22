namespace CubicBot.Telegram.Stats;

public sealed class StatsConfig
{
    public bool EnableGrass { get; set; } = true;

    public bool EnableCommandStats { get; set; } = true;

    public bool EnableMessageCounter { get; set; } = true;

    public bool EnableParenthesisEnclosure { get; set; } = true;
}
