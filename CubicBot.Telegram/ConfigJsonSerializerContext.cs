using System.Text.Json.Serialization;

namespace CubicBot.Telegram;

[JsonSerializable(typeof(Config))]
[JsonSourceGenerationOptions(
    IgnoreReadOnlyProperties = true,
    WriteIndented = true)]
public partial class ConfigJsonSerializerContext : JsonSerializerContext
{
}
