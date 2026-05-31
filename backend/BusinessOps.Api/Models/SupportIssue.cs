namespace BusinessOps.Api.Models;

public class SupportIssue
{
    public int Id { get; set; }

    public int? RelatedOrderId { get; set; }

    public Order? RelatedOrder { get; set; }

    public string IssueType { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }
}