namespace BusinessOps.Api.Dtos;

public class UpdateSupportIssueRequest
{
    public int? RelatedOrderId { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ResolvedAt { get; set; }
}