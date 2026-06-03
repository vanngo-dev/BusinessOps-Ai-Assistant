# Phase 5 — Business Insights API

## Video Title

Adding Business Insights Before AI in BusinessOps AI Assistant

## Goal

In this phase, we add a business insights endpoint that calculates useful operational metrics from structured database records.

## What Was Built

- OperationsSummaryDto
- OperationsInsightsService
- InsightsController
- GET /api/insights/operations-summary endpoint

## Why This Phase Matters

AI should not replace basic business logic.

Before asking AI to summarize a business, the application should first calculate reliable structured metrics.

This gives the system a clear operational signal.

## Metrics Calculated

The operations summary endpoint calculates:

- Total products
- Low-stock products
- Open orders
- Delayed shipments
- Orders missing tracking numbers
- High-priority support issues
- Unresolved support issues
- Estimated open order value

## Why These Metrics Were Chosen

These metrics represent common operational risks:

- Inventory shortages
- Delayed fulfillment
- Missing shipment tracking
- Customer support pressure
- Open revenue tied to unfulfilled orders

These are the kinds of problems a business manager would want to see quickly.

## Endpoint

```text
GET /api/insights/operations-summary
{
  "totalProducts": 10,
  "lowStockProducts": 5,
  "openOrders": 5,
  "delayedShipments": 3,
  "ordersMissingTracking": 3,
  "highPrioritySupportIssues": 4,
  "unresolvedSupportIssues": 7,
  "estimatedOpenOrderValue": 9546.25
}