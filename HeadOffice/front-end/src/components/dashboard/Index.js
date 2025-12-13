import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React, { useState, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import TankLevelsCard from './components/TankLevelsCard';
import DailySalesChart from './components/DailySalesChart';
import StationComparisonChart from './components/StationComparisonChart';
import PaymentMethodChart from './components/PaymentMethodChart';
import EmployeePerformanceTable from './components/EmployeePerformanceTable';
import MonthlySalesTrend from './components/MonthlySalesTrend';
import FuelStats from './components/FuelStats';

const _languageService = DependenciesInjector.services.languageService;
const _identityService = DependenciesInjector.services.identityService;
const _reportService = DependenciesInjector.services.reportService;

const Index = () => {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);

  // Mock data - replace with actual API call
  useEffect(() => {
    // Simulate API call
    // setTimeout(() => {
    //   const mockData = {
    //     tankLevels: [
    //       {
    //         tankId: 1,
    //         stationId: 1,
    //         fuelType: 1,
    //         capacity: 50000.0,
    //         currentVolume: 35000.0,
    //         currentLevel: 175.5,
    //         utilizationPercentage: 70.0
    //       },
    //       {
    //         tankId: 2,
    //         stationId: 1,
    //         fuelType: 2,
    //         capacity: 40000.0,
    //         currentVolume: 28000.0,
    //         currentLevel: 140.25,
    //         utilizationPercentage: 70.0
    //       },
    //       {
    //         tankId: 3,
    //         stationId: 2,
    //         fuelType: 1,
    //         capacity: 60000.0,
    //         currentVolume: 45000.0,
    //         currentLevel: 225.75,
    //         utilizationPercentage: 75.0
    //       }
    //     ],
    //     dailySales: [
    //       {
    //         hour: 6,
    //         totalVolume: 1250.5,
    //         totalAmount: 1875.75
    //       },
    //       {
    //         hour: 7,
    //         totalVolume: 2100.25,
    //         totalAmount: 3150.38
    //       },
    //       {
    //         hour: 8,
    //         totalVolume: 1850.75,
    //         totalAmount: 2776.13
    //       },
    //       {
    //         hour: 9,
    //         totalVolume: 1650.0,
    //         totalAmount: 2475.0
    //       },
    //       {
    //         hour: 10,
    //         totalVolume: 1950.5,
    //         totalAmount: 2925.75
    //       },
    //       {
    //         hour: 11,
    //         totalVolume: 2200.25,
    //         totalAmount: 3300.38
    //       },
    //       {
    //         hour: 12,
    //         totalVolume: 2400.75,
    //         totalAmount: 3601.13
    //       },
    //       {
    //         hour: 13,
    //         totalVolume: 2100.0,
    //         totalAmount: 3150.0
    //       },
    //       {
    //         hour: 14,
    //         totalVolume: 1950.5,
    //         totalAmount: 2925.75
    //       },
    //       {
    //         hour: 15,
    //         totalVolume: 2250.25,
    //         totalAmount: 3375.38
    //       },
    //       {
    //         hour: 16,
    //         totalVolume: 2400.75,
    //         totalAmount: 3601.13
    //       },
    //       {
    //         hour: 17,
    //         totalVolume: 2600.0,
    //         totalAmount: 3900.0
    //       },
    //       {
    //         hour: 18,
    //         totalVolume: 2800.5,
    //         totalAmount: 4200.75
    //       },
    //       {
    //         hour: 19,
    //         totalVolume: 2500.25,
    //         totalAmount: 3750.38
    //       },
    //       {
    //         hour: 20,
    //         totalVolume: 2200.75,
    //         totalAmount: 3301.13
    //       },
    //       {
    //         hour: 21,
    //         totalVolume: 1800.0,
    //         totalAmount: 2700.0
    //       },
    //       {
    //         hour: 22,
    //         totalVolume: 1500.5,
    //         totalAmount: 2250.75
    //       },
    //       {
    //         hour: 23,
    //         totalVolume: 1200.25,
    //         totalAmount: 1800.38
    //       }
    //     ],
    //     stationsComparison: [
    //       {
    //         stationId: 1,
    //         stationName: 'Main Station',
    //         totalVolume: 45000.75,
    //         totalAmount: 67501.13
    //       },
    //       {
    //         stationId: 2,
    //         stationName: 'Downtown Station',
    //         totalVolume: 38000.5,
    //         totalAmount: 57000.75
    //       },
    //       {
    //         stationId: 3,
    //         stationName: 'Highway Station',
    //         totalVolume: 32000.25,
    //         totalAmount: 48000.38
    //       },
    //       {
    //         stationId: null,
    //         stationName: 'Unknown Station',
    //         totalVolume: 1500.0,
    //         totalAmount: 2250.0
    //       }
    //     ],
    //     salesByPaymentMethod: [
    //       {
    //         paymentMethod: 1,
    //         totalVolume: 65000.5,
    //         totalAmount: 97500.75
    //       },
    //       {
    //         paymentMethod: 2,
    //         totalVolume: 45000.25,
    //         totalAmount: 67500.38
    //       },
    //       {
    //         paymentMethod: 3,
    //         totalVolume: 15000.75,
    //         totalAmount: 22501.13
    //       }
    //     ],
    //     employeePerformance: [
    //       {
    //         employeeId: 1,
    //         fullName: 'Ahmed Hassan',
    //         totalVolume: 25000.5,
    //         totalAmount: 37500.75
    //       },
    //       {
    //         employeeId: 2,
    //         fullName: 'Mohammed Ali',
    //         totalVolume: 22000.25,
    //         totalAmount: 33000.38
    //       },
    //       {
    //         employeeId: 3,
    //         fullName: 'Fatima Zahra',
    //         totalVolume: 20000.75,
    //         totalAmount: 30001.13
    //       },
    //       {
    //         employeeId: 4,
    //         fullName: 'Omar Khalil',
    //         totalVolume: 18000.0,
    //         totalAmount: 27000.0
    //       },
    //       {
    //         employeeId: 5,
    //         fullName: 'Aisha Rahman',
    //         totalVolume: 15000.5,
    //         totalAmount: 22500.75
    //       }
    //     ],
    //     monthlySalesTrend: [
    //       {
    //         year: 2023,
    //         month: 2,
    //         totalVolume: 180000.5,
    //         totalAmount: 270000.75
    //       },
    //       {
    //         year: 2023,
    //         month: 3,
    //         totalVolume: 195000.25,
    //         totalAmount: 292500.38
    //       },
    //       {
    //         year: 2023,
    //         month: 4,
    //         totalVolume: 210000.75,
    //         totalAmount: 315001.13
    //       },
    //       {
    //         year: 2023,
    //         month: 5,
    //         totalVolume: 225000.0,
    //         totalAmount: 337500.0
    //       },
    //       {
    //         year: 2023,
    //         month: 6,
    //         totalVolume: 240000.5,
    //         totalAmount: 360000.75
    //       },
    //       {
    //         year: 2023,
    //         month: 7,
    //         totalVolume: 255000.25,
    //         totalAmount: 382500.38
    //       },
    //       {
    //         year: 2023,
    //         month: 8,
    //         totalVolume: 270000.75,
    //         totalAmount: 405001.13
    //       },
    //       {
    //         year: 2023,
    //         month: 9,
    //         totalVolume: 285000.0,
    //         totalAmount: 427500.0
    //       },
    //       {
    //         year: 2023,
    //         month: 10,
    //         totalVolume: 300000.5,
    //         totalAmount: 450000.75
    //       },
    //       {
    //         year: 2023,
    //         month: 11,
    //         totalVolume: 315000.25,
    //         totalAmount: 472500.38
    //       },
    //       {
    //         year: 2023,
    //         month: 12,
    //         totalVolume: 330000.75,
    //         totalAmount: 495001.13
    //       },
    //       {
    //         year: 2024,
    //         month: 1,
    //         totalVolume: 345000.0,
    //         totalAmount: 517500.0
    //       }
    //     ]
    //   };
    //   setDashboardData(mockData);
    //   setLoading(false);
    // }, 1000);
    handleGetDataAsync();
  }, []);

  const handleGetDataAsync = async () => {
    const response = await _reportService.getDashboardReports();
    setDashboardData(response);
    setLoading(false);
  };

  if (loading) {
    return (
      <div
        className="d-flex justify-content-center align-items-center"
        style={{ height: '400px' }}
      >
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="overflow-hidden dashboard-grid">
      <div className="mb-4">
        <h4 className="mb-2">
          {_languageService?.resources.welcome.replace(
            '{USERNAME}',
            _identityService.currentUser.fullName ??
              _identityService.currentUser.userName
          )}
        </h4>
        <p className="text-muted">
          {_languageService?.resources.fuelManagementDashboard}
        </p>
      </div>

      {/* Stats Cards */}
      <Row className="g-3 mb-3 stats-cards">
        <Col xs={12}>
          <FuelStats data={dashboardData} />
        </Col>
      </Row>

      {/* Tank Levels and Daily Sales */}
      <Row className="g-3 mb-3">
        <Col xs={12} lg={6}>
          <TankLevelsCard data={dashboardData.tankLevels} />
        </Col>
        <Col xs={12} lg={6}>
          <DailySalesChart data={dashboardData.dailySales} />
        </Col>
      </Row>

      {/* Station Comparison and Payment Methods */}
      <Row className="g-3 mb-3">
        <Col md={12}>
          <StationComparisonChart data={dashboardData.stationsComparison} />
        </Col>
        <Col md={12}>
          <PaymentMethodChart data={dashboardData.salesByPaymentMethod} />
        </Col>
      </Row>

      {/* Monthly Sales Trend */}
      <Row className="g-3 mb-3">
        <Col xs={12}>
          <MonthlySalesTrend data={dashboardData.monthlySalesTrend} />
        </Col>
      </Row>

      {/* Employee Performance */}
      <Row className="g-3 mb-3">
        <Col xs={12}>
          <EmployeePerformanceTable data={dashboardData.employeePerformance} />
        </Col>
      </Row>
    </div>
  );
};

export default Index;
