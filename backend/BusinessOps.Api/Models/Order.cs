namespace BusinessOps.Api.Models;

public class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public DateTime RequiredShipDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

    public List<Shipment> Shipments { get; set; } = new();

    public List<SupportIssue> SupportIssues { get; set; } = new();
}