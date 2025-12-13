# Fuel Management Dashboard Components

This directory contains the components for the comprehensive fuel management dashboard.

## Components Overview

### 1. FuelStats
Displays key statistics cards showing:
- Daily Volume (total fuel sold)
- Daily Revenue (total sales)
- Tank Utilization (average level)
- Active Stations (total locations)

**Props:**
- `data`: Object containing all dashboard data

### 2. TankLevelsCard
Shows real-time tank levels with:
- Progress bars for utilization
- Current volume and level
- Available/Used/Capacity breakdown
- Color-coded status indicators

**Props:**
- `data`: Array of tank level objects

### 3. DailySalesChart
Line chart displaying hourly sales trends:
- Volume and revenue over 24 hours
- Peak hour identification
- Dual-axis chart with smooth curves

**Props:**
- `data`: Array of hourly sales data

### 4. StationComparisonChart
Bar chart comparing station performance:
- Volume and revenue by station
- Top performing station highlight
- Side-by-side comparison

**Props:**
- `data`: Array of station performance data

### 5. PaymentMethodChart
Pie chart showing sales distribution by payment method:
- Cash, Credit Card, Mobile Payment breakdown
- Revenue percentages
- Visual distribution with legend

**Props:**
- `data`: Array of payment method data

### 6. MonthlySalesTrend
Line chart displaying monthly trends:
- Volume and revenue growth over months
- Growth rate calculations
- Area charts with gradients

**Props:**
- `data`: Array of monthly sales data

### 7. EmployeePerformanceTable
Table showing employee rankings:
- Performance-based sorting
- Volume and revenue metrics
- Progress bars and badges
- Avatar with initials

**Props:**
- `data`: Array of employee performance data

## Data Structure

The dashboard expects data in the following format:

```javascript
{
  tankLevels: [
    {
      tankId: number,
      stationId: number,
      fuelType: number, // 1=Petrol, 2=Diesel, 3=Premium
      capacity: number,
      currentVolume: number,
      currentLevel: number,
      utilizationPercentage: number
    }
  ],
  dailySales: [
    {
      hour: number, // 0-23
      totalVolume: number,
      totalAmount: number
    }
  ],
  stationsComparison: [
    {
      stationId: number,
      stationName: string,
      totalVolume: number,
      totalAmount: number
    }
  ],
  salesByPaymentMethod: [
    {
      paymentMethod: number, // 1=Cash, 2=Credit Card, 3=Mobile
      totalVolume: number,
      totalAmount: number
    }
  ],
  employeePerformance: [
    {
      employeeId: number,
      fullName: string,
      totalVolume: number,
      totalAmount: number
    }
  ],
  monthlySalesTrend: [
    {
      year: number,
      month: number,
      totalVolume: number,
      totalAmount: number
    }
  ]
}
```

## Features

- **Responsive Design**: All components adapt to different screen sizes
- **Real-time Data**: Supports live data updates
- **Interactive Charts**: Hover tooltips and click interactions
- **Color Coding**: Consistent color scheme for different metrics
- **Performance Indicators**: Visual progress bars and badges
- **Loading States**: Graceful loading with spinners

## Dependencies

- React Bootstrap for UI components
- ECharts for data visualization
- FontAwesome for icons
- Custom theme utilities for colors and styling

## Usage

```javascript
import FuelStats from './components/FuelStats';
import TankLevelsCard from './components/TankLevelsCard';
// ... other imports

const Dashboard = () => {
  const [dashboardData, setDashboardData] = useState(null);
  
  // Fetch data from API
  useEffect(() => {
    // API call here
  }, []);
  
  return (
    <div>
      <FuelStats data={dashboardData} />
      <TankLevelsCard data={dashboardData?.tankLevels} />
      {/* ... other components */}
    </div>
  );
};
```
