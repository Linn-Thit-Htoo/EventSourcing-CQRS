using DotNet_EventSourcing.ProductMicroservice.Entities;
using DotNet_EventSourcing.ProductMicroservice.Events;
using DotNet_EventSourcing.ProductMicroservice.Extensions;
using DotNet_EventSourcing.ProductMicroservice.Persistence.Base;
using DotNet_EventSourcing.ProductMicroservice.Services.RabbitMQ;
using DotNet_EventSourcing.ProductMicroservice.Utils;
using MediatR;
using Newtonsoft.Json;

namespace DotNet_EventSourcing.ProductMicroservice.Features.Product.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
{
    private readonly IRepositoryBase<TblProduct> _productRepository;
    private readonly IBus _bus;

    public CreateProductCommandHandler(IRepositoryBase<TblProduct> productRepository, IBus bus)
    {
        _productRepository = productRepository;
        _bus = bus;
    }

    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Result<CreateProductResponse> result;

        //bool productDuplicate = await _productRepository
        //    .GetByCondition(x => x.ProductName.ToLower().Trim().Contains(request.ProductName) && !x.IsDeleted)
        //    .AnyAsync(cancellationToken);
        //if (productDuplicate)
        //{
        //    result = Result<CreateProductResponse>.Duplicate("Product already exists.");
        //    goto result;
        //}

        var entity = request.ToEntity();
        await _productRepository.AddAsync(entity, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        DomainEvent productCreatedEvent = new()
        {
            AggregateType = "Product",
            EventType = "ProductCreated",
            EventData = JsonConvert.SerializeObject(request),
            StreamId = entity.ProductId
        };
        _bus.Send("DirectExchange", "EventQueue", "eventdirect", productCreatedEvent);

        result = Result<CreateProductResponse>.Success("Saving Product Successful.");

        result:
        return result;
    }
}
