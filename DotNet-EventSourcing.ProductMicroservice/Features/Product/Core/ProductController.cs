global using DotNet_EventSourcing.ProductMicroservice.Features.Product.UpdateProduct;
global using MediatR;

namespace DotNet_EventSourcing.ProductMicroservice.Features.Product.Core;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProductAsync(
        CreateProductCommand command,
        CancellationToken cs
    )
    {
        var result = await _sender.Send(command, cs);
        return Ok(result);
    }

    [HttpPut("UpdateBlog")]
    public async Task<IActionResult> UpdateBlogAsync(
        UpdateProductCommand command,
        CancellationToken cs
    )
    {
        var result = await _sender.Send(command, cs);
        return Ok(result);
    }
}
