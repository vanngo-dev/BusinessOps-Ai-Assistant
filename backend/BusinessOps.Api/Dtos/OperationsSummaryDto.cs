namespace BusinessOps.Api.Dtos;

public class OperationsSummaryDto
{
    public int TotalProducts { get; set; }

    public int LowStockProducts { get; set; }

    public int OpenOrders { get; set; }

    public int DelayedShipments { get; set; }

    public int OrdersMissingTracking { get; set; }

    public int HighPrioritySupportIssues { get; set; }

    public int UnresolvedSupportIssues { get; set; }

    public decimal EstimatedOpenOrderValue { get; set; }
}