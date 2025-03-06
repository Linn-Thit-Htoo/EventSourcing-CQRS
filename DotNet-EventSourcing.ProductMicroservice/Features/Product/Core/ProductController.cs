using DotNet_EventSourcing.ProductMicroservice.Features.Product.CreateProduct;
using DotNet_EventSourcing.ProductMicroservice.Features.Product.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreateProductAsync(CreateProductCommand command, CancellationToken cs)
    {
        var result = await _sender.Send(command, cs);
        return Ok(result);
    }

    [HttpPut("UpdateBlog")]
    public async Task<IActionResult> UpdateBlogAsync(UpdateProductCommand command, CancellationToken cs)
    {
        var result = await _sender.Send(command, cs);
        return Ok(result);
    }
}
