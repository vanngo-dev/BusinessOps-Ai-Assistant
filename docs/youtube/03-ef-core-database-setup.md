# Phase 3 — EF Core Database Setup

## Video Title

Connecting BusinessOps AI Assistant to a Real Database with EF Core

## Goal

In this phase, the backend is connected to a relational database using Entity Framework Core.

## What Was Built

- Installed EF Core packages
- Created AppDbContext
- Configured database connection string
- Registered DbContext in Program.cs
- Created initial database migration
- Applied migration to create database tables
- Added sample seed data

## Why EF Core?

Entity Framework Core lets the application work with relational database tables through C# classes.

This is useful because the app already has business models:

- Product
- Order
- OrderItem
- Shipment
- SupportIssue

EF Core maps these models to database tables.

## Why Relational Data?

Business operations data is naturally relational.

Examples:

- One order has many order items
- One product can appear in many orders
- One order can have many shipments
- One order can have multiple support issues

A relational database makes these connections easy to store and query.

## Why Seed Data Matters

Seed data makes the app demo-ready before users manually enter records.

The sample data includes realistic business problems:

- Low-stock products
- Delayed shipment
- Missing tracking number
- High-priority support issue
- Unresolved support issue
- Order past required ship date

This gives the future dashboard and AI assistant meaningful data to analyze.

## Commands Used

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run