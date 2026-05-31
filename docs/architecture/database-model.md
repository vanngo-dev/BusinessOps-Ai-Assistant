# Database Model

## Overview

BusinessOps AI Assistant uses a relational business data model to represent core operational workflows.

The first version of the model focuses on:

- Products
- Orders
- Order items
- Shipments
- Support issues

These entities were chosen because they reflect common internal business operations found in ERP-style systems, warehouse workflows, e-commerce workflows, and customer support processes.

## Entity: Product

A product represents a SKU or item that the business sells, stocks, or tracks.

### Fields

| Field | Purpose |
|---|---|
| Id | Primary identifier |
| Sku | Unique stock keeping unit |
| Name | Product name |
| Category | Product category |
| CurrentStock | Current inventory quantity |
| ReorderPoint | Stock level where replenishment should be considered |
| UnitCost | Internal product cost |
| IsActive | Whether the product is currently active |
| CreatedAt | When the product record was created |

### Business Purpose

Products allow the system to identify low-stock items, inventory risk, and SKU-level operational problems.

## Entity: Order

An order represents a customer or business order.

### Fields

| Field | Purpose |
|---|---|
| Id | Primary identifier |
| OrderNumber | Business-facing order number |
| CustomerName | Customer or account name |
| Status | Current order status |
| OrderDate | Date the order was created |
| RequiredShipDate | Date the order should ship by |
| TotalAmount | Total order value |
| Notes | Optional operational notes |

### Business Purpose

Orders allow the system to track customer demand, open orders, delayed orders, and total order value.

## Entity: OrderItem

An order item represents a product included inside an order.

### Fields

| Field | Purpose |
|---|---|
| Id | Primary identifier |
| OrderId | Related order |
| ProductId | Related product |
| Quantity | Quantity ordered |
| UnitPrice | Price charged for each unit |

### Business Purpose

Order items connect orders to products. This lets the system analyze which SKUs are being sold, which orders contain certain products, and how product demand affects inventory.

## Entity: Shipment

A shipment represents fulfillment and delivery tracking for an order.

### Fields

| Field | Purpose |
|---|---|
| Id | Primary identifier |
| OrderId | Related order |
| Carrier | Shipping carrier |
| TrackingNumber | Tracking number, if available |
| Status | Current shipment status |
| ShippedDate | Date shipped |
| DeliveredDate | Date delivered |
| DelayReason | Explanation for shipment delay |

### Business Purpose

Shipments allow the system to identify delayed orders, missing tracking numbers, fulfillment problems, and delivery bottlenecks.

## Entity: SupportIssue

A support issue represents a customer, internal, or operational problem.

### Fields

| Field | Purpose |
|---|---|
| Id | Primary identifier |
| RelatedOrderId | Optional related order |
| IssueType | Type of issue |
| Priority | Low, medium, high, or urgent |
| Status | Open, in progress, resolved, etc. |
| Description | Issue details |
| CreatedAt | When the issue was created |
| ResolvedAt | When the issue was resolved |

### Business Purpose

Support issues allow the system to surface operational risk, customer pain points, unresolved problems, and high-priority work.

## Relationships

### Product to OrderItem

One product can appear in many order items.

```text
Product 1 -> many OrderItems