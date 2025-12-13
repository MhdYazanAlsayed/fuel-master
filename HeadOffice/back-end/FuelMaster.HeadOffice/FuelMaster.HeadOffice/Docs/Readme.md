# FuelMaster HeadOffice - Documentation

## Table of Contents

1. [Overview](#overview)
2. [System Architecture](#system-architecture)
3. [Multi-Tenancy](#multi-tenancy)
4. [Core Business Modules](#core-business-modules)
5. [API Documentation](#api-documentation)
6. [Setup & Installation](#setup--installation)
7. [Configuration](#configuration)
8. [Development Guidelines](#development-guidelines)

---

## Overview

**FuelMaster HeadOffice** is a comprehensive fuel management system designed to manage multiple fuel stations, their operations, and associated business processes. The system supports multi-tenancy, allowing multiple organizations to operate independently within the same application instance.

### Key Features

- ✅ **Multi-Tenant Architecture**: Support for multiple independent organizations
- ✅ **Station Management**: Complete lifecycle management of fuel stations
- ✅ **Tank Management**: Monitor and manage fuel storage tanks
- ✅ **Nozzle Management**: Control and track fuel dispensing nozzles
- ✅ **Transaction Management**: Record and track all fuel transactions
- ✅ **Delivery Management**: Manage fuel deliveries to stations
- ✅ **User & Permission Management**: Role-based access control
- ✅ **Reporting**: Comprehensive reporting and analytics
- ✅ **Real-time Updates**: SignalR integration for live updates

---

## System Architecture

### Technology Stack

- **Framework**: ASP.NET Core
- **Database**: SQL Server (with Entity Framework Core)
- **Authentication**: JWT Bearer Token
- **Logging**: Serilog
- **Real-time Communication**: SignalR
- **Caching**: In-memory caching
- **Validation**: FluentValidation

### Project Structure

```
FuelMaster.HeadOffice/
├── Controllers/          # API Controllers
├── Extensions/           # Dependency Injection & Middleware
├── wwwroot/             # Static files & SQL procedures
│
FuelMaster.HeadOffice.ApplicationService/
├── Authentication/       # User authentication & authorization
├── Repositories/         # Business logic & data access
├── Services/            # Domain services
├── Database/            # Migration & seeding services
│
FuelMaster.HeadOffice.Core/
├── Contracts/           # Interfaces & contracts
├── Entities/            # Domain entities
├── Models/              # DTOs, Requests, Responses
├── Helpers/             # Utility classes
├── Configurations/      # Configuration classes
├── Constants/           # Application constants
├── Enums/              # Enumerations
│
FuelMaster.HeadOffice.Infrastructure/
├── Contexts/            # DbContext implementations
├── Migrations/          # Database migrations
├── Seeders/            # Data seeders
└── DomainEvents/       # Domain event handlers
```

---

## Multi-Tenancy

### Overview

The system implements a **database-per-tenant** multi-tenancy model, where each tenant has its own isolated database. This ensures complete data separation and security between different organizations.

### How It Works

#### Tenant Identification

1. **Tenant ID Extraction**: The `TenantMiddleware` extracts the tenant ID from:
   - HTTP headers (e.g., `X-Tenant-Id`)
   - JWT claims (`tenant_id`)
2. **Validation**: The middleware validates:

   - Tenant ID exists in configuration
   - User has access to the requested tenant (via JWT claims)

3. **Context Setting**: Valid tenant ID is stored in `HttpContext.Items` for use throughout the request lifecycle

#### Database Context

- Each tenant has a dedicated database connection string
- The `ContextFactoryService` creates the appropriate `FuelMasterDbContext` based on the tenant ID
- All queries are automatically scoped to the current tenant's database

#### Configuration

Tenants are configured in `appsettings.json`:

```json
{
  "MultiTenantSettings": {
    "Tenants": [
      {
        "TenantId": "tenant1",
        "ConnectionString": "Server=...;Database=Tenant1DB;..."
      },
      {
        "TenantId": "tenant2",
        "ConnectionString": "Server=...;Database=Tenant2DB;..."
      }
    ]
  }
}
```

### Migration Management

- Migrations are applied to all tenant databases on application startup
- The `MigrationService` handles applying migrations across all configured tenants
- Each tenant database maintains its own migration history

---

## Core Business Modules

### 1. Station Management

#### Overview

Stations represent physical fuel stations managed within the system. Each station belongs to a tenant and can have multiple tanks, pumps, and nozzles.

#### Key Concepts

- **Station**: A physical location where fuel is sold
- **Hierarchy**: Stations → Tanks → Pumps → Nozzles
- **Relationships**: Connected to zones, cities, employees

#### Key Features

- Create, read, update, and delete stations
- Paginated listing with filtering
- Station details and relationships
- Zone and city associations
- Employee assignments

#### API Endpoints

- `GET /api/stations` - Get all stations
- `GET /api/stations/pagination?page={page}` - Get paginated stations
- `GET /api/stations/{id}` - Get station details
- `POST /api/stations` - Create new station
- `PUT /api/stations/{id}` - Update station
- `DELETE /api/stations/{id}` - Delete station

#### Data Models

- `CreateStationDto`: Station creation request
- `EditStationDto`: Station update request
- `StationResult`: Station response model

---

### 2. Tank Management

#### Overview

Tanks store fuel at stations. Each tank is associated with a specific fuel type and station.

#### Key Concepts

- **Tank**: Fuel storage container
- **Fuel Type**: Type of fuel stored (e.g., Gasoline 95, Diesel)
- **Capacity**: Maximum storage capacity
- **Current Level**: Current fuel level in the tank

#### Key Features

- Create, read, update, and delete tanks
- Filter tanks by station, fuel type
- Paginated listing
- Tank level monitoring
- Transaction history

#### API Endpoints

- `GET /api/tanks` - Get all tanks (with optional filters)
- `GET /api/tanks/pagination?page={page}` - Get paginated tanks
- `GET /api/tanks/{id}` - Get tank details
- `POST /api/tanks` - Create new tank
- `PUT /api/tanks/{id}` - Update tank
- `DELETE /api/tanks/{id}` - Delete tank

#### Data Models

- `CreateTankDto`: Tank creation request
- `EditTankDto`: Tank update request
- `GetTankRequest`: Tank filtering request
- `TankResult`: Tank response model

---

### 3. Nozzle Management

#### Overview

Nozzles are the physical dispensing points where customers receive fuel. Each nozzle is connected to a pump, which draws from a tank.

#### Key Concepts

- **Nozzle**: Fuel dispensing point
- **Pump**: Mechanical device that pumps fuel from tank to nozzle
- **Status**: Operational status (Active, Inactive, Maintenance)
- **Transaction Tracking**: Each nozzle can record multiple transactions

#### Key Features

- Create, read, update, and delete nozzles
- Nozzle status management
- Association with pumps and tanks
- Transaction recording
- Status monitoring

#### API Endpoints

- `GET /api/nozzles` - Get all nozzles
- `GET /api/nozzles/{id}` - Get nozzle details
- `POST /api/nozzles` - Create new nozzle
- `PUT /api/nozzles/{id}` - Update nozzle
- `DELETE /api/nozzles/{id}` - Delete nozzle

#### Data Models

- `NozzleDto`: Nozzle data transfer object
- `NozzleStatus`: Enumeration of nozzle statuses

---

### 4. Pump Management

#### Overview

Pumps are mechanical devices that transfer fuel from tanks to nozzles. Each pump can be connected to multiple nozzles.

#### Key Concepts

- **Pump**: Fuel transfer mechanism
- **Tank Connection**: Pump draws fuel from associated tank
- **Nozzle Association**: One pump can serve multiple nozzles

#### Key Features

- Create, read, update, and delete pumps
- Pump-tank associations
- Pump-nozzle relationships
- Status management

#### API Endpoints

- `GET /api/pumps` - Get all pumps
- `GET /api/pumps/{id}` - Get pump details
- `POST /api/pumps` - Create new pump
- `PUT /api/pumps/{id}` - Update pump
- `DELETE /api/pumps/{id}` - Delete pump

---

### 5. Transaction Management

#### Overview

Transactions record all fuel sales and movements within the system. This includes customer sales, internal transfers, and adjustments.

#### Key Concepts

- **Transaction**: Record of fuel movement or sale
- **Payment Method**: Cash, Card, Credit, etc.
- **Transaction Type**: Sale, Delivery, Adjustment, etc.
- **Volume & Price**: Amount of fuel and price per unit

#### Key Features

- Record new transactions
- Transaction history and filtering
- Sales reporting
- Payment method tracking
- Volume and revenue calculations

#### API Endpoints

- `GET /api/transactions` - Get all transactions
- `GET /api/transactions/{id}` - Get transaction details
- `POST /api/transactions` - Create new transaction
- `GET /api/transactions/filter` - Filter transactions

#### Data Models

- `TransactionDto`: Transaction data
- `PaymentMethod`: Enumeration of payment methods

---

### 6. Delivery Management

#### Overview

Deliveries track fuel shipments from suppliers to stations, updating tank levels upon completion.

#### Key Concepts

- **Delivery**: Fuel shipment to a station
- **Supplier**: Fuel provider
- **Tank Update**: Delivery increases tank fuel level
- **Delivery Status**: Pending, In Transit, Completed, Cancelled

#### Key Features

- Create, read, update, and delete deliveries
- Delivery tracking
- Automatic tank level updates
- Delivery history
- Paginated listing

#### API Endpoints

- `GET /api/deliveries` - Get all deliveries
- `GET /api/deliveries/pagination` - Get paginated deliveries
- `GET /api/deliveries/{id}` - Get delivery details
- `POST /api/deliveries` - Create new delivery
- `DELETE /api/deliveries/{id}` - Delete delivery

#### Data Models

- `DeliveryDto`: Delivery data transfer object
- `GetDeliveriesPaginationDto`: Pagination parameters

---

### 7. User & Permission Management

#### Overview

The system uses role-based access control (RBAC) to manage user permissions. Users belong to groups, and groups have assigned permissions.

#### Key Concepts

- **User**: System user account
- **Employee**: Employee entity linked to user
- **Group**: Collection of permissions (role)
- **Permission**: Specific action or resource access right

#### Key Features

- User registration and authentication
- Group (role) management
- Permission assignment
- User-employee linking
- JWT token generation

#### API Endpoints

- `POST /api/account/register` - Register new user
- `POST /api/account/login` - User login
- `GET /api/groups` - Get all groups
- `GET /api/permissions` - Get all permissions
- `POST /api/groups` - Create group
- `PUT /api/groups/{id}` - Update group permissions

---

### 8. Employee Management

#### Overview

Employees represent staff members who work at stations. They can be linked to user accounts for system access.

#### Key Concepts

- **Employee**: Staff member record
- **Station Assignment**: Employee belongs to one or more stations
- **User Link**: Optional connection to system user account

#### Key Features

- Create, read, update, and delete employees
- Employee-station associations
- Employee-user account linking
- Employee listing and filtering

#### API Endpoints

- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee details
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

---

### 9. Fuel Type Management

#### Overview

Fuel types define the different types of fuel managed in the system (e.g., Gasoline 95, Diesel, Premium).

#### Key Concepts

- **Fuel Type**: Category of fuel
- **Price**: Base price per unit
- **Tank Association**: Each tank stores one fuel type

#### Key Features

- Create, read, update fuel types
- Fuel type listing
- Price management (via zone prices)

#### API Endpoints

- `GET /api/fuel-types` - Get all fuel types
- `POST /api/fuel-types` - Create new fuel type
- `PUT /api/fuel-types/{id}` - Update fuel type

#### Data Models

- `FuelTypeDto`: Fuel type data
- `FuelTypeResult`: Fuel type response model

---

### 10. Zone & Pricing Management

#### Overview

Zones represent geographical areas with specific pricing for fuel types. Pricing can change over time, maintaining a price history.

#### Key Concepts

- **Zone**: Geographical pricing region
- **Zone Price**: Price for a fuel type in a zone
- **Price History**: Historical record of price changes
- **City Association**: Cities belong to zones

#### Key Features

- Zone creation and management
- Zone price setting
- Price history tracking
- Zone-city relationships

#### API Endpoints

- `GET /api/zones` - Get all zones
- `POST /api/zones` - Create new zone
- `PUT /api/zones/{id}` - Update zone
- `GET /api/zones/prices` - Get zone prices
- `POST /api/zones/prices` - Set zone price
- `GET /api/zones/prices/history` - Get price history

---

### 11. City Management

#### Overview

Cities represent geographical locations where stations operate. Cities are associated with zones for pricing purposes.

#### Key Concepts

- **City**: Geographical location
- **Zone Association**: City belongs to a zone
- **Station Location**: Stations are located in cities

#### Key Features

- Create, read, update, delete cities
- City-zone associations
- City listing

#### API Endpoints

- `GET /api/cities` - Get all cities
- `POST /api/cities` - Create new city
- `PUT /api/cities/{id}` - Update city
- `DELETE /api/cities/{id}` - Delete city

---

### 12. Reports & Analytics

#### Overview

The reporting module provides comprehensive analytics and insights into station operations, sales, inventory, and more.

#### Key Concepts

- **Sales Reports**: Revenue and volume analysis
- **Inventory Reports**: Tank level and stock reports
- **Transaction Reports**: Transaction analysis
- **Station Reports**: Station performance metrics

#### Key Features

- Sales reporting by station, date range, fuel type
- Inventory level reports
- Transaction summaries
- Export capabilities
- Real-time dashboard data

#### API Endpoints

- `GET /api/reports/sales` - Sales reports
- `GET /api/reports/inventory` - Inventory reports
- `GET /api/reports/transactions` - Transaction reports
- `GET /api/reports/stations` - Station performance reports

---

## API Documentation

### Base URL

```
http://localhost:5000/api
```

### Authentication

Most endpoints require JWT Bearer token authentication. Include the token in the Authorization header:

```
Authorization: Bearer {your_jwt_token}
```

### Multi-Tenant Headers

Include the tenant ID in request headers:

```
X-Tenant-Id: {tenant_id}
```

### Common Response Formats

#### Success Response

```json
{
  "data": { ... },
  "message": "Operation successful"
}
```

#### Error Response

```json
{
  "error": "Error message",
  "details": { ... }
}
```

#### Paginated Response

```json
{
  "items": [ ... ],
  "totalItems": 100,
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

### Standard HTTP Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## Setup & Installation

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (Express or higher)
- Visual Studio 2022 or VS Code (recommended)

### Installation Steps

1. **Clone the Repository**

   ```bash
   git clone [repository-url]
   cd FuelMaster.HeadOffice
   ```

2. **Configure Database**

   - Update `appsettings.json` with your database connection strings
   - Configure tenant databases in `MultiTenantSettings.Tenants`

3. **Restore Dependencies**

   ```bash
   dotnet restore
   ```

4. **Apply Migrations**

   - Migrations are applied automatically on startup
   - Or manually: `dotnet ef database update`

5. **Run Seeders**

   - Seeders run automatically on startup
   - Initial data is populated for all tenants

6. **Run the Application**
   ```bash
   dotnet run --project FuelMaster.HeadOffice
   ```

### Development Environment Setup

1. Update `appsettings.Development.json` with development settings
2. Configure tenant databases for development
3. Set up logging paths in configuration
4. Configure JWT settings for authentication

---

## Configuration

### Application Settings (`appsettings.json`)

#### Connection Strings

```json
{
  "ConnectionStrings": {
    "Default": "Server=...;Database=FuelMasterBasedDB;..."
  }
}
```

#### Multi-Tenant Settings

```json
{
  "MultiTenantSettings": {
    "Tenants": [
      {
        "TenantId": "tenant1",
        "ConnectionString": "Server=...;Database=Tenant1DB;..."
      }
    ]
  }
}
```

#### Pagination Settings

```json
{
  "PaginationSetting": {
    "Length": 10
  }
}
```

#### Cache Settings

```json
{
  "CacheSettings": {
    "DefaultDurationMinutes": 15,
    "FuelMasterGroupsDurationMinutes": 20,
    "PaginationDurationMinutes": 10,
    "DetailsDurationMinutes": 30
  }
}
```

#### JWT Bearer Settings

```json
{
  "BerearSettings": {
    "Key": "your-secret-key",
    "Audience": "https://www.yoursite.com",
    "Issuer": "https://www.yoursite.com"
  }
}
```

---

## Development Guidelines

### Code Structure

- **Controllers**: Handle HTTP requests/responses
- **Repositories**: Contain business logic and data access
- **Contracts**: Define interfaces for dependency injection
- **Entities**: Domain models with EF Core configurations
- **DTOs**: Data transfer objects for API communication

### Validation

- Use FluentValidation for request validation
- Validators are placed alongside controllers in `Validators` folders
- Validation errors return detailed messages

### Error Handling

- Use try-catch blocks in controllers
- Log errors using ILogger
- Return appropriate HTTP status codes
- Provide meaningful error messages

### Caching Strategy

- Pagination results are cached
- Group permissions are cached
- Details endpoints use longer cache duration
- Cache invalidation on data mutations

### Best Practices

1. **Multi-Tenancy**: Always ensure tenant context is available
2. **Validation**: Validate all user inputs
3. **Logging**: Log important operations and errors
4. **Error Messages**: Use resource files for localization
5. **Pagination**: Use pagination for large datasets
6. **Transactions**: Use database transactions for complex operations

### Adding New Features

1. Define contracts in `Core/Contracts`
2. Create entities in `Core/Entities`
3. Implement repositories in `ApplicationService/Repositories`
4. Create controllers in `Controllers`
5. Add validators for request DTOs
6. Update mapper for entity-DTO conversions
7. Add database migrations if schema changes

---

## Additional Resources

### Database Procedures

SQL procedures are stored in `wwwroot/Procedures/` and can be executed as needed.

### Logging

Logs are written to:

- `Logs/Errors/` - Error logs
- `Logs/Information/` - Information logs

### Real-Time Updates

SignalR hub (`RealTimeHub`) provides real-time updates for:

- Station status changes
- Transaction notifications
- Tank level updates
- System-wide announcements

---

## Support & Contribution

### Reporting Issues

Please report bugs or request features through the issue tracker.

### Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

---

**Last Updated**: [Date]  
**Version**: [Version Number]  
**Maintained By**: [Team/Organization Name]
