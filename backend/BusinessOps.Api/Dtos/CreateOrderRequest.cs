namespace BusinessOps.Api.Dtos;

public class CreateOrderRequest
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime RequiredShipDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
}