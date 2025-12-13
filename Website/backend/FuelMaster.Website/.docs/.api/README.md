# FuelMaster Tenant Management Platform - API Documentation

## Base URL

```
https://api.fuelmaster.com/api
```

For local development:

```
https://localhost:5001/api
```

## Authentication

Most endpoints require authentication via secure HTTP-only cookies. The authentication token is automatically stored in a secure cookie after login/registration.

### Getting Authenticated

1. Register a new account via `POST /api/auth/register`
2. Login via `POST /api/auth/login`
3. The authentication token is automatically stored in a secure cookie named `access_token`
4. All subsequent requests will automatically include this cookie for authentication

**Note:** The cookie is:

- HTTP-only (not accessible via JavaScript)
- Secure (only sent over HTTPS)
- SameSite=Strict (CSRF protection)
- Automatically expires based on token expiration

### Token Payload

The JWT token includes the following information in its payload:

- **User Information**: User ID, email, name
- **Roles**: User roles
- **Tenant Information**:
  - `has_tenant`: Whether the user has a tenant
  - `tenant_status`: Current tenant status (e.g., "Active", "Suspended")
- **Subscription Information**:
  - `has_subscription`: Whether the user has a subscription
  - `subscription_status`: Current subscription status (e.g., "Active", "Cancelled")
  - `subscription_end_date`: Subscription end date (ISO 8601 format, if applicable)

This information is used for authorization checks without requiring database queries on each request. If a user's tenant or subscription status changes, they will need to re-authenticate to receive an updated token.

---

## API Endpoints

### 1. Authentication (`/api/auth`)

#### Register User

**POST** `/api/auth/register`

Create a new user account.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response:** `200 OK`

The authentication token is set in a secure HTTP-only cookie named `access_token`. The response includes user status information (subscription and tenant status) to help the frontend determine next steps.

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": false,
  "hasActiveTenant": false,
  "canPerformOperations": false,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": "Please subscribe to a plan to continue",
  "expiresAt": "2024-01-16T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Validation errors
- `500 Internal Server Error` - Server error

**Notes:**

- The response format matches the `/api/auth/check` endpoint, providing immediate status information after registration
- New users will typically have `hasActiveSubscription: false` and `hasActiveTenant: false`

---

#### Login

**POST** `/api/auth/login`

Authenticate and get JWT token.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:** `200 OK`

The authentication token is set in a secure HTTP-only cookie named `access_token`. The response includes user status information (subscription and tenant status) to help the frontend determine next steps.

**Response when user has active subscription and tenant:**

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": true,
  "hasActiveTenant": true,
  "canPerformOperations": true,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": null,
  "expiresAt": "2024-01-16T10:00:00Z"
}
```

**Response when subscription is missing:**

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": false,
  "hasActiveTenant": false,
  "canPerformOperations": false,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": "Please subscribe to a plan to continue",
  "expiresAt": "2024-01-16T10:00:00Z"
}
```

**Response when tenant is missing:**

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": true,
  "hasActiveTenant": false,
  "canPerformOperations": false,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": "Please create a tenant to continue",
  "expiresAt": "2024-01-16T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid request
- `401 Unauthorized` - Invalid credentials
- `500 Internal Server Error` - Server error

**Notes:**

- The response format matches the `/api/auth/check` endpoint, providing immediate status information after login
- The frontend can use this response to determine if the user needs to subscribe to a plan or create a tenant without making an additional API call

---

#### Google OAuth Login

**POST** `/api/auth/google`

Initiate Google OAuth authentication flow.

**Response:** Redirects to Google OAuth consent screen

**Callback:** `GET /api/auth/google-callback`

Returns the same response format as login endpoint (token stored in cookie).

---

#### Logout

**POST** `/api/auth/logout`

Logout the authenticated user and clear the authentication cookie.

**Response:** `200 OK`

```json
{
  "message": "Logged out successfully"
}
```

---

#### Check User Status

**GET** `/api/auth/check`

Check the current user's authentication status, active subscription, and active tenant. This endpoint reads information directly from the JWT token payload (no database queries), making it fast and efficient. It is used by the frontend to determine if the user can perform operations or needs to subscribe/create a tenant. Requires authentication.

**Performance Note:** This endpoint reads tenant and subscription information from the JWT token claims, eliminating database queries for improved performance.

**Response:** `200 OK`

**Note:** The `expiresAt` field is not included in this response since it's not an authentication endpoint. Use `/api/auth/login` or `/api/auth/register` to get token expiration information.

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": true,
  "hasActiveTenant": true,
  "canPerformOperations": true,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": null,
  "expiresAt": null
}
```

**Response when subscription is missing:**

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": false,
  "hasActiveTenant": false,
  "canPerformOperations": false,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": "Please subscribe to a plan to continue",
  "expiresAt": null
}
```

**Response when tenant is missing:**

```json
{
  "isAuthenticated": true,
  "hasActiveSubscription": true,
  "hasActiveTenant": false,
  "canPerformOperations": false,
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  },
  "message": "Please create a tenant to continue",
  "expiresAt": null
}
```

**Error Responses:**

- `401 Unauthorized` - User is not authenticated
- `500 Internal Server Error` - Server error

**Notes:**

- `canPerformOperations` is `true` only when both `hasActiveSubscription` and `hasActiveTenant` are `true`
- The frontend should use this endpoint to determine if the user needs to subscribe to a plan or create a tenant
- Users without an active subscription and tenant cannot perform operations on most endpoints (see authorization requirements below)
- This endpoint reads data from the JWT token claims, so the information reflects the state at the time of authentication. If a user's subscription or tenant status changes, they will need to re-authenticate to get updated information in their token
- The `expiresAt` field is `null` for this endpoint since it's not an authentication endpoint. Use `/api/auth/login` or `/api/auth/register` to get token expiration information

---

### 2. Subscriptions (`/api/subscription`)

**All endpoints require authentication unless specified.**

#### Get Subscription Plans

**GET** `/api/subscription/plans?activeOnly=true`

Get all available subscription plans. Public endpoint (no authentication required).

**Query Parameters:**

- `activeOnly` (boolean, default: true) - Filter only active plans

**Response:** `200 OK`

```json
[
  {
    "id": "plan-guid",
    "name": "Free Plan",
    "description": "Free plan for testing",
    "price": 0.0,
    "billingCycle": "Monthly",
    "isFree": true,
    "features": "{\"maxUsers\": 5, \"maxStations\": 1}",
    "isActive": true
  },
  {
    "id": "plan-guid-2",
    "name": "Starter Plan",
    "description": "Perfect for small businesses",
    "price": 29.99,
    "billingCycle": "Monthly",
    "isFree": false,
    "features": "{\"maxUsers\": 25, \"maxStations\": 5}",
    "isActive": true
  }
]
```

---

#### Get Plan by ID

**GET** `/api/subscription/plans/{id}`

Get a specific subscription plan by ID. Public endpoint.

**Response:** `200 OK`

```json
{
  "id": "plan-guid",
  "name": "Free Plan",
  "description": "Free plan for testing",
  "price": 0.0,
  "billingCycle": "Monthly",
  "isFree": true,
  "features": "{\"maxUsers\": 5}",
  "isActive": true
}
```

**Error Responses:**

- `404 Not Found` - Plan not found

---

#### Subscribe to Plan

**POST** `/api/subscription/subscribe`

Subscribe the authenticated user to a subscription plan.

**Request Body:**

```json
{
  "planId": "plan-guid"
}
```

**Response:** `200 OK`

```json
{
  "id": "subscription-guid",
  "planId": "plan-guid",
  "plan": {
    "id": "plan-guid",
    "name": "Starter Plan",
    "description": "Perfect for small businesses",
    "price": 29.99,
    "billingCycle": "Monthly",
    "isFree": false,
    "features": "{\"maxUsers\": 25}",
    "isActive": true
  },
  "status": "Active",
  "startDate": "2024-01-15T10:00:00Z",
  "endDate": null,
  "nextBillingDate": "2024-02-15T10:00:00Z",
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid plan or plan not active
- `401 Unauthorized` - User not authenticated

---

#### Get My Active Subscription

**GET** `/api/subscription/my-subscription`

Get the authenticated user's active subscription.

**Response:** `200 OK`

```json
{
  "id": "subscription-guid",
  "planId": "plan-guid",
  "plan": { ... },
  "status": "Active",
  "startDate": "2024-01-15T10:00:00Z",
  "endDate": null,
  "nextBillingDate": "2024-02-15T10:00:00Z",
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `404 Not Found` - No active subscription found

---

#### Get My Subscriptions

**GET** `/api/subscription/my-subscriptions`

Get all subscriptions for the authenticated user (including cancelled/expired).

**Response:** `200 OK`

```json
[
  {
    "id": "subscription-guid",
    "planId": "plan-guid",
    "plan": { ... },
    "status": "Active",
    "startDate": "2024-01-15T10:00:00Z",
    "endDate": null,
    "nextBillingDate": "2024-02-15T10:00:00Z",
    "createdAt": "2024-01-15T10:00:00Z"
  }
]
```

---

#### Cancel Subscription

**POST** `/api/subscription/{subscriptionId}/cancel`

Cancel a specific subscription.

**Response:** `200 OK`

```json
{
  "message": "Subscription cancelled successfully"
}
```

**Error Responses:**

- `404 Not Found` - Subscription not found or doesn't belong to user

---

### 3. Billing (`/api/billing`)

**All endpoints require authentication.**

#### Add Payment Card

**POST** `/api/billing/payment-cards`

Add a payment card for the authenticated user.

**Request Body:**

```json
{
  "cardNumber": "4111111111111111",
  "expiryMonth": 12,
  "expiryYear": 2025,
  "cvv": "123",
  "cardholderName": "John Doe"
}
```

**Response:** `200 OK`

```json
{
  "id": "card-guid",
  "cardLastFour": "1111",
  "cardBrand": "Visa",
  "expiryMonth": 12,
  "expiryYear": 2025,
  "isDefault": true,
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid card information
- `401 Unauthorized` - User not authenticated

---

#### Get Payment Cards

**GET** `/api/billing/payment-cards`

Get all payment cards for the authenticated user.

**Response:** `200 OK`

```json
[
  {
    "id": "card-guid",
    "cardLastFour": "1111",
    "cardBrand": "Visa",
    "expiryMonth": 12,
    "expiryYear": 2025,
    "isDefault": true,
    "createdAt": "2024-01-15T10:00:00Z"
  }
]
```

---

#### Delete Payment Card

**DELETE** `/api/billing/payment-cards/{cardId}`

Delete a payment card.

**Response:** `200 OK`

```json
{
  "message": "Payment card deleted successfully"
}
```

**Error Responses:**

- `404 Not Found` - Card not found

---

#### Set Default Payment Card

**PUT** `/api/billing/payment-cards/{cardId}/set-default`

Set a payment card as the default.

**Response:** `200 OK`

```json
{
  "id": "card-guid",
  "cardLastFour": "1111",
  "cardBrand": "Visa",
  "expiryMonth": 12,
  "expiryYear": 2025,
  "isDefault": true,
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `404 Not Found` - Card not found

---

#### Process Billing

**POST** `/api/billing/process/{subscriptionId}`

Process payment for a subscription using the default payment card.

**Response:** `200 OK`

```json
{
  "id": "billing-guid",
  "userSubscriptionId": "subscription-guid",
  "amount": 29.99,
  "status": "Completed",
  "paymentDate": "2024-01-15T10:00:00Z",
  "transactionId": "txn-123456",
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid subscription or no payment card
- `500 Internal Server Error` - Payment processing failed

---

#### Get Billing History

**GET** `/api/billing/history`

Get billing history for the authenticated user.

**Response:** `200 OK`

```json
[
  {
    "id": "billing-guid",
    "userSubscriptionId": "subscription-guid",
    "amount": 29.99,
    "status": "Completed",
    "paymentDate": "2024-01-15T10:00:00Z",
    "transactionId": "txn-123456",
    "createdAt": "2024-01-15T10:00:00Z"
  }
]
```

---

#### Refund Billing

**POST** `/api/billing/refund/{billingHistoryId}`

Process a refund for a completed billing transaction.

**Response:** `200 OK`

```json
{
  "message": "Refund processed successfully"
}
```

**Error Responses:**

- `400 Bad Request` - Unable to process refund

---

### 4. Tenants (`/api/tenant`)

**All endpoints require authentication.**

#### Create Tenant

**POST** `/api/tenant`

Create a new tenant. Requires an active subscription.

**Request Body:**

```json
{
  "name": "fuel-star",
  "adminUsername": "admin",
  "adminEmail": "admin@fuel-star.com",
  "adminPassword": "SecurePassword123!"
}
```

**Response:** `201 Created`

```json
{
  "id": "tenant-guid",
  "name": "fuel-star",
  "databaseName": "FuelMaster_fuel-star",
  "status": "Active",
  "planId": "plan-guid",
  "plan": {
    "id": "plan-guid",
    "name": "Starter Plan",
    "description": "Perfect for small businesses",
    "price": 29.99,
    "billingCycle": "Monthly",
    "isFree": false,
    "features": "{\"maxUsers\": 25}",
    "isActive": true
  },
  "adminUsername": "admin",
  "adminEmail": "admin@fuel-star.com",
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid tenant name or missing subscription
- `409 Conflict` - Tenant name already exists
- `500 Internal Server Error` - Database provisioning failed

**Notes:**

- Tenant name must be lowercase alphanumeric with hyphens only
- Tenant name must be unique
- User must have an active subscription

---

#### Get Tenant by ID

**GET** `/api/tenant/{id}`

Get tenant details by ID. User must own the tenant or be a root admin.

**Response:** `200 OK`

```json
{
  "id": "tenant-guid",
  "name": "fuel-star",
  "databaseName": "FuelMaster_fuel-star",
  "status": "Active",
  "planId": "plan-guid",
  "plan": { ... },
  "adminUsername": "admin",
  "adminEmail": "admin@fuel-star.com",
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `403 Forbidden` - User doesn't own the tenant
- `404 Not Found` - Tenant not found

---

#### Get Tenant by Name

**GET** `/api/tenant/name/{name}`

Get tenant details by name. User must own the tenant or be a root admin.

**Response:** Same as Get Tenant by ID

---

#### Get My Tenant

**GET** `/api/tenant/my-tenant`

Get the authenticated user's tenant.

**Response:** Same as Get Tenant by ID

**Error Responses:**

- `404 Not Found` - No tenant found for user

---

#### Get My Tenants

**GET** `/api/tenant/my-tenants`

Get all tenants for the authenticated user.

**Response:** `200 OK`

```json
[
  {
    "id": "tenant-guid",
    "name": "fuel-star",
    "databaseName": "FuelMaster_fuel-star",
    "status": "Active",
    "planId": "plan-guid",
    "plan": { ... },
    "adminUsername": "admin",
    "adminEmail": "admin@fuel-star.com",
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": "2024-01-15T10:00:00Z"
  }
]
```

---

#### Update Tenant

**PUT** `/api/tenant/{id}`

Update tenant information. User must own the tenant or be a root admin.

**Request Body:**

```json
{
  "name": "new-tenant-name",
  "status": "Suspended"
}
```

**Response:** `200 OK` (Same format as Get Tenant)

**Error Responses:**

- `400 Bad Request` - Invalid data
- `403 Forbidden` - User doesn't own the tenant
- `404 Not Found` - Tenant not found
- `409 Conflict` - New name already exists

---

#### Delete Tenant

**DELETE** `/api/tenant/{id}`

Soft delete a tenant. User must own the tenant.

**Response:** `200 OK`

```json
{
  "message": "Tenant deleted successfully"
}
```

**Error Responses:**

- `404 Not Found` - Tenant not found or doesn't belong to user

---

#### Get Tenant Connection String

**GET** `/api/tenant/{tenantName}/connection-string`

Get the decrypted connection string for a tenant. **Root Admin only.**

**Response:** `200 OK`

```json
{
  "connectionString": "Server=...;Database=FuelMaster_fuel-star;...",
  "tenantName": "fuel-star"
}
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin
- `404 Not Found` - Tenant not found

---

### 5. Backups (`/api/backup`)

**All endpoints require authentication.**

#### Create Backup

**POST** `/api/backup/tenant/{tenantId}`

Create a backup for a tenant. User must own the tenant or be a root admin.

**Request Body:**

```json
{
  "backupType": "Full",
  "backupName": "backup_2024_01_15"
}
```

**Response:** `201 Created`

```json
{
  "id": "backup-guid",
  "tenantId": "tenant-guid",
  "tenantName": "fuel-star",
  "backupName": "backup_2024_01_15",
  "backupType": "Full",
  "status": "Completed",
  "backupDate": "2024-01-15T10:00:00Z",
  "fileSize": 1048576,
  "backupLocation": "/backups/fuel-star/backup_2024_01_15.bak",
  "createdAt": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid request
- `403 Forbidden` - User doesn't own the tenant
- `404 Not Found` - Tenant not found
- `500 Internal Server Error` - Backup creation failed

**Backup Types:**

- `Full` - Complete database backup
- `Incremental` - Incremental backup

---

#### Get Backups by Tenant

**GET** `/api/backup/tenant/{tenantId}`

Get all backups for a tenant. User must own the tenant or be a root admin.

**Response:** `200 OK`

```json
[
  {
    "id": "backup-guid",
    "tenantId": "tenant-guid",
    "tenantName": "fuel-star",
    "backupName": "backup_2024_01_15",
    "backupType": "Full",
    "status": "Completed",
    "backupDate": "2024-01-15T10:00:00Z",
    "fileSize": 1048576,
    "backupLocation": "/backups/fuel-star/backup_2024_01_15.bak",
    "createdAt": "2024-01-15T10:00:00Z"
  }
]
```

---

#### Get Backup by ID

**GET** `/api/backup/{id}`

Get backup details by ID. User must own the tenant or be a root admin.

**Response:** `200 OK` (Same format as Create Backup)

**Error Responses:**

- `403 Forbidden` - User doesn't own the tenant
- `404 Not Found` - Backup not found

---

#### Restore Backup

**POST** `/api/backup/{id}/restore`

Restore a tenant database from a backup. User must own the tenant or be a root admin.

**Request Body (Optional):**

```json
{
  "restorePoint": "2024-01-15T10:30:00Z"
}
```

**Response:** `200 OK`

```json
{
  "message": "Backup restored successfully"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid backup or restore point
- `403 Forbidden` - User doesn't own the tenant
- `404 Not Found` - Backup not found
- `500 Internal Server Error` - Restore failed

---

### 6. Admin (`/api/admin`)

**All endpoints require Root Admin role.**

#### Get All Users

**GET** `/api/admin/users`

Get all users in the system.

**Response:** `200 OK`

```json
[
  {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "createdAt": "2024-01-15T10:00:00Z",
    "tenantCount": 1,
    "subscriptionCount": 1
  }
]
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin

---

#### Get User by ID

**GET** `/api/admin/users/{userId}`

Get user details by ID.

**Response:** `200 OK`

```json
{
  "id": "user-guid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "createdAt": "2024-01-15T10:00:00Z",
  "tenantCount": 1,
  "subscriptionCount": 1
}
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin
- `404 Not Found` - User not found

---

#### Set Root Admin Status

**PUT** `/api/admin/users/{userId}/root-admin`

Grant or revoke root admin status for a user.

**Request Body:**

```json
{
  "isRootAdmin": true
}
```

**Response:** `200 OK`

```json
{
  "message": "User root admin status updated to true"
}
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin
- `404 Not Found` - User not found

---

#### Get All Tenants

**GET** `/api/admin/tenants`

Get all tenants in the system.

**Response:** `200 OK`

```json
[
  {
    "id": "tenant-guid",
    "name": "fuel-star",
    "databaseName": "FuelMaster_fuel-star",
    "status": "Active",
    "planId": "plan-guid",
    "plan": { ... },
    "adminUsername": "admin",
    "adminEmail": "admin@fuel-star.com",
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": "2024-01-15T10:00:00Z"
  }
]
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin

---

#### Get System Health

**GET** `/api/admin/health`

Get system health metrics and statistics.

**Response:** `200 OK`

```json
{
  "totalUsers": 150,
  "activeUsers": 120,
  "totalTenants": 45,
  "activeTenants": 42,
  "totalSubscriptions": 48,
  "activeSubscriptions": 42,
  "lastChecked": "2024-01-15T10:00:00Z"
}
```

**Error Responses:**

- `403 Forbidden` - User is not a root admin

---

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request

```json
{
  "message": "Error description",
  "errors": {
    "fieldName": ["Error message"]
  }
}
```

### 401 Unauthorized

```json
{
  "message": "User not authenticated"
}
```

### 403 Forbidden

**Standard Forbidden Response:**

```json
{
  "message": "Access denied"
}
```

**Missing Active Subscription:**

```json
{
  "message": "Active subscription required",
  "code": "NO_ACTIVE_SUBSCRIPTION",
  "requiresAction": "subscribe"
}
```

**Missing Active Tenant:**

```json
{
  "message": "Active tenant required",
  "code": "NO_ACTIVE_TENANT",
  "requiresAction": "create_tenant"
}
```

### 404 Not Found

```json
{
  "message": "Resource not found"
}
```

### 409 Conflict

```json
{
  "message": "Resource conflict (e.g., duplicate name)"
}
```

### 500 Internal Server Error

```json
{
  "message": "An error occurred while processing your request"
}
```

---

## Status Codes Reference

- **200 OK** - Request succeeded
- **201 Created** - Resource created successfully
- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Authentication required
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **409 Conflict** - Resource conflict
- **500 Internal Server Error** - Server error

---

## Rate Limiting

Currently, there are no rate limits implemented. This may be added in future versions.

---

## Pagination

Currently, list endpoints return all results. Pagination may be added in future versions for endpoints that return large datasets.

---

## Versioning

Current API version: **v1**

API versioning may be implemented in future versions using URL versioning:

```
/api/v1/tenant
/api/v2/tenant
```

---

## Swagger Documentation

Interactive API documentation is available at:

```
/swagger
```

This provides a complete interactive interface to test all endpoints.

---

## Notes

1. **Authentication**: Authentication tokens are stored in secure HTTP-only cookies. The cookie is automatically sent with each request. Tokens expire after 24 hours (configurable). Users need to re-authenticate after expiration. The JWT token payload includes user information, roles, and tenant/subscription status, which is used for authorization checks without requiring database queries on each request.

2. **Platform Access**: This platform is only accessible to administrators. Regular users and employees access the Headoffice system, not this tenant management platform.

3. **Active Subscription and Tenant Requirement**:

   - **Users must have both an active subscription and an active tenant to perform operations** on most endpoints.
   - Authorization checks are performed using information stored in the JWT token payload (tenant and subscription status), eliminating database queries for improved performance.
   - Endpoints that require active subscription and tenant:
     - All billing endpoints (`/api/billing/*`)
     - All backup endpoints (`/api/backup/*`)
     - Most tenant management endpoints (except `POST /api/tenant` for creating a tenant)
   - Endpoints that do NOT require active subscription/tenant:
     - Authentication endpoints (`/api/auth/*`)
     - Subscription plan viewing (`GET /api/subscription/plans`)
     - Subscribing to a plan (`POST /api/subscription/subscribe`)
     - Creating a tenant (`POST /api/tenant`) - requires active subscription but not tenant
     - Admin endpoints (Root Admin only)
   - If a user attempts to access a protected endpoint without an active subscription or tenant, they will receive a `403 Forbidden` response with details about what is missing.
   - **Important**: If a user's subscription or tenant status changes (e.g., subscription expires, tenant is suspended), they will need to re-authenticate to receive an updated token with the current status. Until re-authentication, the token will reflect the status at the time of login/registration.

4. **User Status Check**: The `/api/auth/login` and `/api/auth/register` endpoints now return the same `UserStatusResponse` format as `/api/auth/check`, providing immediate status information after authentication. This eliminates the need for an additional API call to check user status. Use `GET /api/auth/check` to refresh status information at any time. The frontend can use the response from login/register to determine if the user needs to:

   - Subscribe to a plan (if `hasActiveSubscription` is `false`)
   - Create a tenant (if `hasActiveSubscription` is `true` but `hasActiveTenant` is `false`)
   - Proceed with normal operations (if `canPerformOperations` is `true`)

   **Note**: The status information reflects the state at the time of authentication. If status changes occur (e.g., subscription expires), users must re-authenticate to get updated information. All three endpoints (`/api/auth/login`, `/api/auth/register`, and `/api/auth/check`) read information from JWT token claims (no database queries), making them fast and efficient.

5. **Tenant Name Validation**: Tenant names must:

   - Be 3-100 characters long
   - Contain only lowercase letters, numbers, and hyphens
   - Be unique across the system

6. **Subscription Requirements**: Users must have an active subscription before creating a tenant.

7. **Payment Processing**: The payment provider integration is customizable. Update the `PaymentProviderService` implementation to integrate with your payment gateway.

8. **Headoffice Integration**: The system communicates with the Headoffice server for database provisioning and backup/restore operations. Ensure the Headoffice API is properly configured.

9. **Connection String Security**: Connection strings are encrypted at rest. Only root admins can retrieve decrypted connection strings.

---

**Last Updated:** 2024-01-15  
**API Version:** 1.0
