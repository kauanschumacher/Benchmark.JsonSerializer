using System.Text.Json.Nodes;

namespace Benchmark.JsonSerializers.Extensions;

public static class JsonNodeExtensions
{
    public static JsonNode Clone(
        this JsonNode node, 
        IEnumerable<KeyValuePair<string, JsonNode?>>? prependProperties = null,
        IEnumerable<KeyValuePair<string, JsonNode?>>? appendProperties = null) =>
        node switch
        {
            JsonArray jsonArray => new JsonArray(GetProperties(jsonArray).ToArray()),
            JsonObject jsonObject => new JsonObject(GetPropertiesWithAppendAndPrepend(jsonObject, prependProperties, appendProperties)),
            _ => node
        };

    private static IEnumerable<KeyValuePair<string, JsonNode?>> GetPropertiesWithAppendAndPrepend(JsonNode node, IEnumerable<KeyValuePair<string, JsonNode?>>? prependProperties, IEnumerable<KeyValuePair<string, JsonNode?>>? appendProperties)
    {
        foreach (var property in prependProperties ?? Enumerable.Empty<KeyValuePair<string, JsonNode?>>())
            yield return new KeyValuePair<string, JsonNode?>(property.Key, GetPropertyValue(property.Value));
        
        if (node is JsonObject jsonObject)
            foreach (var property in GetProperties(jsonObject))
                yield return new KeyValuePair<string, JsonNode?>(property.Key, GetPropertyValue(property.Value));
        
        foreach (var property in appendProperties ?? Enumerable.Empty<KeyValuePair<string, JsonNode?>>())
            yield return new KeyValuePair<string, JsonNode?>(property.Key, GetPropertyValue(property.Value));
    }

    private static JsonNode? GetPropertyValue(JsonNode? jsonNode) 
        => jsonNode switch
        {
            JsonValue jsonValue => ParseJsonNode(jsonValue),
            JsonObject jsonObject => new JsonObject(GetProperties(jsonObject)),
            JsonArray jsonArray => new JsonArray(GetProperties(jsonArray).ToArray()),
            _ => null
        };

    private static JsonNode? ParseJsonNode(JsonNode? jsonValue) 
        => jsonValue?.Parent is not null 
            ? JsonNode.Parse(jsonValue.ToJsonString()) 
            : jsonValue;

    private static IDictionary<string, JsonNode?> GetProperties(JsonObject jsonObject)
        => jsonObject.ToDictionary(x => x.Key, x => GetPropertyValue(x.Value));

    private static IEnumerable<JsonNode?> GetProperties(JsonArray jsonArray)
        => jsonArray.Select(GetPropertyValue);
}