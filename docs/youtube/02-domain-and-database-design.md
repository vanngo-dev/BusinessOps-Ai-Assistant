# Phase 2 — Domain and Database Design

## Video Title

Designing the Business Data Model for BusinessOps AI Assistant

## Goal

In this phase, we design the core business entities that will power the application.

## What Was Built

Created C# model classes for:

- Product
- Order
- OrderItem
- Shipment
- SupportIssue

## Why Domain Modeling Matters

Business applications should start with the real-world workflow.

Before building dashboards or AI features, the system needs to understand the main business objects:

- What products exist?
- What orders are open?
- What products are inside each order?
- What shipments are delayed?
- What support issues need attention?

Good models make the rest of the app easier to build.

## Product Model

The Product model represents a SKU or inventory item.

It contains fields like:

- SKU
- Name
- Category
- Current stock
- Reorder point
- Unit cost

This allows the app to later detect low-stock products and inventory risk.

## Order Model

The Order model represents a customer or business order.

It contains:

- Order number
- Customer name
- Status
- Order date
- Required ship date
- Total amount
- Notes

This supports order tracking, delayed order detection, and revenue summaries.

## OrderItem Model

The OrderItem model connects orders and products.

An order can contain multiple products, and a product can appear in many orders.

This is important because real business orders are usually made of multiple line items.

## Shipment Model

The Shipment model tracks fulfillment.

It contains:

- Carrier
- Tracking number
- Status
- Shipped date
- Delivered date
- Delay reason

This lets the app detect missing tracking numbers and delayed shipments.

## SupportIssue Model

The SupportIssue model tracks operational or customer problems.

It contains:

- Issue type
- Priority
- Status
- Description
- Created date
- Resolved date
- Optional related order

This gives the future AI assistant real issues to summarize.

## Entity Relationships

The main relationships are:

```text
Order 1 -> many OrderItems
Product 1 -> many OrderItems
Order 1 -> many Shipments
Order 1 -> many SupportIssues