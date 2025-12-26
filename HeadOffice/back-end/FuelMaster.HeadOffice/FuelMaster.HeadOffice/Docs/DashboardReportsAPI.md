# Dashboard Reports API Documentation

## Overview

The Dashboard Reports API provides comprehensive statistics and analytics for fuel management operations. All reports are automatically filtered based on the authenticated user's scope (ALL, City, Area, Station, or Self).

## Base URL

```
/api/reports/dashboard
```

## Authentication

All endpoints require authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer {token}
```

## User Scopes

Reports are automatically filtered based on the user's scope:

- **ALL**: Admin/CEO - sees reports for all stations in the company
- **City**: City Manager - sees reports for all stations in their city
- **Area**: Area Manager - sees reports for all stations in their area
- **Station/Self**: Station Manager/Employee - sees reports for their specific station only

## Endpoints

### Get Dashboard Reports

Get comprehensive dashboard reports including all statistics.

**Endpoint:** `GET /api/reports/dashboard`

**Request:**
- No request body or query parameters required
- User scope is automatically determined from the JWT token

**Response:**

**Success (200 OK):**
```json
{
  "succeeded": true,
  "message": null,
  "entity": {
    "tankLevels": [
      {
        "tankId": 1,
        "stationId": 1,
        "stationName": "Station Name",
        "number": 1,
        "fuelTypeName": "Gasoline",
        "capacity": 50000.00,
        "currentVolume": 35000.00,
        "currentLevel": 70.00,
        "utilizationPercentage": 70.00
      }
    ],
    "dailySales": [
      {
        "hour": 0,
        "totalVolume": 150.50,
        "totalAmount": 180.60
      },
      {
        "hour": 1,
        "totalVolume": 0.00,
        "totalAmount": 0.00
      }
      // ... hours 0-23
    ],
    "stationsComparison": [
      {
        "stationId": 1,
        "stationName": "Station Name",
        "totalVolume": 5000.00,
        "totalAmount": 6000.00
      }
    ],
    "salesByPaymentMethod": [
      {
        "paymentMethod": "Cash",
        "totalVolume": 3000.00,
        "totalAmount": 3600.00
      },
      {
        "paymentMethod": "Mada",
        "totalVolume": 2000.00,
        "totalAmount": 2400.00
      },
      {
        "paymentMethod": "Account",
        "totalVolume": 1000.00,
        "totalAmount": 1200.00
      }
    ],
    "employeePerformance": [
      {
        "employeeId": 1,
        "fullName": "John Doe",
        "totalVolume": 1500.00,
        "totalAmount": 1800.00
      }
    ],
    "monthlySalesTrend": [
      {
        "year": 2024,
        "month": 1,
        "totalVolume": 50000.00,
        "totalAmount": 60000.00
      },
      {
        "year": 2024,
        "month": 2,
        "totalVolume": 55000.00,
        "totalAmount": 66000.00
      }
      // ... last 12 months
    ]
  }
}
```

**Error (400 Bad Request):**
```json
{
  "succeeded": false,
  "message": "Error message describing what went wrong",
  "entity": null
}
```

## Response Models

### DashboardReportsResult

Main response object containing all dashboard reports.

| Property | Type | Description |
|----------|------|-------------|
| `tankLevels` | `TankLevelReportResult[]` | List of tank level reports |
| `dailySales` | `DailySalesReportResult[]` | List of hourly sales for current day |
| `stationsComparison` | `StationsComparisonReportResult[]` | Comparison of stations by sales |
| `salesByPaymentMethod` | `SalesByPaymentMethodReportResult[]` | Sales grouped by payment method |
| `employeePerformance` | `EmployeePerformanceReportResult[]` | Employee performance metrics |
| `monthlySalesTrend` | `MonthlySalesTrendReportResult[]` | Monthly sales trend (last 12 months) |

### TankLevelReportResult

Tank level and utilization information.

| Property | Type | Description |
|----------|------|-------------|
| `tankId` | `int` | Unique identifier of the tank |
| `stationId` | `int` | Unique identifier of the station |
| `stationName` | `string?` | Name of the station (localized) |
| `number` | `int` | Tank number |
| `fuelTypeName` | `string` | Name of the fuel type (localized) |
| `capacity` | `decimal` | Maximum capacity of the tank (liters) |
| `currentVolume` | `decimal` | Current volume in the tank (liters) |
| `currentLevel` | `decimal` | Current level in the tank (cm) |
| `utilizationPercentage` | `decimal` | Percentage of capacity utilized (0-100) |

### DailySalesReportResult

Hourly sales data for the current day.

| Property | Type | Description |
|----------|------|-------------|
| `hour` | `int` | Hour of the day (0-23) |
| `totalVolume` | `decimal` | Total volume sold in this hour (liters) |
| `totalAmount` | `decimal` | Total amount sold in this hour (currency) |

**Note:** All 24 hours (0-23) are included in the response, with zero values for hours with no sales.

### StationsComparisonReportResult

Station comparison by total sales.

| Property | Type | Description |
|----------|------|-------------|
| `stationId` | `int?` | Unique identifier of the station |
| `stationName` | `string` | Name of the station (localized) |
| `totalVolume` | `decimal` | Total volume sold (liters) |
| `totalAmount` | `decimal` | Total amount sold (currency) |

**Note:** Results are ordered by `totalAmount` in descending order.

### SalesByPaymentMethodReportResult

Sales grouped by payment method.

| Property | Type | Description |
|----------|------|-------------|
| `paymentMethod` | `string` | Payment method name (Cash, Mada, Account, Undefined) |
| `totalVolume` | `decimal` | Total volume sold with this payment method (liters) |
| `totalAmount` | `decimal` | Total amount sold with this payment method (currency) |

**Note:** Results are ordered by `totalAmount` in descending order.

### EmployeePerformanceReportResult

Employee performance metrics.

| Property | Type | Description |
|----------|------|-------------|
| `employeeId` | `int` | Unique identifier of the employee |
| `fullName` | `string` | Full name of the employee |
| `totalVolume` | `decimal` | Total volume sold by employee (liters) |
| `totalAmount` | `decimal` | Total amount sold by employee (currency) |

**Note:** Results are ordered by `totalAmount` in descending order.

### MonthlySalesTrendReportResult

Monthly sales trend for the last 12 months.

| Property | Type | Description |
|----------|------|-------------|
| `year` | `int` | Year of the sales data |
| `month` | `int` | Month of the sales data (1-12) |
| `totalVolume` | `decimal` | Total volume sold in this month (liters) |
| `totalAmount` | `decimal` | Total amount sold in this month (currency) |

**Note:** Results are ordered by year and month in ascending order.

## Example Requests

### cURL

```bash
curl -X GET "https://api.example.com/api/reports/dashboard" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json"
```

### JavaScript (Fetch)

```javascript
const response = await fetch('https://api.example.com/api/reports/dashboard', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});

const data = await response.json();
if (data.succeeded) {
  console.log('Dashboard Reports:', data.entity);
} else {
  console.error('Error:', data.message);
}
```

### C# (HttpClient)

```csharp
using var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync("https://api.example.com/api/reports/dashboard");
var result = await response.Content.ReadFromJsonAsync<ResultDto<DashboardReportsResult>>();

if (result.Succeeded)
{
    var reports = result.Entity;
    // Use reports...
}
```

## Error Handling

The API uses standard HTTP status codes:

- **200 OK**: Request successful
- **400 Bad Request**: Invalid request or error occurred
- **401 Unauthorized**: Missing or invalid authentication token

Error responses include a `message` field describing what went wrong.

## Notes

1. **Localization**: Station names and fuel type names are automatically localized based on the current culture (Arabic/English).

2. **Scope Filtering**: All reports are automatically filtered based on the user's scope. No additional filtering parameters are needed.

3. **Performance**: The endpoint aggregates data from multiple sources. For large datasets, response times may vary.

4. **Data Range**:
   - **Daily Sales**: Current day only (24 hours)
   - **Monthly Trend**: Last 12 months from current date
   - **Other Reports**: All-time data (filtered by user scope)

5. **Empty Results**: If no data is available for a particular report section, an empty array is returned.

## Version

API Version: 1.0

Last Updated: 2024

