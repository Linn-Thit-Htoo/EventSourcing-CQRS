namespace DotNet_EventSourcing.ProductMicroservice.Features.Product.UpdateProduct;

public class UpdateProductCommand : IRequest<Result<UpdateProductResponse>>
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public double Price { get; set; }
}
