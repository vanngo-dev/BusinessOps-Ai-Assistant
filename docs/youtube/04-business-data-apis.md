# Phase 4 — Business Data APIs

## Video Title

Building CRUD APIs for BusinessOps AI Assistant

## Goal

In this phase, we expose the business operations data through REST API endpoints.

## What Was Built

Created CRUD endpoints for:

- Products
- Orders
- Shipments
- Support issues

## Why REST APIs Matter

The React frontend needs a clean way to read and update business data.

REST endpoints give the frontend access to records such as:

- Products
- Orders
- Shipments
- Support issues

These endpoints are also useful for Swagger testing and future integrations.

## Why DTOs Are Used

DTOs, or Data Transfer Objects, help prevent exposing raw database models directly.

They allow the API to control what data is returned and what data is accepted from requests.

This makes the backend safer, cleaner, and easier to change later.

## Endpoints Created

### Products

```text
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