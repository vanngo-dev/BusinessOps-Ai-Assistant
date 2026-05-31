namespace BusinessOps.Api.Dtos;

public class ShipmentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? DelayReason { get; set; }
}