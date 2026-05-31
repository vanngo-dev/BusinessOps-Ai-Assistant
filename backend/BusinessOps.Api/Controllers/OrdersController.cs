using BusinessOps.Api.Data;
using BusinessOps.Api.Dtos;
using BusinessOps.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await _db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => ToDto(o))
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
        {
            return NotFound(new { message = $"Order with ID {id} was not found." });
        }

        return Ok(ToDto(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OrderNumber))
        {
            return BadRequest(new { message = "Order number is required." });
        }

        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            return BadRequest(new { message = "Customer name is required." });
        }

        var orderExists = await _db.Orders.AnyAsync(o => o.OrderNumber == request.OrderNumber);

        if (orderExists)
        {
            return Conflict(new { message = $"Order number {request.OrderNumber} already exists." });
        }

        var order = new Order
        {
            OrderNumber = request.OrderNumber.Trim(),
            CustomerName = request.CustomerName.Trim(),
            Status = request.Status.Trim(),
            OrderDate = request.OrderDate,
            RequiredShipDate = request.RequiredShipDate,
            TotalAmount = request.TotalAmount,
            Notes = request.Notes
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, ToDto(order));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderRequest request)
    {
        var order = await _db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
        {
            return NotFound(new { message = $"Order with ID {id} was not found." });
        }

        if (string.IsNullOrWhiteSpace(request.OrderNumber))
        {
            return BadRequest(new { message = "Order number is required." });
        }

        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            return BadRequest(new { message = "Customer name is required." });
        }

        var orderNumberExists = await _db.Orders
            .AnyAsync(o => o.OrderNumber == request.OrderNumber && o.Id != id);

        if (orderNumberExists)
        {
            return Conflict(new { message = $"Another order with number {request.OrderNumber} already exists." });
        }

        order.OrderNumber = request.OrderNumber.Trim();
        order.CustomerName = request.CustomerName.Trim();
        order.Status = request.Status.Trim();
        order.OrderDate = request.OrderDate;
        order.RequiredShipDate = request.RequiredShipDate;
        order.TotalAmount = request.TotalAmount;
        order.Notes = request.Notes;

        await _db.SaveChangesAsync();

        return Ok(ToDto(order));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _db.Orders.FindAsync(id);

        if (order is null)
        {
            return NotFound(new { message = $"Order with ID {id} was not found." });
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerName = order.CustomerName,
            Status = order.Status,
            OrderDate = order.OrderDate,
            RequiredShipDate = order.RequiredShipDate,
            TotalAmount = order.TotalAmount,
            Notes = order.Notes,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                ProductSku = oi.Product?.Sku,
                ProductName = oi.Product?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };
    }
}