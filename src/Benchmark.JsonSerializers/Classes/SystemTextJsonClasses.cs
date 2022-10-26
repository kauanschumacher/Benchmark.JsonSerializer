using System.Text.Json.Serialization;

namespace Benchmark.JsonSerializers.Classes;

public static class SystemTextJsonClasses
{
    [JsonDerivedType(typeof(SomeEvent), nameof(SomeEvent))]
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
    public interface IEvent 
    {
    }

    public class SomeEvent : IEvent
    {
        public Guid Id { get; set; }
        public SomeDto SomeData { get; set; }
    }

    public class SomeDto
    {
        public string SomeString { get; set; }
        public string MoreOneString { get; set; }
    }
}


