using System.Text.Json;
using System.Text.Json.Serialization;

namespace Geta.Optimizely.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions OptionsWithNulls = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() }
        };

        private static readonly JsonSerializerOptions OptionsWithoutNulls = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            return JsonSerializer.Serialize(obj, includeNull ? OptionsWithNulls : OptionsWithoutNulls);
        }
    }
}
