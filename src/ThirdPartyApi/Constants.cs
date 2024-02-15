using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThirdPartyApi;

internal static class Constants
{
    public static readonly JsonSerializerOptions WriteIndentedSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };
}