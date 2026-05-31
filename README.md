# BusinessOps AI Assistant

BusinessOps AI Assistant is a full-stack internal tools platform for analyzing business operations data, including products, orders, shipments, inventory issues, and support tickets.

The project uses C#/.NET, React, TypeScript, SQL, and AI-assisted summaries to help identify operational risks and recommended actions.

## Why This Project Exists

Many businesses rely on scattered spreadsheets, manual checks, and disconnected systems to manage daily operations. This project demonstrates how a modern internal tool can centralize operational data and use AI to summarize issues, detect bottlenecks, and support better decision-making.

## Tech Stack

- C# / .NET 8 Web API
- Entity Framework Core
- PostgreSQL or SQL Server
- React
- TypeScript
- Tailwind CSS
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

## Project Goal

This portfolio project demonstrates full-stack development, business systems design, API development, database modeling, dashboard UI design, and practical AI application development.

## Running the Backend API

From the project root:

```bash
cd backend/BusinessOps.Api
dotnet run

## Database Setup

This project uses Entity Framework Core for relational database access.

### Install EF Tool

```bash
dotnet tool install --global dotnet-ef

## Business Data API Endpoints

The backend exposes CRUD endpoints for core business records.

### Products

```http
GET /api/products
GET /api/products/{id}
POST /api/products
PUT /api/products/{id}
DELETE /api/products/{id}

GET /api/orders
GET /api/orders/{id}
POST /api/orders
PUT /api/orders/{id}
DELETE /api/orders/{id}

GET /api/shipments
GET /api/shipments/{id}
POST /api/shipments
PUT /api/shipments/{id}
DELETE /api/shipments/{id}

GET /api/support-issues
GET /api/support-issues/{id}
POST /api/support-issues
PUT /api/support-issues/{id}
DELETE /api/support-issues/{id}

http://localhost:5074/swagger