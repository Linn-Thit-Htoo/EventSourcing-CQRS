namespace DotNet_EventSourcing.ProductMicroservice.Events
{
    public class ProductEvent
    {
        protected string AggregateType { get; set; } = "Product";
    }
}
