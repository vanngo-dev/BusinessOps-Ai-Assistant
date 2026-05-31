using BusinessOps.Api.Data;
using BusinessOps.Api.Dtos;
using BusinessOps.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _db.Products
            .OrderBy(p => p.Sku)
            .Select(p => ToDto(p))
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        return Ok(ToDto(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Sku))
        {
            return BadRequest(new { message = "SKU is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Product name is required." });
        }

        var skuExists = await _db.Products.AnyAsync(p => p.Sku == request.Sku);

        if (skuExists)
        {
            return Conflict(new { message = $"A product with SKU {request.Sku} already exists." });
        }

        var product = new Product
        {
            Sku = request.Sku.Trim(),
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            CurrentStock = request.CurrentStock,
            ReorderPoint = request.ReorderPoint,
            UnitCost = request.UnitCost,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ToDto(product));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductRequest request)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        if (string.IsNullOrWhiteSpace(request.Sku))
        {
            return BadRequest(new { message = "SKU is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Product name is required." });
        }

        var skuExists = await _db.Products
            .AnyAsync(p => p.Sku == request.Sku && p.Id != id);

        if (skuExists)
        {
            return Conflict(new { message = $"Another product with SKU {request.Sku} already exists." });
        }

        product.Sku = request.Sku.Trim();
        product.Name = request.Name.Trim();
        product.Category = request.Category.Trim();
        product.CurrentStock = request.CurrentStock;
        product.ReorderPoint = request.ReorderPoint;
        product.UnitCost = request.UnitCost;
        product.IsActive = request.IsActive;

        await _db.SaveChangesAsync();

        return Ok(ToDto(product));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        var hasOrderItems = await _db.OrderItems.AnyAsync(oi => oi.ProductId == id);

        if (hasOrderItems)
        {
            return Conflict(new
            {
                message = "Cannot delete product because it is linked to existing order items."
            });
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Category = product.Category,
            CurrentStock = product.CurrentStock,
            ReorderPoint = product.ReorderPoint,
            UnitCost = product.UnitCost,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}