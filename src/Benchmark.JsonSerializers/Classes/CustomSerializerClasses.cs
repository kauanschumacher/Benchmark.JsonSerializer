namespace Benchmark.JsonSerializers.Classes;

public static class CustomSerializerClasses
{
    public interface IEvent 
    {
    }

    public class AnotherEvent : IEvent
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