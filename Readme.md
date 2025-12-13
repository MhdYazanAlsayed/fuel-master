# FuelMaster HeadOffice - Documentation

## Table of Contents

1. [Overview](#overview)
2. [System Architecture](#system-architecture)
3. [Multi-Tenancy](#multi-tenancy)
4. [What is HeadOffice](#what-is-headoffice)
5. [User Lifecycle](#user-lifecycle)
6. [Core Business Modules](#core-business-modules)
7. [PTS Controller](#pts-controller)
8. [User Management](#user-management)
9. [Setup & Installation](#setup--installation)
10. [Configuration](#configuration)

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

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│              FuelMaster Website Platform                     │
│  • User Authentication & Account Management                  │
│  • Subscription Management                                   │
│  • Tenant Metadata Storage                                   │
│  • Database Provisioning Orchestration                       │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ REST API
                          │ (Session & Tenant Data)
                          ▼
┌─────────────────────────────────────────────────────────────┐
│              FuelMaster HeadOffice Server                    │
│  • Multi-Tenant Business Logic System                       │
│  • Tenant Database Management                                │
│  • Station, Tank, Pump, Nozzle Management                   │
│  • Transaction & Delivery Management                        │
│  • Employee, User, Role, Permission Management              │
│  • Reporting & Analytics                                     │
│  • PTS Controller Integration                               │
└─────────────────────────────────────────────────────────────┘
```

---

## Multi-Tenancy

### Overview

The system implements a **database-per-tenant** multi-tenancy model, where each tenant has its own isolated database. This ensures complete data separation and security between different organizations. 

### How It Works

#### Tenant Source - Integration with Website Platform

HeadOffice does **not** store tenant configuration statically. Instead, it dynamically retrieves tenant information from the **FuelMaster Website Platform**:

1. **Session-Based Tenant Identification**: 
   - User sessions originate from the Website application
   - The Website platform manages user accounts, subscriptions, and tenant metadata
   - HeadOffice receives tenant information via API calls to the Website platform

2. **Dynamic Tenant Retrieval**:
   - HeadOffice calls the Website platform's API to fetch tenant information
   - Tenant data includes: Tenant ID, Database Name, Connection String, Status (Active/Inactive)
   - Tenant information is cached in memory for performance
   - When tenant is not found in cache or marked as inactive, HeadOffice refreshes the tenant list from the Website platform

3. **Tenant Middleware Processing**:
   - The `TenantMiddleware` extracts tenant ID from JWT token claims
   - Validates tenant exists and is active by querying the Website platform (if not in cache)
   - Sets the tenant context for the current request
   - Routes database queries to the appropriate tenant database

#### Database Context

- Each tenant has a dedicated database created dynamically when the tenant is provisioned
- The connection string is generated and managed by HeadOffice
- All business data (stations, tanks, transactions, employees, etc.) is stored in the tenant's isolated database
- Database queries are automatically scoped to the current tenant's database based on the tenant context

#### Tenant Lifecycle

1. **Creation**: User creates tenant through Website platform → Website requests database creation from HeadOffice → HeadOffice creates isolated database
2. **Activation**: Tenant status is managed in Website platform → HeadOffice syncs status changes
3. **Operations**: All business operations occur within the tenant's isolated database
4. **Deactivation**: Tenant can be suspended or deleted through Website platform → HeadOffice respects status changes

---

## What is HeadOffice

FuelMaster HeadOffice is the **core business logic engine** for the FuelMaster platform. It serves as the central system that manages all fuel station operations, tenant data, and business processes.

### Primary Responsibilities

1. **Tenant Database Management**:
   - Creates and manages isolated databases for each tenant
   - Handles database migrations and schema updates
   - Provides database backup and restore capabilities
   - Monitors database health and status

2. **Business Logic Execution**:
   - Processes all fuel station operations (stations, tanks, pumps, nozzles)
   - Manages transactions, deliveries, and inventory
   - Handles user authentication and authorization within tenant context
   - Executes business rules and validations

3. **Data Storage & Isolation**:
   - Stores all tenant-specific business data in isolated databases
   - Ensures complete data separation between tenants
   - Provides secure access controls per tenant

4. **Integration Hub**:
   - Receives data from PTS Controllers at fuel stations
   - Integrates with Website platform for tenant management
   - Provides real-time updates via SignalR
   - Supports reporting and analytics

### What HeadOffice Does NOT Do

- **User Account Management**: Handled by Website platform
- **Subscription Management**: Managed by Website platform  
- **Tenant Metadata Storage**: Stored in Website platform database
- **Billing & Payments**: Processed by Website platform

HeadOffice focuses exclusively on business data and operations, while the Website platform handles user management, subscriptions, and tenant provisioning.

---

## User Lifecycle

### Overview

The complete user journey from account creation to operating their fuel station management system.

### 1. Account Creation

Users register through the **FuelMaster Website Platform**:
- **Standard Registration**: Email and password using ASP.NET Core Identity
- **Social Authentication**: Google OAuth integration
- Account information is stored in the Website platform database

### 2. Subscription Plan Selection

Before creating a tenant, users must select a subscription plan:
- Available plans range from Free tier to various paid tiers
- Each plan has different features, limits (users, stations, etc.), and pricing
- Free plan available for testing and evaluation
- Plan selection is required before tenant creation

### 3. Tenant Creation

Once a user has an active subscription, they can create a tenant:
- User provides a unique tenant name (e.g., "fuel-star")
- System validates tenant name availability
- Website platform initiates tenant database creation request to HeadOffice
- HeadOffice creates an isolated database for the tenant
- Initial schema is applied, including admin user setup
- Tenant becomes active and operational

### 4. Tenant Management & Operations

After tenant creation, users can:

#### Subscription Tracking
- View current subscription status and plan details
- Monitor subscription expiration dates
- Track billing history and payment status
- Upgrade or downgrade subscription plans

#### Database Management
- **Backup Operations**: 
  - Create on-demand database backups
  - Schedule automated backups (daily, weekly, etc.)
  - View backup history and status
  - Download backup files

- **Database Status Monitoring**:
  - Monitor database connection health
  - View database size and storage usage
  - Track database response times
  - Receive alerts for database issues

- **Backup Scheduling**:
  - Configure automated backup schedules
  - Set backup retention policies
  - Define backup storage locations

#### API Status Monitoring
- Monitor API endpoint health
- Track API response times
- View API usage statistics
- Receive notifications for API issues

#### Additional Features
- View tenant activity logs
- Monitor system performance metrics
- Access reporting and analytics
- Manage tenant settings and configurations

### 5. Ongoing Operations

- Users log into the Website platform to access their tenant dashboard
- Website platform manages the session and provides access to HeadOffice
- All business operations are performed within the tenant's isolated database
- Users can switch between multiple tenants if they own multiple subscriptions

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

- User registration and authentication within tenant context
- Group (role) management
- Permission assignment
- User-employee linking
- JWT token generation with tenant claims

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

---

## PTS Controller

### Overview

**PTS (Pump Transaction System) Controller** is a protocol interface that enables communication between physical fuel stations and the HeadOffice system. It serves as the bridge that allows fuel station equipment to send real-time operational data to the cloud platform.

### Purpose

PTS Controller facilitates real-time data transmission from fuel station infrastructure (pumps, tanks, dispensers) to the HeadOffice system, enabling centralized monitoring and management of station operations.

### Key Functions

#### 1. **Pump Transaction Upload**
- Receives transaction data from fuel pumps
- Captures sales information including volume, price, payment method
- Records customer transactions in real-time
- Enables immediate transaction tracking and reporting

#### 2. **Tank Measurement Upload**
- Receives tank level measurements from sensors
- Monitors fuel inventory levels in real-time
- Tracks fuel volume in storage tanks
- Enables proactive inventory management

#### 3. **Tank Delivery Upload**
- Records fuel delivery information
- Updates tank levels after deliveries
- Tracks delivery volumes and timestamps
- Maintains delivery history

#### 4. **Status Upload**
- Receives operational status updates from station equipment
- Monitors pump and nozzle status (Active, Inactive, Maintenance)
- Tracks equipment health and availability
- Enables remote monitoring of station operations

### Data Flow

```
Fuel Station Equipment
         ↓
   PTS Controller
         ↓
   HeadOffice API
         ↓
   Tenant Database
         ↓
   Dashboard & Reports
```

### Integration Points

- **Protocol**: JSON-based PTS protocol (`jsonPTS`)
- **Request Types**: UploadPumpTransaction, UploadTankMeasurement, UploadInTankDelivery, UploadStatus
- **Response**: Standardized response format with success/error indicators
- **Authentication**: Secure API integration with HeadOffice

### Benefits

- **Real-time Monitoring**: Immediate visibility into station operations
- **Automated Data Collection**: Reduces manual data entry errors
- **Centralized Management**: All station data flows to a single platform
- **Historical Tracking**: Complete transaction and inventory history
- **Proactive Alerts**: Early detection of issues and anomalies

---

## User Management

### Overview

HeadOffice manages users within the tenant context. While user accounts and subscriptions are managed by the Website platform, HeadOffice handles the business user accounts that operate within each tenant's environment.

### User Management Architecture

#### Two-Level User System

1. **Platform Users** (Managed by Website Platform):
   - Account owners and subscription holders
   - Access to Website platform dashboard
   - Manage subscriptions, tenants, backups
   - Platform-level authentication

2. **Tenant Users** (Managed by HeadOffice):
   - Business users within each tenant
   - Employees, managers, administrators of fuel stations
   - Access to HeadOffice business operations
   - Tenant-scoped authentication and authorization

### How User Management Works in HeadOffice

#### User Authentication

- Users authenticate to HeadOffice using JWT tokens
- Tokens include tenant ID claims
- Tenant middleware validates tenant access
- Users are scoped to their tenant's database

#### Role-Based Access Control (RBAC)

**Groups (Roles)**:
- Collections of permissions assigned to users
- Examples: Admin, Manager, Operator, Viewer
- Custom groups can be created per tenant
- Groups define what actions users can perform

**Permissions**:
- Granular access rights for specific resources
- Permissions are assigned to groups
- Examples: View Stations, Create Transactions, Manage Employees
- Fine-grained control over user capabilities

**User-Group Assignment**:
- Users belong to one or more groups
- Permissions are inherited from assigned groups
- Users can have different roles in different contexts

#### Employee-User Linking

- Employees (staff at fuel stations) can be linked to user accounts
- Enables employees to access the system
- Links business identity (employee record) to system access (user account)
- Supports station-specific access control

### User Management Features

#### User Creation & Management
- Create user accounts within tenant context
- Assign users to groups (roles)
- Link users to employee records
- Enable/disable user accounts
- Manage user credentials

#### Permission Management
- Define custom permissions
- Create permission groups
- Assign permissions to groups
- Audit permission changes

#### Access Control
- Tenant isolation ensures users only access their tenant's data
- Group-based permissions control feature access
- Station-level access control through employee assignments
- Audit logging of user actions

### Integration with Website Platform

- Website platform manages platform user accounts
- HeadOffice manages tenant user accounts
- Platform users can access HeadOffice after tenant authentication
- Clear separation between platform and tenant user contexts

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

2. **Configure Database & Website Integration**

   - Update `appsettings.json` with your database connection strings
   - Configure Website platform API settings for tenant retrieval

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
2. Configure Website platform integration for tenant retrieval
3. Set up logging paths in configuration
4. Configure JWT settings for authentication
5. Ensure Website platform is running for tenant synchronization

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

#### Website Platform Integration

```json
{
  "FuelMasterWebsiteConfiguration": {
    "BaseUrl": "https://website-api-url",
    "ApiKey": "your-api-key-for-headoffice-requests"
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

### Tenant Synchronization

- HeadOffice periodically synchronizes tenant information from Website platform
- Tenant status changes (active/inactive) are reflected automatically
- New tenants become available after synchronization
- Tenant cache is refreshed when needed

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
