using BusinessOps.Api.Models;

namespace BusinessOps.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Products.Any())
        {
            return;
        }

        var products = new List<Product>
        {
            new()
            {
                Sku = "SKU-1001",
                Name = "Industrial Sensor Module",
                Category = "Electronics",
                CurrentStock = 8,
                ReorderPoint = 15,
                UnitCost = 42.50m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1002",
                Name = "Control Board Assembly",
                Category = "Electronics",
                CurrentStock = 4,
                ReorderPoint = 10,
                UnitCost = 88.75m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1003",
                Name = "Warehouse Barcode Scanner",
                Category = "Hardware",
                CurrentStock = 22,
                ReorderPoint = 8,
                UnitCost = 135.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1004",
                Name = "Thermal Shipping Label Roll",
                Category = "Supplies",
                CurrentStock = 6,
                ReorderPoint = 20,
                UnitCost = 11.25m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1005",
                Name = "Protective Packaging Kit",
                Category = "Supplies",
                CurrentStock = 30,
                ReorderPoint = 12,
                UnitCost = 7.80m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1006",
                Name = "Replacement Power Adapter",
                Category = "Electronics",
                CurrentStock = 14,
                ReorderPoint = 10,
                UnitCost = 19.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1007",
                Name = "Inventory Bin Small",
                Category = "Warehouse",
                CurrentStock = 50,
                ReorderPoint = 25,
                UnitCost = 3.50m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1008",
                Name = "Inventory Bin Large",
                Category = "Warehouse",
                CurrentStock = 12,
                ReorderPoint = 18,
                UnitCost = 6.75m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1009",
                Name = "USB Diagnostic Cable",
                Category = "Electronics",
                CurrentStock = 40,
                ReorderPoint = 15,
                UnitCost = 9.99m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Sku = "SKU-1010",
                Name = "Return Processing Label",
                Category = "Supplies",
                CurrentStock = 3,
                ReorderPoint = 25,
                UnitCost = 2.25m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        db.Products.AddRange(products);
        await db.SaveChangesAsync();

        var orders = new List<Order>
        {
            new()
            {
                OrderNumber = "ORD-5001",
                CustomerName = "Northwind Manufacturing",
                Status = "Open",
                OrderDate = DateTime.UtcNow.AddDays(-7),
                RequiredShipDate = DateTime.UtcNow.AddDays(-1),
                TotalAmount = 1450.00m,
                Notes = "Past required ship date. Needs review."
            },
            new()
            {
                OrderNumber = "ORD-5002",
                CustomerName = "Blue Ridge Supply",
                Status = "Processing",
                OrderDate = DateTime.UtcNow.AddDays(-5),
                RequiredShipDate = DateTime.UtcNow.AddDays(2),
                TotalAmount = 780.50m,
                Notes = "Waiting for inventory allocation."
            },
            new()
            {
                OrderNumber = "ORD-5003",
                CustomerName = "Pacific Retail Group",
                Status = "Open",
                OrderDate = DateTime.UtcNow.AddDays(-3),
                RequiredShipDate = DateTime.UtcNow.AddDays(4),
                TotalAmount = 2340.00m,
                Notes = null
            },
            new()
            {
                OrderNumber = "ORD-5004",
                CustomerName = "Apex Distribution",
                Status = "Shipped",
                OrderDate = DateTime.UtcNow.AddDays(-10),
                RequiredShipDate = DateTime.UtcNow.AddDays(-5),
                TotalAmount = 980.25m,
                Notes = "Shipped late due to label stock shortage."
            },
            new()
            {
                OrderNumber = "ORD-5005",
                CustomerName = "Summit Online Goods",
                Status = "Open",
                OrderDate = DateTime.UtcNow.AddDays(-2),
                RequiredShipDate = DateTime.UtcNow.AddDays(1),
                TotalAmount = 520.00m,
                Notes = "Customer requested rush processing."
            },
            new()
            {
                OrderNumber = "ORD-5006",
                CustomerName = "Redwood Components",
                Status = "Processing",
                OrderDate = DateTime.UtcNow.AddDays(-6),
                RequiredShipDate = DateTime.UtcNow,
                TotalAmount = 3100.00m,
                Notes = "Large order, verify stock before release."
            },
            new()
            {
                OrderNumber = "ORD-5007",
                CustomerName = "Metro Service Depot",
                Status = "Cancelled",
                OrderDate = DateTime.UtcNow.AddDays(-8),
                RequiredShipDate = DateTime.UtcNow.AddDays(-3),
                TotalAmount = 250.00m,
                Notes = "Cancelled due to duplicate order."
            },
            new()
            {
                OrderNumber = "ORD-5008",
                CustomerName = "Harbor Logistics",
                Status = "Open",
                OrderDate = DateTime.UtcNow.AddDays(-1),
                RequiredShipDate = DateTime.UtcNow.AddDays(3),
                TotalAmount = 1125.75m,
                Notes = "Needs tracking once fulfilled."
            }
        };

        db.Orders.AddRange(orders);
        await db.SaveChangesAsync();

        var orderItems = new List<OrderItem>
        {
            new() { OrderId = orders[0].Id, ProductId = products[0].Id, Quantity = 10, UnitPrice = 69.99m },
            new() { OrderId = orders[0].Id, ProductId = products[3].Id, Quantity = 15, UnitPrice = 18.00m },
            new() { OrderId = orders[1].Id, ProductId = products[1].Id, Quantity = 3, UnitPrice = 129.00m },
            new() { OrderId = orders[1].Id, ProductId = products[5].Id, Quantity = 5, UnitPrice = 29.99m },
            new() { OrderId = orders[2].Id, ProductId = products[2].Id, Quantity = 8, UnitPrice = 199.00m },
            new() { OrderId = orders[2].Id, ProductId = products[8].Id, Quantity = 20, UnitPrice = 15.99m },
            new() { OrderId = orders[3].Id, ProductId = products[4].Id, Quantity = 50, UnitPrice = 12.50m },
            new() { OrderId = orders[3].Id, ProductId = products[3].Id, Quantity = 10, UnitPrice = 18.00m },
            new() { OrderId = orders[4].Id, ProductId = products[9].Id, Quantity = 30, UnitPrice = 4.99m },
            new() { OrderId = orders[4].Id, ProductId = products[6].Id, Quantity = 25, UnitPrice = 6.99m },
            new() { OrderId = orders[5].Id, ProductId = products[1].Id, Quantity = 12, UnitPrice = 129.00m },
            new() { OrderId = orders[5].Id, ProductId = products[0].Id, Quantity = 18, UnitPrice = 69.99m },
            new() { OrderId = orders[6].Id, ProductId = products[8].Id, Quantity = 5, UnitPrice = 15.99m },
            new() { OrderId = orders[7].Id, ProductId = products[7].Id, Quantity = 40, UnitPrice = 9.99m },
            new() { OrderId = orders[7].Id, ProductId = products[4].Id, Quantity = 30, UnitPrice = 12.50m }
        };

        db.OrderItems.AddRange(orderItems);
        await db.SaveChangesAsync();

        var shipments = new List<Shipment>
        {
            new()
            {
                OrderId = orders[0].Id,
                Carrier = "UPS",
                TrackingNumber = null,
                Status = "Pending",
                ShippedDate = null,
                DeliveredDate = null,
                DelayReason = "Waiting for low-stock sensor modules"
            },
            new()
            {
                OrderId = orders[2].Id,
                Carrier = "FedEx",
                TrackingNumber = "FX123456789",
                Status = "In Transit",
                ShippedDate = DateTime.UtcNow.AddDays(-1),
                DeliveredDate = null,
                DelayReason = null
            },
            new()
            {
                OrderId = orders[3].Id,
                Carrier = "USPS",
                TrackingNumber = "US987654321",
                Status = "Delivered",
                ShippedDate = DateTime.UtcNow.AddDays(-6),
                DeliveredDate = DateTime.UtcNow.AddDays(-2),
                DelayReason = "Label supply shortage delayed fulfillment"
            },
            new()
            {
                OrderId = orders[4].Id,
                Carrier = "UPS",
                TrackingNumber = null,
                Status = "Pending",
                ShippedDate = null,
                DeliveredDate = null,
                DelayReason = null
            },
            new()
            {
                OrderId = orders[5].Id,
                Carrier = "DHL",
                TrackingNumber = "DHL555000111",
                Status = "Delayed",
                ShippedDate = DateTime.UtcNow.AddDays(-3),
                DeliveredDate = null,
                DelayReason = "Carrier exception at regional hub"
            },
            new()
            {
                OrderId = orders[7].Id,
                Carrier = "FedEx",
                TrackingNumber = null,
                Status = "Pending",
                ShippedDate = null,
                DeliveredDate = null,
                DelayReason = "Awaiting warehouse pick confirmation"
            }
        };

        db.Shipments.AddRange(shipments);
        await db.SaveChangesAsync();

        var supportIssues = new List<SupportIssue>
        {
            new()
            {
                RelatedOrderId = orders[0].Id,
                IssueType = "Delayed Shipment",
                Priority = "High",
                Status = "Open",
                Description = "Customer asking why order has not shipped after required ship date.",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = orders[1].Id,
                IssueType = "Inventory Shortage",
                Priority = "High",
                Status = "Open",
                Description = "Control board assembly stock may not cover current order demand.",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = orders[3].Id,
                IssueType = "Late Fulfillment",
                Priority = "Medium",
                Status = "Resolved",
                Description = "Shipment was delayed due to label roll shortage.",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                ResolvedAt = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                RelatedOrderId = orders[5].Id,
                IssueType = "Carrier Delay",
                Priority = "High",
                Status = "In Progress",
                Description = "DHL shipment delayed at regional hub.",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = null,
                IssueType = "Warehouse Process",
                Priority = "Medium",
                Status = "Open",
                Description = "Pick confirmation process is causing tracking assignment delays.",
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = orders[4].Id,
                IssueType = "Rush Order",
                Priority = "Medium",
                Status = "Open",
                Description = "Customer requested rush handling but order has not been picked.",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = null,
                IssueType = "Low Stock",
                Priority = "High",
                Status = "Open",
                Description = "Return processing labels are below reorder point.",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                ResolvedAt = null
            },
            new()
            {
                RelatedOrderId = orders[7].Id,
                IssueType = "Missing Tracking",
                Priority = "Low",
                Status = "Open",
                Description = "Tracking number not assigned yet for pending shipment.",
                CreatedAt = DateTime.UtcNow,
                ResolvedAt = null
            }
        };

        db.SupportIssues.AddRange(supportIssues);
        await db.SaveChangesAsync();
    }
}