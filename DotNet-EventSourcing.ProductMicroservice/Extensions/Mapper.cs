namespace DotNet_EventSourcing.ProductMicroservice.Extensions;

public static class Mapper
{
    public static TblProduct ToEntity(this CreateProductCommand createProductCommand)
    {
        return new TblProduct
        {
            CategoryName = createProductCommand.CategoryName,
            IsDeleted = false,
            Price = createProductCommand.Price,
            ProductId = Guid.NewGuid(),
            ProductName = createProductCommand.ProductName,
        };
    }
}
