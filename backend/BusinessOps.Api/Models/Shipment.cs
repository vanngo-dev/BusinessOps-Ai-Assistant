namespace BusinessOps.Api.Models;

public class Shipment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order? Order { get; set; }

    public string Carrier { get; set; } = string.Empty;

    public string? TrackingNumber { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? ShippedDate { get; set; }

    public DateTime? DeliveredDate { get; set; }

    public string? DelayReason { get; set; }
}