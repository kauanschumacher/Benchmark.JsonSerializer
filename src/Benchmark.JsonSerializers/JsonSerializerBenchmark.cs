using System.Text.Json;
using Benchmark.JsonSerializers.Classes;
using Benchmark.JsonSerializers.Converters;
using BenchmarkDotNet.Attributes;

namespace Benchmark.JsonSerializers;

public class JsonSerializerBenchmark
{
    private static readonly SystemTextJsonClasses.IEvent Native_SimpleEvent = new SystemTextJsonClasses.SimpleEvent(); 
    private static readonly SystemTextJsonClasses.IEvent Native_ComplexEvent = new SystemTextJsonClasses.ComplexEvent(); 
    private static readonly CustomSerializerClasses.IEvent Custom_SimpleEvent = new CustomSerializerClasses.SimpleEvent(); 
    private static readonly CustomSerializerClasses.IEvent Custom_ComplexEvent = new CustomSerializerClasses.ComplexEvent(); 
    
    private static readonly JsonSerializerOptions Custom_Options = new()
    {
        Converters = { new EventJsonConverter() }
    }; 

    [Benchmark]
    public string SystemTextJson_SimpleEvent()
    {
        return JsonSerializer.Serialize(Native_SimpleEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public string CustomSerializer_SimpleEvent()
    {
        return JsonSerializer.Serialize(Custom_SimpleEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
    
    [Benchmark]
    public async Task SystemTextJson_SimpleEvent_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Native_SimpleEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public async Task CustomSerializer_SimpleEvent_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Custom_SimpleEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
    
    [Benchmark]
    public string SystemTextJson_ComplexEvent()
    {
        return JsonSerializer.Serialize(Native_ComplexEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public string CustomSerializer_ComplexEvent()
    {
        return JsonSerializer.Serialize(Custom_ComplexEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
    
    [Benchmark]
    public async Task SystemTextJson_ComplexEvent_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Native_ComplexEvent, typeof(SystemTextJsonClasses.IEvent));
    }
    
    [Benchmark]
    public async Task CustomSerializer_ComplexEvent_Async()
    {
        await using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, Custom_ComplexEvent, typeof(CustomSerializerClasses.IEvent), Custom_Options);
    }
}