namespace DotNet_EventSourcing.ProductMicroservice.Events
{
    public class ProductCreatedEvent : ProductEvent
    {
        protected string EventType { get; private set; } = "ProductCreated";
        public string EventData { get; set; }
    }
}
