namespace BusinessOps.Api.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}