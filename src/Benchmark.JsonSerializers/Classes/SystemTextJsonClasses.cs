using System.Text.Json.Serialization;

namespace Benchmark.JsonSerializers.Classes;

public static class SystemTextJsonClasses
{
    [JsonDerivedType(typeof(SimpleEvent), nameof(SimpleEvent))]
    [JsonDerivedType(typeof(ComplexEvent), nameof(ComplexEvent))]
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
    public interface IEvent 
    {
    }

    public class SimpleEvent : IEvent
    {
        public Guid Id { get; set; }
        public string SomeString { get; set; }
        public string MoreOneString { get; set; }
        public decimal SomeDecimal { get; set; }
        public long SomeLong { get; set; }
    }

    public class ComplexEvent : IEvent
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


