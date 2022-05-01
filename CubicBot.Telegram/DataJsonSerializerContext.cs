using System.Text.Json.Serialization;

namespace CubicBot.Telegram;

[JsonSerializable(typeof(Data))]
[JsonSourceGenerationOptions(IgnoreReadOnlyProperties = true)]
public partial class DataJsonSerializerContext : JsonSerializerContext
{
}
