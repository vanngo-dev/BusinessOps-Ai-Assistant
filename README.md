# BusinessOps AI Assistant

BusinessOps AI Assistant is a full-stack internal tools platform for analyzing business operations data, including products, orders, shipments, inventory issues, and support tickets.

The project uses C#/.NET, React, TypeScript, SQL, and AI-assisted summaries to help identify operational risks and recommended actions.

## Why This Project Exists

Many businesses rely on scattered spreadsheets, manual checks, and disconnected systems to manage daily operations. This project demonstrates how a modern internal tool can centralize operational data and use AI to summarize issues, detect bottlenecks, and support better decision-making.

## Tech Stack

- C# / .NET Web API
- Entity Framework Core
- SQL Server
- React
- TypeScript
- Vite
- REST APIs
- AI assistant service

## Core Features Planned

- Business operations dashboard
- Products and inventory records
- Orders and shipment tracking
- Support issue tracking
- Operational insights API
- AI question assistant
- AI daily operations summary
- CSV import workflow

## Status

In progress.

Completed phases:

- Phase 0 - Project setup and documentation structure
- Phase 1 - .NET Web API foundation
- Phase 2 - Domain models
- Phase 3 - EF Core database and seed data
- Phase 4 - CRUD APIs for business data
- Phase 5 - Business Insights API
- Phase 6 - React frontend foundation
- Phase 7 - Operations Dashboard UI

## Project Goal

This portfolio project demonstrates full-stack development, business systems design, API development, database modeling, dashboard UI design, and practical AI application development.

## Running the Backend API

From the project root:

```bash
cd backend/BusinessOps.Api
dotnet run
```

Backend URL:

```text
http://localhost:5074
```

Swagger URL:

```text
http://localhost:5074/swagger
```

## Running the Frontend

From the project root:

```bash
cd frontend
npm install
npm run dev
```

Frontend URL:

```text
http://localhost:5173
```

If port `5173` is already in use, Vite may print the next available localhost port.

Frontend environment example:

```bash
VITE_API_BASE_URL=http://localhost:5074
```

## Database Setup

This project uses Entity Framework Core for relational database access.

### Install EF Tool

```bash
dotnet tool install --global dotnet-ef
```

## Business Data API Endpoints

The backend exposes CRUD endpoints for core business records.

### Products

```http
GET /api/products
GET /api/products/{id}
POST /api/products
PUT /api/products/{id}
DELETE /api/products/{id}
```

### Orders

```http
GET /api/orders
GET /api/orders/{id}
POST /api/orders
PUT /api/orders/{id}
DELETE /api/orders/{id}
```

### Shipments

```http
GET /api/shipments
GET /api/shipments/{id}
POST /api/shipments
PUT /api/shipments/{id}
DELETE /api/shipments/{id}
```

### Support Issues

```http
GET /api/support-issues
GET /api/support-issues/{id}
POST /api/support-issues
PUT /api/support-issues/{id}
DELETE /api/support-issues/{id}
```

## Business Insights API

The backend includes an operations summary endpoint that calculates useful business metrics from the database.

### Operations Summary

```http
GET /api/insights/operations-summary
```

Example response:

```json
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
```

## Operations Dashboard UI

The frontend dashboard reads the operations summary endpoint and displays:

- Current operations risk
- Open order value
- Product and inventory counts
- Fulfillment risk metrics
- Support issue metrics
- Priority queue
- Risk mix by operating area

The dashboard is currently scoped to:

```http
GET /api/insights/operations-summary
```
