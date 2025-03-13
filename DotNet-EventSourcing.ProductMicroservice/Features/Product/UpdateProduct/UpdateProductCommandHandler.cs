
namespace DotNet_EventSourcing.ProductMicroservice.Features.Product.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
{
    private readonly IRepositoryBase<TblProduct> _productRepository;
    private readonly IBus _bus;

    public UpdateProductCommandHandler(IRepositoryBase<TblProduct> productRepository, IBus bus)
    {
        _productRepository = productRepository;
        _bus = bus;
    }

    public async Task<Result<UpdateProductResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<UpdateProductResponse> result;
        var transaction = await _productRepository.BeginTransactionAsync(cancellationToken);

        var item = await _productRepository
            .GetByCondition(x => x.ProductId == request.ProductId && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);
        if (item is null)
        {
            result = Result<UpdateProductResponse>.NotFound("Product not found.");
            goto result;
        }

        item.ProductName = request.ProductName;
        item.CategoryName = request.CategoryName;
        item.Price = request.Price;
        _productRepository.Update(item);

        await _productRepository.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        DomainEvent domainEvent =
            new()
            {
                AggregateType = "Product",
                EventType = "ProductUpdated",
                EventData = JsonConvert.SerializeObject(request),
                StreamId = request.ProductId
            };
        _bus.Send("DirectExchange", "EventQueue", "eventdirect", domainEvent);

        result = Result<UpdateProductResponse>.Success();

    result:
        return result;
    }
}
