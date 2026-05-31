using BusinessOps.Api.Data;
using BusinessOps.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Services;

public class OperationsInsightsService
{
    private readonly AppDbContext _db;

    public OperationsInsightsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OperationsSummaryDto> GetOperationsSummaryAsync()
    {
        var totalProducts = await _db.Products.CountAsync();

        var lowStockProducts = await _db.Products
            .CountAsync(p => p.IsActive && p.CurrentStock <= p.ReorderPoint);

        var openOrders = await _db.Orders
            .CountAsync(o =>
                o.Status == "Open" ||
                o.Status == "Processing");

        var delayedShipments = await _db.Shipments
            .CountAsync(s =>
                s.Status == "Delayed" ||
                s.DelayReason != null ||
                (
                    s.ShippedDate == null &&
                    s.Order != null &&
                    s.Order.RequiredShipDate < DateTime.UtcNow
                ));

        var ordersMissingTracking = await _db.Shipments
            .CountAsync(s =>
                s.Status != "Delivered" &&
                string.IsNullOrWhiteSpace(s.TrackingNumber));

        var highPrioritySupportIssues = await _db.SupportIssues
            .CountAsync(si =>
                si.Priority == "High" &&
                si.Status != "Resolved");

        var unresolvedSupportIssues = await _db.SupportIssues
            .CountAsync(si =>
                si.Status != "Resolved" &&
                si.ResolvedAt == null);

        var estimatedOpenOrderValue = await _db.Orders
            .Where(o =>
                o.Status == "Open" ||
                o.Status == "Processing")
            .SumAsync(o => o.TotalAmount);

        return new OperationsSummaryDto
        {
            TotalProducts = totalProducts,
            LowStockProducts = lowStockProducts,
            OpenOrders = openOrders,
            DelayedShipments = delayedShipments,
            OrdersMissingTracking = ordersMissingTracking,
            HighPrioritySupportIssues = highPrioritySupportIssues,
            UnresolvedSupportIssues = unresolvedSupportIssues,
            EstimatedOpenOrderValue = estimatedOpenOrderValue
        };
    }
}