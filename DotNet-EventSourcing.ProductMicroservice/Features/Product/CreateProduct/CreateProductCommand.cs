namespace DotNet_EventSourcing.ProductMicroservice.Features.Product.CreateProduct;

public class CreateProductCommand : IRequest<Result<CreateProductResponse>>
{
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public double Price { get; set; }
}
