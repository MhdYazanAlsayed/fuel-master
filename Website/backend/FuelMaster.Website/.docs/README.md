# FuelMaster Tenant Management Platform - System Design

## Overview

This platform serves as the **Tenant Management System** for the FuelMaster multi-tenant SaaS architecture. It is responsible for managing tenant creation, subscriptions, billing, and database provisioning. This system does **NOT** store tenant-specific business data - it only manages tenant metadata and orchestrates database creation on the Headoffice server.

## System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│         FuelMaster Tenant Management Platform                │
│  (This System - TenantDB)                                    │
├─────────────────────────────────────────────────────────────┤
│  • User Authentication (Identity + Google OAuth)            │
│  • Subscription Management                                   │
│  • Billing & Payment Processing                              │
│  • Tenant Creation & Management                              │
│  • Database Provisioning Orchestration                       │
│  • Root Admin Functions                                      │
│  • Backup/Restore Management                                 │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ REST API
                          ▼
┌─────────────────────────────────────────────────────────────┐
│              Headoffice Server                               │
├─────────────────────────────────────────────────────────────┤
│  • Database Creation Service                                │
│  • Tenant Database Storage                                   │
│  • Business Logic System                                     │
│  • Employee/User/Role/Permission Management                  │
│  • Hierarchy Management                                      │
└─────────────────────────────────────────────────────────────┘
```

### Key Principles

1. **Separation of Concerns**: This system (TenantDB) only manages tenant metadata and subscriptions. All tenant business data resides in databases on the Headoffice server.
2. **Database Per Tenant**: Each tenant gets their own isolated database on the Headoffice server.
3. **No Tenant Data Storage**: This system does NOT store any tenant-specific business data (employees, users, roles, etc.).

## User Workflow

### 1. Account Creation
- User registers via:
  - **Standard Registration**: Email/Password using ASP.NET Core Identity
  - **Google OAuth**: Social authentication
- User account is stored in this system's database (TenantDB)

### 2. Plan Selection
- User must select a subscription plan
- At least one **Free Plan** available for testing
- Plan information stored with user account
- User cannot proceed to tenant creation without selecting a plan

### 3. Subscription Management
- **Free Plan**: 
  - No billing required
  - Subscription status tracked
  - Subscription date recorded
- **Paid Plans**:
  - Payment card information collected and stored securely
  - Billing information tracked:
    - Subscription start date
    - Subscription status (Active, Suspended, Cancelled, etc.)
    - Next billing date
    - Payment history
  - Integration with custom payment provider

### 4. Tenant Creation
- User provides:
  - **Tenant Name**: Unique identifier (e.g., "fuel-star")
  - **Admin Account Information**: 
    - Admin username
    - Admin email
    - Admin password (hashed)
- System validates tenant name uniqueness
- System initiates tenant database creation process

### 5. Database Provisioning
- System communicates with Headoffice server via REST API
- Request includes:
  - Tenant name
  - Admin user credentials
  - Selected plan information
- Headoffice server:
  - Creates new database for tenant
  - Sets up initial schema
  - Creates admin user in tenant database
  - Returns connection string
- System stores:
  - Tenant name
  - Connection string (encrypted)
  - Database creation timestamp
  - Admin user reference

### 6. Integration with Headoffice
- Headoffice system can query this platform for:
  - Tenant information
  - Connection strings
  - Tenant status
- All business data (Employees, Users, Roles, Permissions, Hierarchy) is managed by Headoffice system
- This system provides tenant metadata only

## Core Features

### 1. Authentication & Authorization
- **ASP.NET Core Identity** for user management
- **Google OAuth** integration for social login
- Role-based access control:
  - Regular users
  - Root administrators
- JWT or Cookie-based authentication

### 2. Subscription Management
- **Plan Management**:
  - Create/Read/Update/Delete subscription plans
  - Plan features and limits
  - Pricing information
- **User Subscriptions**:
  - Track user's selected plan
  - Subscription status tracking
  - Subscription history
- **Free Plan Support**:
  - Special handling for free tier
  - No billing required

### 3. Billing & Payments
- **Payment Card Management**:
  - Secure storage of payment cards (PCI compliance)
  - Card validation
  - Card update/delete functionality
- **Billing Information**:
  - Subscription start date
  - Current status (Active, Suspended, Cancelled, Expired)
  - Next billing date
  - Billing history
- **Custom Payment Provider Integration**:
  - Flexible payment gateway integration
  - Payment processing
  - Refund handling

### 4. Tenant Management
- **Tenant CRUD Operations**:
  - Create tenant
  - View tenant details
  - Update tenant information
  - Deactivate/Delete tenant
- **Tenant Metadata Storage**:
  - Tenant name (unique identifier)
  - Connection string (encrypted)
  - Creation date
  - Status (Active, Suspended, Deleted)
  - Associated user account
  - Selected plan

### 5. Database Provisioning
- **Orchestration Service**:
  - Communicates with Headoffice REST API
  - Initiates database creation
  - Handles provisioning errors
  - Retry logic for failed operations
- **Connection String Management**:
  - Secure storage (encryption at rest)
  - Retrieval for Headoffice queries
  - Rotation support (future)

### 6. Root Admin Functions
- **Platform Administration**:
  - User management
  - Tenant oversight
  - Subscription monitoring
  - System health checks
- **Access Control**:
  - Root admin role
  - Admin-only endpoints
  - Audit logging

### 7. Backup & Restore Management
- **Backup Operations**:
  - Initiate tenant database backups
  - Schedule automated backups
  - Backup history tracking
- **Restore Operations**:
  - Restore from backup
  - Point-in-time recovery
  - Restore validation
- **Integration**:
  - Coordinate with Headoffice system for actual backup/restore operations

## Database Design

### Technology Stack
- **Database**: SQL Server
- **ORM**: Entity Framework Core (recommended)
- **Migrations**: EF Core Migrations

### Core Entities

#### 1. ApplicationUser (Identity)
- Inherits from `IdentityUser`
- Additional properties:
  - `FirstName`
  - `LastName`
  - `CreatedAt`
  - `IsRootAdmin` (boolean)

#### 2. SubscriptionPlan
```csharp
- Id (Guid)
- Name (string)
- Description (string)
- Price (decimal)
- BillingCycle (enum: Monthly, Yearly)
- IsFree (boolean)
- Features (JSON or related table)
- IsActive (boolean)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

#### 3. UserSubscription
```csharp
- Id (Guid)
- UserId (Guid) -> ApplicationUser
- PlanId (Guid) -> SubscriptionPlan
- Status (enum: Active, Suspended, Cancelled, Expired)
- StartDate (DateTime)
- EndDate (DateTime?)
- NextBillingDate (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

#### 4. PaymentCard
```csharp
- Id (Guid)
- UserId (Guid) -> ApplicationUser
- CardLastFour (string) // Last 4 digits
- CardBrand (string) // Visa, Mastercard, etc.
- ExpiryMonth (int)
- ExpiryYear (int)
- IsDefault (boolean)
- Token (string) // Encrypted payment token from provider
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

#### 5. BillingHistory
```csharp
- Id (Guid)
- UserSubscriptionId (Guid) -> UserSubscription
- Amount (decimal)
- Status (enum: Pending, Completed, Failed, Refunded)
- PaymentDate (DateTime)
- TransactionId (string)
- PaymentProviderResponse (JSON)
- CreatedAt (DateTime)
```

#### 6. Tenant
```csharp
- Id (Guid)
- Name (string) // Unique, e.g., "fuel-star"
- UserId (Guid) -> ApplicationUser
- ConnectionString (string) // Encrypted
- DatabaseName (string)
- Status (enum: Active, Suspended, Deleted)
- PlanId (Guid) -> SubscriptionPlan
- AdminUsername (string)
- AdminEmail (string)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- DeletedAt (DateTime?)
```

#### 7. TenantBackup
```csharp
- Id (Guid)
- TenantId (Guid) -> Tenant
- BackupName (string)
- BackupType (enum: Full, Incremental)
- Status (enum: InProgress, Completed, Failed)
- BackupDate (DateTime)
- FileSize (long)
- BackupLocation (string)
- CreatedAt (DateTime)
```

#### 8. AuditLog
```csharp
- Id (Guid)
- UserId (Guid?) -> ApplicationUser (nullable)
- Action (string) // "CreateTenant", "UpdateSubscription", etc.
- EntityType (string)
- EntityId (Guid)
- Changes (JSON) // Before/After values
- IpAddress (string)
- UserAgent (string)
- CreatedAt (DateTime)
```

## Integration Points

### Headoffice System Integration

#### REST API Endpoints (Headoffice provides)

1. **Create Tenant Database**
   ```
   POST /api/tenants/create
   Request Body:
   {
     "tenantName": "fuel-star",
     "adminUsername": "admin",
     "adminEmail": "admin@fuel-star.com",
     "adminPassword": "hashed_password",
     "planId": "guid"
   }
   Response:
   {
     "success": true,
     "databaseName": "FuelMaster_fuel-star",
     "connectionString": "encrypted_connection_string",
     "adminUserId": "guid"
   }
   ```

2. **Get Tenant Information**
   ```
   GET /api/tenants/{tenantName}
   Response:
   {
     "tenantName": "fuel-star",
     "connectionString": "encrypted_connection_string",
     "status": "Active"
   }
   ```

3. **Backup Tenant Database**
   ```
   POST /api/tenants/{tenantName}/backup
   Request Body:
   {
     "backupType": "Full",
     "backupName": "backup_2024_01_15"
   }
   ```

4. **Restore Tenant Database**
   ```
   POST /api/tenants/{tenantName}/restore
   Request Body:
   {
     "backupId": "guid",
     "restorePoint": "2024-01-15T10:30:00Z"
   }
   ```

#### This System Provides (for Headoffice queries)

1. **Get Tenant Connection String**
   ```
   GET /api/tenants/{tenantName}/connection-string
   Headers: Authorization: Bearer {token}
   Response:
   {
     "connectionString": "encrypted_connection_string",
     "databaseName": "FuelMaster_fuel-star"
   }
   ```

2. **List All Tenants**
   ```
   GET /api/tenants
   Headers: Authorization: Bearer {root_admin_token}
   Response:
   {
     "tenants": [
       {
         "id": "guid",
         "name": "fuel-star",
         "status": "Active",
         "createdAt": "2024-01-15T10:00:00Z"
       }
     ]
   }
   ```

## Technology Stack

### Backend
- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - API framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **ASP.NET Core Identity** - Authentication
- **Google OAuth** - Social authentication
- **Swagger/OpenAPI** - API documentation

### Security
- **JWT Authentication** or **Cookie Authentication**
- **Encryption at Rest** - For sensitive data (connection strings, payment tokens)
- **HTTPS** - Enforced
- **CORS** - Configured appropriately
- **Input Validation** - Data annotations, FluentValidation

### Infrastructure
- **Dependency Injection** - Built-in .NET DI
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **MediatR** (optional) - CQRS pattern
- **AutoMapper** (optional) - Object mapping

## Project Structure

```
FuelMaster.Website/
├── FuelMaster.Website/              # API Layer
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── SubscriptionController.cs
│   │   ├── TenantController.cs
│   │   ├── BillingController.cs
│   │   ├── BackupController.cs
│   │   └── AdminController.cs
│   ├── DTOs/
│   │   ├── Requests/
│   │   └── Responses/
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── AuditLoggingMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── FuelMaster.Website.Core/          # Domain Layer
│   ├── Entities/
│   │   ├── ApplicationUser.cs
│   │   ├── SubscriptionPlan.cs
│   │   ├── UserSubscription.cs
│   │   ├── PaymentCard.cs
│   │   ├── BillingHistory.cs
│   │   ├── Tenant.cs
│   │   └── TenantBackup.cs
│   ├── Enums/
│   │   ├── SubscriptionStatus.cs
│   │   ├── TenantStatus.cs
│   │   └── BackupStatus.cs
│   ├── Interfaces/
│   │   ├── ITenantService.cs
│   │   ├── ISubscriptionService.cs
│   │   ├── IBillingService.cs
│   │   ├── IBackupService.cs
│   │   └── IHeadofficeApiClient.cs
│   └── Services/
│       ├── TenantService.cs
│       ├── SubscriptionService.cs
│       ├── BillingService.cs
│       └── BackupService.cs
│
└── FuelMaster.Website.Infrastructure/ # Infrastructure Layer
    ├── Data/
    │   ├── ApplicationDbContext.cs
    │   ├── Configurations/
    │   └── Migrations/
    ├── Repositories/
    │   ├── ITenantRepository.cs
    │   ├── TenantRepository.cs
    │   └── ...
    ├── Services/
    │   ├── HeadofficeApiClient.cs
    │   ├── EncryptionService.cs
    │   └── PaymentProviderService.cs
    └── Identity/
        └── IdentityConfig.cs
```

## Implementation Phases

### Phase 1: Foundation
1. Set up ASP.NET Core Identity
2. Configure Google OAuth
3. Create database context and initial migrations
4. Implement basic authentication endpoints
5. Set up project structure (Clean Architecture)

### Phase 2: Subscription Management
1. Create SubscriptionPlan entity and seed data
2. Implement plan selection workflow
3. Create UserSubscription entity
4. Implement subscription status tracking
5. Add subscription management endpoints

### Phase 3: Billing & Payments
1. Design payment card storage (encrypted)
2. Integrate with custom payment provider
3. Implement billing history tracking
4. Create billing management endpoints
5. Add subscription status management (Active, Suspended, etc.)

### Phase 4: Tenant Management
1. Create Tenant entity
2. Implement tenant name validation and uniqueness
3. Create tenant creation workflow
4. Implement tenant listing and details endpoints
5. Add tenant status management

### Phase 5: Database Provisioning
1. Create Headoffice API client service
2. Implement database creation orchestration
3. Handle connection string storage (encrypted)
4. Add error handling and retry logic
5. Implement tenant database validation

### Phase 6: Root Admin Features
1. Implement root admin role
2. Create admin-only endpoints
3. Add user management for admins
4. Implement tenant oversight features
5. Add system health monitoring

### Phase 7: Backup & Restore
1. Design backup/restore workflow
2. Integrate with Headoffice backup API
3. Implement backup scheduling
4. Create restore functionality
5. Add backup history tracking

### Phase 8: Integration & Testing
1. Complete Headoffice system integration
2. End-to-end testing
3. Security audit
4. Performance optimization
5. Documentation

## Security Considerations

1. **Connection String Encryption**: All tenant connection strings must be encrypted at rest
2. **Payment Card Security**: PCI compliance - store only tokens, never full card numbers
3. **API Authentication**: Secure API communication between this system and Headoffice
4. **Input Validation**: Validate all user inputs, especially tenant names
5. **SQL Injection Prevention**: Use parameterized queries (EF Core handles this)
6. **Audit Logging**: Log all sensitive operations (tenant creation, subscription changes, etc.)
7. **Rate Limiting**: Implement rate limiting on public endpoints
8. **HTTPS Only**: Enforce HTTPS in production

## Configuration Requirements

### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=FuelMaster_TenantDB;..."
  },
  "Identity": {
    "Google": {
      "ClientId": "...",
      "ClientSecret": "..."
    }
  },
  "PaymentProvider": {
    "ApiKey": "...",
    "ApiSecret": "...",
    "BaseUrl": "..."
  },
  "Headoffice": {
    "ApiBaseUrl": "https://headoffice-api.example.com",
    "ApiKey": "...",
    "TimeoutSeconds": 30
  },
  "Encryption": {
    "Key": "..." // For connection string encryption
  }
}
```

## Open Questions & Decisions Needed

1. **Payment Provider Details**: What is the custom payment provider? What are their API specifications?
2. **Headoffice API Contract**: Need detailed API documentation from Headoffice team
3. **Database Naming Convention**: What pattern for tenant database names? (e.g., `FuelMaster_{tenantName}`)
4. **Connection String Format**: What database system on Headoffice? (SQL Server, PostgreSQL, etc.)
5. **Backup Storage**: Where are backups stored? How long are they retained?
6. **Subscription Limits**: What limits per plan? (e.g., number of users, storage, features)
7. **Multi-Plan Support**: Can a user have multiple subscriptions/tenants?
8. **Tenant Deletion**: Soft delete or hard delete? What happens to the database?
9. **Root Admin Creation**: How are root admins initially created? Manual database insert or setup endpoint?

## Next Steps

1. Review and refine this design document
2. Get Headoffice API specifications
3. Finalize payment provider integration details
4. Set up development environment
5. Begin Phase 1 implementation

---

**Document Version**: 1.0  
**Last Updated**: 2024-01-15  
**Status**: Design Phase

