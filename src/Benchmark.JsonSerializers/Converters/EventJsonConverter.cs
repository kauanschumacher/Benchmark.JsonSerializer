using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Benchmark.JsonSerializers.Classes;
using Benchmark.JsonSerializers.Extensions;

namespace Benchmark.JsonSerializers.Converters;

public class EventJsonConverter : JsonConverter<CustomSerializerClasses.IEvent?>
{
    private const string DiscriminatorFieldName = "$type";

    private static readonly IEnumerable<Assembly> SecuredAssemblies = new[] { typeof(CustomSerializerClasses.IEvent).Assembly };

    private static readonly IEnumerable<Type> EventTypes = SecuredAssemblies
        .SelectMany(x => x.GetTypes())
        .Where(x => x.IsAssignableTo(typeof(CustomSerializerClasses.IEvent)))
        .Where(x => !x.IsAbstract);

    private static readonly IDictionary<string, Type> EventTypesByDiscriminator = EventTypes.ToDictionary(GetDiscriminator, x => x);
    
    private static readonly IDictionary<Type, string> EventDiscriminatorByTypes = EventTypes.ToDictionary(x => x, GetDiscriminator);

    private static string GetDiscriminator(Type type) 
        => $"{type.Namespace}.{type.Name}";

    public override CustomSerializerClasses.IEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty(DiscriminatorFieldName, out var discriminatorJsonElement)) throw new JsonException();

        var discriminator = discriminatorJsonElement.GetString() ?? string.Empty;
        if (!EventTypesByDiscriminator.TryGetValue(discriminator, out var type)) throw new JsonException();

        var jsonObject = jsonDocument.RootElement.GetRawText();
        return (CustomSerializerClasses.IEvent) JsonSerializer.Deserialize(jsonObject, type, options)!;
    }

    public override void Write(Utf8JsonWriter writer, CustomSerializerClasses.IEvent? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;            
        }

        var type = value.GetType();
        if (!EventDiscriminatorByTypes.TryGetValue(type, out var discriminator)) throw new JsonException();
        JsonSerializer.SerializeToNode(value, type, options)?
            .Clone(prependProperties: GetPrependProperties(discriminator))
            .WriteTo(writer, options);
    }

    private IEnumerable<KeyValuePair<string, JsonNode?>> GetPrependProperties(string discriminator)
    {
        yield return new KeyValuePair<string, JsonNode?>(DiscriminatorFieldName, discriminator);
    }
}