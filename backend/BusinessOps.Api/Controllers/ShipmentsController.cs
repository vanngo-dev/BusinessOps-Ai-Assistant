using BusinessOps.Api.Data;
using BusinessOps.Api.Dtos;
using BusinessOps.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ShipmentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ShipmentDto>>> GetShipments()
    {
        var shipments = await _db.Shipments
            .Include(s => s.Order)
            .OrderByDescending(s => s.ShippedDate ?? DateTime.MinValue)
            .Select(s => ToDto(s))
            .ToListAsync();

        return Ok(shipments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShipmentDto>> GetShipment(int id)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Order)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment is null)
        {
            return NotFound(new { message = $"Shipment with ID {id} was not found." });
        }

        return Ok(ToDto(shipment));
    }

    [HttpPost]
    public async Task<ActionResult<ShipmentDto>> CreateShipment(CreateShipmentRequest request)
    {
        var orderExists = await _db.Orders.AnyAsync(o => o.Id == request.OrderId);

        if (!orderExists)
        {
            return BadRequest(new { message = $"Order with ID {request.OrderId} does not exist." });
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(new { message = "Shipment status is required." });
        }

        var shipment = new Shipment
        {
            OrderId = request.OrderId,
            Carrier = request.Carrier.Trim(),
            TrackingNumber = request.TrackingNumber,
            Status = request.Status.Trim(),
            ShippedDate = request.ShippedDate,
            DeliveredDate = request.DeliveredDate,
            DelayReason = request.DelayReason
        };

        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();

        var createdShipment = await _db.Shipments
            .Include(s => s.Order)
            .FirstAsync(s => s.Id == shipment.Id);

        return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, ToDto(createdShipment));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShipmentDto>> UpdateShipment(int id, UpdateShipmentRequest request)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Order)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment is null)
        {
            return NotFound(new { message = $"Shipment with ID {id} was not found." });
        }

        var orderExists = await _db.Orders.AnyAsync(o => o.Id == request.OrderId);

        if (!orderExists)
        {
            return BadRequest(new { message = $"Order with ID {request.OrderId} does not exist." });
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(new { message = "Shipment status is required." });
        }

        shipment.OrderId = request.OrderId;
        shipment.Carrier = request.Carrier.Trim();
        shipment.TrackingNumber = request.TrackingNumber;
        shipment.Status = request.Status.Trim();
        shipment.ShippedDate = request.ShippedDate;
        shipment.DeliveredDate = request.DeliveredDate;
        shipment.DelayReason = request.DelayReason;

        await _db.SaveChangesAsync();

        var updatedShipment = await _db.Shipments
            .Include(s => s.Order)
            .FirstAsync(s => s.Id == shipment.Id);

        return Ok(ToDto(updatedShipment));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteShipment(int id)
    {
        var shipment = await _db.Shipments.FindAsync(id);

        if (shipment is null)
        {
            return NotFound(new { message = $"Shipment with ID {id} was not found." });
        }

        _db.Shipments.Remove(shipment);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static ShipmentDto ToDto(Shipment shipment)
    {
        return new ShipmentDto
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            OrderNumber = shipment.Order?.OrderNumber,
            Carrier = shipment.Carrier,
            TrackingNumber = shipment.TrackingNumber,
            Status = shipment.Status,
            ShippedDate = shipment.ShippedDate,
            DeliveredDate = shipment.DeliveredDate,
            DelayReason = shipment.DelayReason
        };
    }
}