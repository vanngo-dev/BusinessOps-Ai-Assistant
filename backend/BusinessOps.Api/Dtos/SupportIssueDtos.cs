namespace BusinessOps.Api.Dtos;

public class SupportIssueDto
{
    public int Id { get; set; }
    public int? RelatedOrderId { get; set; }
    public string? RelatedOrderNumber { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}