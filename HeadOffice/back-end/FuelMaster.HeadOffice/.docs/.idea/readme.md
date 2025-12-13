# FuelMaster HeadOffice - Business Overview

## Platform Type

**Business-to-Business (B2B) Software as a Service (SaaS) Platform**

FuelMaster HeadOffice is a comprehensive B2B platform designed specifically for **station management**. The system serves as a centralized management solution for companies operating fuel stations and related businesses.

## Target Users

- **Companies**: Organizations that own and operate multiple fuel stations
- **Managers**: Station managers and administrative personnel responsible for overseeing operations
- **Admins**: System administrators who need comprehensive oversight and reporting capabilities

## Core Purpose

The platform enables companies and managers to **manage everything in their company** through a unified, centralized system. It provides complete visibility and control over all aspects of station operations across multiple locations.

## Key Features & Capabilities

### 1. Multi-Location Station Management

- **City-Based Organization**: Manage stations across different cities
- **Centralized Control**: Oversee all stations from a single platform
- **Location Tracking**: Track and manage stations by geographic location

### 2. Infrastructure Management

- **Tanks**: Monitor and manage fuel storage tanks
  - Track capacity, limits, current levels, and volumes
  - Sensor integration for real-time monitoring
- **Pumps & Nozzles**: Manage fuel dispensing equipment
  - Configure pumps and nozzles per station
  - Track usage, volume, and performance metrics

### 3. Human Resources Management

- **Employee Management**:
  - Manage employee records and assignments
  - Track employee activities and responsibilities
  - Assign employees to specific stations

### 4. Transaction Management

- **Transaction Tracking**:
  - Monitor all transactions across stations
  - Track sales, payments, and financial activities
  - Maintain transaction history and records

### 5. Delivery Management

- **Delivery Operations**:
  - Manage fuel deliveries to stations
  - Track delivery schedules and quantities
  - Monitor delivery status and history

### 6. Pricing Management

- **Zone Pricing System**:
  - Set and manage prices by geographic zones
  - Easily adjust pricing across multiple stations
  - Maintain pricing history and track changes
  - Flexible pricing strategies per zone

### 7. Real-Time Monitoring & Tracking

- **Live Data**:
  - Real-time tracking of station operations
  - Monitor tank levels, sales, and activities as they happen
  - Instant visibility into system status

### 8. Reporting & Analytics

- **Comprehensive Reporting**:
  - Generate detailed reports on all aspects of operations
  - Historical data analysis
  - Customizable report generation
- **Real-Time Reports**:
  - Live dashboards with current metrics
  - Real-time analytics and insights
  - Instant access to operational data

## Business Value Proposition

### For Companies

- **Centralized Management**: Control all stations from one platform
- **Operational Efficiency**: Streamline operations across multiple locations
- **Data-Driven Decisions**: Access comprehensive reports and analytics
- **Cost Management**: Optimize pricing and operations through zone-based pricing
- **Compliance & Tracking**: Maintain complete records of all operations

### For Managers

- **Complete Visibility**: See everything happening across all stations
- **Easy Management**: Simple interface to manage complex operations
- **Real-Time Insights**: Make informed decisions with live data
- **Efficient Workflow**: Manage employees, deliveries, and transactions seamlessly

### For Admins

- **Full Control**: Comprehensive administrative capabilities
- **Advanced Reporting**: Generate and analyze detailed reports
- **System Oversight**: Monitor all aspects of the platform
- **Real-Time Monitoring**: Track system performance and operations live

## System Architecture

The platform is designed as a **multi-tenant B2B SaaS solution**, allowing:

- Multiple companies to use the platform independently
- Data isolation between different companies
- Scalable architecture to support growing businesses
- Secure access control and permissions management

## Technology Stack

- **Backend**: .NET/C# (HeadOffice application)
- **Database**: Entity Framework Core with SQL Server
- **Architecture**: Clean Architecture with Repository Pattern
- **Caching**: In-memory caching for performance optimization
- **API**: RESTful API design for service integration

## Use Cases

1. **Station Owner**: Manage 50+ stations across multiple cities, set zone prices, monitor real-time operations, generate reports
2. **Station Manager**: Oversee assigned stations, manage employees, track deliveries, monitor transactions
3. **Admin User**: Create reports, analyze company-wide data, configure system settings, monitor all operations

## Future Vision

The platform is designed to be the **complete solution** for fuel station management, providing:

- End-to-end operational management
- Comprehensive reporting and analytics
- Real-time monitoring and alerts
- Scalable infrastructure for business growth
- Integration capabilities with external systems

---

## High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                        Client Applications                        │
│  (Web Frontend, Mobile Apps, Third-party Integrations)            │
└────────────────────────────┬──────────────────────────────────────┘
                             │
                             │ HTTP/REST API
                             │ JWT Authentication
                             │
┌────────────────────────────▼──────────────────────────────────────┐
│                    FuelMaster.HeadOffice                           │
│                    (Presentation Layer)                            │
├───────────────────────────────────────────────────────────────────┤
│  • Controllers (API Endpoints)                                   │
│  • Middleware (Tenant, Authentication, Error Handling)           │
│  • Extensions (Dependency Injection, Middleware Registration)     │
│  • Program.cs (Application Bootstrap)                             │
└────────────────────────────┬──────────────────────────────────────┘
                             │
                             │ Service Calls
                             │
┌────────────────────────────▼──────────────────────────────────────┐
│              FuelMaster.HeadOffice.Application                     │
│                  (Application Layer)                              │
├───────────────────────────────────────────────────────────────────┤
│  • Business Services (CityService, StationService, TankService)  │
│  • Authentication Services (UserService, TokenService)            │
│  • Caching Services (EntityCache, CacheService)                   │
│  • Domain Services (PricingService, ReportService)               │
│  • DTOs (Data Transfer Objects)                                  │
│  • Helpers (Result, Pagination)                                  │
│  • Extensions (Pagination Extension)                             │
│  • Mappers (AutoMapper Configuration)                             │
└────────────────────────────┬──────────────────────────────────────┘
                             │
                             │ Repository Calls
                             │
┌────────────────────────────▼──────────────────────────────────────┐
│              FuelMaster.HeadOffice.Core                            │
│                  (Domain Layer)                                    │
├───────────────────────────────────────────────────────────────────┤
│  • Entities (Domain Models: Station, Tank, Nozzle, etc.)         │
│  • Interfaces (Repository, Service Contracts)                     │
│  • Models (DTOs, Requests, Responses)                            │
│  • Enums (NozzleStatus, PaymentMethod)                            │
│  • Helpers (DateTimeCulture, Localization)                         │
│  • Configurations (Authorization, Cache, Pagination)             │
│  • Repositories/Interfaces (Repository Contracts)                 │
└────────────────────────────┬──────────────────────────────────────┘
                             │
                             │ Data Access
                             │
┌────────────────────────────▼──────────────────────────────────────┐
│          FuelMaster.HeadOffice.Infrastructure                     │
│              (Infrastructure Layer)                                │
├───────────────────────────────────────────────────────────────────┤
│  • DbContext (FuelMasterDbContext)                               │
│  • Repositories (Data Access Implementations)                     │
│  • Migrations (Entity Framework Migrations)                       │
│  • Seeders (Data Seeding Services)                               │
│  • Services (ContextFactory, MigrationService, CacheService)     │
│  • Domain Events (Event Handlers)                                 │
│  • Unit of Work (Transaction Management)                          │
└────────────────────────────┬──────────────────────────────────────┘
                             │
                             │ Database Queries
                             │
┌────────────────────────────▼──────────────────────────────────────┐
│                    SQL Server Databases                            │
│  (Multi-Tenant: Database-per-Tenant Architecture)                │
├───────────────────────────────────────────────────────────────────┤
│  • Base Database (FuelMasterBasedDB)                             │
│  • Tenant Databases (ForYouDB, QservDB, etc.)                    │
│  • Each tenant has isolated database                              │
└───────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────┐
│                    Supporting Services                            │
├───────────────────────────────────────────────────────────────────┤
│  • In-Memory Caching (EntityCache<T>)                            │
│  • Logging (Serilog)                                             │
│  • Authentication (JWT Bearer Tokens)                            │
│  • AutoMapper (Object Mapping)                                   │
└───────────────────────────────────────────────────────────────────┘
```

### Architecture Layers Explained

1. **Presentation Layer** (`FuelMaster.HeadOffice`)

   - Handles HTTP requests/responses
   - API controllers and endpoints
   - Middleware for tenant resolution, authentication, error handling
   - Entry point of the application

2. **Application Layer** (`FuelMaster.HeadOffice.Application`)

   - Business logic orchestration
   - Service implementations for business operations
   - Cross-cutting concerns (authentication, caching)
   - Domain-specific services (pricing, reports)
   - DTOs for data transfer
   - Application-specific helpers and extensions
   - Mapping configurations

3. **Domain Layer** (`FuelMaster.HeadOffice.Core`)

   - Core business entities and domain models
   - Business rules and validations
   - Interface definitions (contracts)
   - Domain events and exceptions

4. **Infrastructure Layer** (`FuelMaster.HeadOffice.Infrastructure`)

   - Data access implementations
   - Database context and configurations
   - External service integrations
   - Technical implementations

5. **Database Layer**
   - Multi-tenant database architecture
   - Each tenant has isolated database
   - Entity Framework Core for ORM

---

## How to Run Locally

### Prerequisites

- **.NET SDK 8.0** or later
- **SQL Server** (SQL Server Express or full SQL Server)
- **Visual Studio 2022** or **Visual Studio Code** (optional, for IDE)
- **Git** (for cloning the repository)

### Step 1: Clone the Repository

```bash
git clone <repository-url>
cd FuelMaster.HeadOffice
```

### Step 2: Configure Database Connection

1. Open `FuelMaster.HeadOffice/appsettings.json`
2. Update the connection strings:

```json
{
  "ConnectionStrings": {
    "Default": "Server=.\\SqlExpress; Database=FuelMasterBasedDB; integrated security=true; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True;"
  },
  "MultiTenantSettings": {
    "Tenants": [
      {
        "TenantId": "foryou",
        "ConnectionString": "Server=.\\SqlExpress; Database=ForYouDB; integrated security=true; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True;"
      },
      {
        "TenantId": "qserv",
        "ConnectionString": "Server=.\\SqlExpress; Database=QservDB; integrated security=true; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True;"
      }
    ]
  }
}
```

**Note**: Adjust the `Server` parameter based on your SQL Server instance:

- Local SQL Server Express: `Server=.\\SqlExpress`
- Local SQL Server: `Server=localhost` or `Server=(localdb)\\mssqllocaldb`
- Remote SQL Server: `Server=your-server-name,1433`

### Step 3: Restore NuGet Packages

```bash
dotnet restore
```

### Step 4: Build the Solution

```bash
dotnet build
```

### Step 5: Run Database Migrations

The application automatically applies migrations on startup. However, you can also run them manually:

```bash
# Navigate to the Infrastructure project
cd FuelMaster.HeadOffice.Infrastructure

# Create a migration (if needed)
dotnet ef migrations add MigrationName --startup-project ../FuelMaster.HeadOffice

# Update database
dotnet ef database update --startup-project ../FuelMaster.HeadOffice
```

### Step 6: Run the Application

#### Option 1: Using .NET CLI

```bash
# Navigate to the main project
cd FuelMaster.HeadOffice

# Run the application
dotnet run
```

#### Option 2: Using Visual Studio

1. Open `FuelMaster.HeadOffice.sln` in Visual Studio
2. Set `FuelMaster.HeadOffice` as the startup project
3. Press `F5` or click "Run"

#### Option 3: Using Visual Studio Code

1. Open the project folder in VS Code
2. Press `F5` or use the terminal: `dotnet run --project FuelMaster.HeadOffice`

### Step 7: Access the Application

Once running, the application will be available at:

- **HTTP**: `http://localhost:5295`
- **HTTPS**: `https://localhost:7286`
- **Swagger UI**: `http://localhost:5295/swagger` or `https://localhost:7286/swagger`

### Configuration Options

#### Development Environment

The application uses `appsettings.Development.json` for development-specific settings. You can override default settings here.

#### Environment Variables

You can also configure the application using environment variables:

```bash
# Set environment
export ASPNETCORE_ENVIRONMENT=Development

# Or on Windows PowerShell
$env:ASPNETCORE_ENVIRONMENT="Development"
```

### Troubleshooting

#### Database Connection Issues

- Ensure SQL Server is running
- Verify connection string matches your SQL Server instance
- Check that databases exist or can be created
- Ensure SQL Server authentication allows your Windows account

#### Port Already in Use

If port 5295 or 7286 is already in use, you can change it in `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "http://localhost:5000"
}
```

#### Migration Errors

- Ensure Entity Framework Core tools are installed: `dotnet tool install --global dotnet-ef`
- Check that connection strings are correct
- Verify database permissions

### Running with Docker (Optional)

If you prefer using Docker for SQL Server:

```bash
# Run SQL Server in Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# Update connection string to use Docker instance
# Server=localhost,1433; Database=...; User Id=sa; Password=YourPassword123;
```

---

## Folder Structure

```
FuelMaster.HeadOffice/
│
├── FuelMaster.HeadOffice/                    # Presentation Layer (API)
│   ├── Controllers/                          # API Controllers
│   │   ├── AccountController.cs
│   │   ├── Cities/                           # City-related endpoints
│   │   ├── Stations/                         # Station-related endpoints
│   │   ├── Tanks/                            # Tank-related endpoints
│   │   ├── Zones/                            # Zone-related endpoints
│   │   └── ...
│   ├── Extensions/                           # Extension methods
│   │   ├── Dependencies/                     # Dependency injection setup
│   │   └── Middlewares/                     # Custom middleware
│   ├── Helpers/                              # Helper classes
│   │   └── FuelMasterController.cs           # Base controller
│   ├── Properties/                           # Project properties
│   │   └── launchSettings.json               # Launch configurations
│   ├── wwwroot/                              # Static files
│   │   └── Procedures/                       # SQL stored procedures
│   ├── Program.cs                            # Application entry point
│   ├── appsettings.json                      # Application configuration
│   └── appsettings.Development.json          # Development configuration
│
├── FuelMaster.HeadOffice.Application/         # Application Layer
│   ├── DTOs/                                 # Data Transfer Objects
│   │   ├── PaginationDto.cs
│   │   └── ResultDto.cs
│   ├── Extensions/                            # Extension methods
│   │   └── PaginationExtension.cs
│   ├── Helpers/                              # Helper classes
│   │   └── Result.cs                         # Result helper
│   ├── Authentication/                       # Authentication services
│   │   ├── UserService.cs
│   │   ├── TokenService.cs
│   │   └── ...
│   ├── Caching/                              # Caching services
│   │   └── EntityCache.cs                    # Generic entity cache
│   ├── Mappers/                              # AutoMapper configurations
│   │   └── FuelMasterMapper.cs
│   └── Services/                             # Business services
│       ├── Implementations/
│       │   └── Business/
│       │       ├── CityService/              # City business logic
│       │       ├── StationService/           # Station business logic
│       │       ├── TankService/             # Tank business logic
│       │       ├── ZoneService/              # Zone business logic
│       │       ├── PricingService.cs         # Pricing service
│       │       ├── ReportService.cs          # Report service
│       │       └── ...
│       └── Interfaces/
│           └── Business/                     # Business service interfaces
│
├── FuelMaster.HeadOffice.Core/                 # Domain Layer
│   ├── Configurations/                       # Configuration classes
│   │   ├── AuthorizationConfiguration.cs
│   │   ├── CacheConfiguration.cs
│   │   ├── PaginationConfiguration.cs
│   │   └── TenantConfiguration.cs
│   ├── Constant/                             # Constants
│   │   └── ConfigKeys.cs
│   ├── Entities/                             # Domain entities
│   │   ├── Base/                             # Base entity classes
│   │   ├── Configs/                          # Configuration entities
│   │   │   ├── City.cs
│   │   │   ├── FuelTypes/
│   │   │   ├── Nozzles/
│   │   │   ├── Stations/
│   │   │   ├── Tanks/
│   │   │   └── Pump.cs
│   │   ├── Employees/                        # Employee entities
│   │   ├── Groups & Permissions/             # Permission entities
│   │   ├── Transactions/                     # Transaction entities
│   │   └── Zones/                            # Zone entities
│   ├── Enums/                                # Enumerations
│   │   ├── NozzleStatus.cs
│   │   └── PaymentMethod.cs
│   ├── Helpers/                              # Helper classes
│   │   ├── DateTimeCulture.cs
│   │   └── LocalizationUtilities.cs
│   ├── Interfaces/                           # Interface definitions
│   │   ├── Authentication/                  # Auth interfaces
│   │   ├── Database/                         # Database interfaces
│   │   ├── EntityServices/                   # Entity service interfaces
│   │   ├── Markers/                          # Marker interfaces
│   │   └── Repositories/                     # Repository interfaces
│   ├── Models/                               # DTOs and models
│   │   ├── Dtos/                            # Data transfer objects
│   │   ├── Requests/                        # Request models
│   │   └── Responses/                       # Response models
│   ├── Repositories/                        # Repository interfaces
│   │   └── Interfaces/
│   │       ├── ICityRepository.cs
│   │       ├── ITankRepository.cs
│   │       └── ...
│   └── Resources/                           # Localization resources
│       ├── Resource.resx
│       └── Resource.ar.resx
│
├── FuelMaster.HeadOffice.Infrastructure/      # Infrastructure Layer
│   ├── Contexts/                            # Database contexts
│   │   ├── Configurations/                  # EF Core configurations
│   │   │   ├── StationConfiguration.cs
│   │   │   ├── TankConfiguration.cs
│   │   │   └── ...
│   │   ├── Seeders/                         # Data seeders
│   │   │   ├── TestSeeder.cs
│   │   │   └── UserSeeder.cs
│   │   ├── FuelMasterDbContext.cs          # Main DbContext
│   │   └── TenantDbContextFactory.cs       # Tenant context factory
│   ├── DomainEvents/                        # Domain event handlers
│   │   └── Handlers/
│   ├── Extensions/                          # Extension methods
│   ├── Migrations/                          # EF Core migrations
│   │   ├── 20251111084737_InitialCreate.cs
│   │   └── ...
│   ├── Repositories/                        # Repository implementations
│   │   ├── CityRepository.cs
│   │   ├── TankRepository.cs
│   │   ├── Stations/
│   │   └── Zones/
│   ├── Services/                            # Infrastructure services
│   │   ├── ContextFactoryService.cs
│   │   ├── MigrationService.cs
│   │   └── Implementations/
│   │       └── CacheService.cs
│   └── UnitOfWorks/                        # Unit of Work pattern
│       └── UnitOfWork.cs
│
├── .docs/                                    # Documentation
│   ├── idea/
│   │   └── Readme.md                         # This file
│   ├── repository/
│   ├── services/
│   └── others/
│
├── .prompts/                                 # AI prompts for development
│   ├── repository/
│   └── services/
│
└── FuelMaster.HeadOffice.sln                 # Solution file
```

### Key Directories Explained

#### Presentation Layer (`FuelMaster.HeadOffice`)

- **Controllers/**: REST API endpoints organized by domain
- **Extensions/**: Dependency injection and middleware setup
- **wwwroot/**: Static files and SQL procedures

#### Application Layer (`FuelMaster.HeadOffice.Application`)

- **Services/**: Business logic implementations
- **Authentication/**: User authentication and authorization
- **Caching/**: Entity caching services
- **Mappers/**: AutoMapper configurations
- **DTOs/**: Data transfer objects for API communication
- **Helpers/**: Application-specific helper classes

#### Domain Layer (`FuelMaster.HeadOffice.Core`)

- **Entities/**: Domain models and business entities
- **Interfaces/**: Contracts and abstractions
- **Models/**: DTOs, requests, and responses
- **Repositories/Interfaces/**: Repository contracts

#### Infrastructure Layer (`FuelMaster.HeadOffice.Infrastructure`)

- **Contexts/**: Database context and configurations
- **Repositories/**: Data access implementations
- **Migrations/**: Database schema migrations
- **Services/**: Technical service implementations

---

**Note**: This platform serves as the central nervous system for fuel station operations, enabling companies to efficiently manage, monitor, and optimize their entire station network from a single, powerful platform.
