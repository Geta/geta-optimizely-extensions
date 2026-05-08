using System.Text.Json;
using System.Text.Json.Serialization;

namespace Geta.Optimizely.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = includeNull
                    ? JsonIgnoreCondition.Never
                    : JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
