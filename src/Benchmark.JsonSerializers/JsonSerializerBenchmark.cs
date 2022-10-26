using System.Text.Json;
using Benchmark.JsonSerializers.Classes;
using Benchmark.JsonSerializers.Converters;
using BenchmarkDotNet.Attributes;

namespace Benchmark.JsonSerializers;

[MemoryDiagnoser(false)]
public class JsonSerializerBenchmark
{
    private static readonly SystemTextJsonClasses.IEvent Native_SomeEvent = new SystemTextJsonClasses.SomeEvent(); 
    private static readonly CustomSerializerClasses.IEvent Custom_AnotherEvent = new CustomSerializerClasses.AnotherEvent(); 
    
    private static readonly JsonSerializerOptions Custom_Options = new()
    {
        Converters = { new EventJsonConverter() }
    }; 
    
    private static readonly JsonSerializerOptions DerivedType_Options = new()
    {
        Converters = { new DerivedTypeJsonConverter<CustomSerializerClasses.IEvent>() }
    }; 
    
    [Benchmark]
    public CustomSerializerClasses.IEvent? CustomSerializer_Deserialize()
    {
        return JsonSerializer.Deserialize<CustomSerializerClasses.IEvent>("""{"Id":"00000000-0000-0000-0000-000000000000","SomeData":{"SomeString":null,"MoreOneString":null},"$type":"Benchmark.JsonSerializers.Classes.ComplexEvent"}""", new JsonSerializerOptions()
        {
            Converters = { new EventJsonConverter() }
        });
    }
    
    [Benchmark]
    public CustomSerializerClasses.IEvent? DerivedTypeSerializer_Deserialize()
    {
        return JsonSerializer.Deserialize<CustomSerializerClasses.IEvent>("""{"Id":"00000000-0000-0000-0000-000000000000","SomeData":{"SomeString":null,"MoreOneString":null},"$type":"Benchmark.JsonSerializers.Classes.ComplexEvent"}""", new JsonSerializerOptions()
        {
            Converters = { new DerivedTypeJsonConverter<CustomSerializerClasses.IEvent>() }
        });
    }
    [Benchmark]
    public SystemTextJsonClasses.IEvent? SystemTextJson_Deserialize()
    {
        return JsonSerializer.Deserialize<SystemTextJsonClasses.IEvent>("""{"$type":"ComplexEvent","Id":"00000000-0000-0000-0000-000000000000","SomeData":{"SomeString":null,"MoreOneString":null}}""");
    }
    
    [Benchmark]
    public string SystemTextJson_Serialize()
    {
        return JsonSerializer.Serialize(Native_SomeEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public string CustomSerializer_Serialize()
    {
        return JsonSerializer.Serialize(Custom_AnotherEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
    
    [Benchmark]
    public string DerivedTypeSerializer_Serialize()
    {
        return JsonSerializer.Serialize(Custom_AnotherEvent, typeof(CustomSerializerClasses.IEvent), DerivedType_Options);
    }
    
    [Benchmark]
    public async Task SystemTextJson_Serialize_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Native_SomeEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public async Task CustomSerializer_Serialize_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Custom_AnotherEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
    
    [Benchmark]
    public async Task DerivedTypeSerializer_Serialize_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Custom_AnotherEvent, typeof(CustomSerializerClasses.IEvent), DerivedType_Options);
    }
}