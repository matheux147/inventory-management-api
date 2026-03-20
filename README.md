# Inventory Management API

A RESTful API built with .NET 10 for managing an inventory system including categories, suppliers, and products. This project demonstrates clean code, good design, and testing practices while handling product lifecycle management and external service integrations.

## Architecture & Patterns

This application follows **Hexagonal Architecture (Ports and Adapters)** principles to ensure separation of concerns and maintainability. It's structured into:

- **Core**: Contains the central business rules, `Domain` entities, and `Application` use cases. It has no dependencies on external frameworks.
- **Primary Adapters**: The entry points to the application, containing the ASP.NET Core REST API.
- **Secondary Adapters**: Implementations of external concerns such as Database Persistence, and integrations with external systems (WMS, Audit Log, Email).
- **Hybrid Distributed Caches**: Used for caching data across the application to improve performance and reduce load on external systems.
- **Unit Tests**: Focused on testing the core business logic in isolation, without dependencies on external systems.

**Key Patterns Used:**
- CQRS (Command and Query Responsibility Segregation) / Mediator Pattern
- Repository Pattern for data access abstraction
- Result Pattern for robust error handling
- Repository Pattern (for data persistence)
- Distributed Caching for performance optimization

## Business Rules Overview

The system manages three main entities:

### Categories & Suppliers
- **Categories** can be created, listed, and deleted. They support hierarchical structures (can have a parent category).
- **Suppliers** can be created and listed, containing basic info including currency and country.

### Products & Lifecycle
- Products are linked to a **Category** and a **Supplier**, holding financial information like acquisition cost in USD and supplier currency.
- Products have a strict lifecycle status: `Created`, `Sold`, `Cancelled`, or `Returned`.
- **Constraints**: 
  - Products cannot be deleted from the system entirely.
  - Cancelled and returned products cannot be transitioned to a `Sold` state.
  - Only sold products can be cancelled or returned.
- **External Integrations**:
  - **On Creation**: Triggers a product creation in a mock Warehouse Management System (WMS) and logs the action in an external Audit System.
  - **On Sold**: Dispatches the product via WMS, sends an email notification to the supplier, and logs the action in the Audit System.

## Tech Stack
- **Framework**: .NET 10
- **Database**: SQL Server (via Docker)
- **Cache**: Cache + MemoryCache
- **Environment**: Distributed via Docker and Docker Compose

## Running the Application

The application is fully containerized. To start the API and and its dependencies, simply run:

```bash
docker-compose up --build -d
```
