namespace DotNet_EventSourcing.ProductMicroservice.Events
{
    public class DomainEvent
    {
        public Guid StreamId { get; set; }
        public string AggregateType { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
    }
}
