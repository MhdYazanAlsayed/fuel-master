import React, { useState, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import TankLevelsCard from './components/TankLevelsCard';
import DailySalesChart from './components/DailySalesChart';
import StationComparisonChart from './components/StationComparisonChart';
import PaymentMethodChart from './components/PaymentMethodChart';
import EmployeePerformanceTable from './components/EmployeePerformanceTable';
import MonthlySalesTrend from './components/MonthlySalesTrend';
import FuelStats from './components/FuelStats';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _identityService = useService(Services.IdentityService);
  const _reportService = useService(Services.ReportService);

  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);

  // Mock data - replace with actual API call
  useEffect(() => {
    handleGetDataAsync();
  }, []);

  const mapFuelTypeNameToNumber = fuelTypeName => {
    if (!fuelTypeName) return 0;
    const fuelTypes = _languageService?.resources.fuelTypes || {};
    // Find the fuel type number by matching the name
    for (const [key, value] of Object.entries(fuelTypes)) {
      if (value === fuelTypeName) {
        return parseInt(key);
      }
    }
    return 0;
  };

  const mapPaymentMethodToNumber = paymentMethod => {
    if (!paymentMethod) return 0;
    const paymentMethods = _languageService?.resources.paymentMethodTypes || {};
    // Find the payment method number by matching the name
    for (const [key, value] of Object.entries(paymentMethods)) {
      if (value === paymentMethod) {
        return parseInt(key);
      }
    }
    // Fallback mapping for common payment method names
    const lowerMethod = paymentMethod.toLowerCase();
    if (lowerMethod.includes('cash')) return 1;
    if (lowerMethod.includes('mada') || lowerMethod.includes('credit'))
      return 2;
    if (lowerMethod.includes('account') || lowerMethod.includes('mobile'))
      return 3;
    return 0;
  };

  const transformDashboardData = data => {
    if (!data) {
      return {
        tankLevels: [],
        dailySales: [],
        stationsComparison: [],
        salesByPaymentMethod: [],
        employeePerformance: [],
        monthlySalesTrend: []
      };
    }

    return {
      ...data,
      tankLevels: (data.tankLevels || []).map(tank => ({
        ...tank,
        fuelType: mapFuelTypeNameToNumber(tank.fuelTypeName),
        station: tank.station || {
          arabicName: tank.stationName || '',
          englishName: tank.stationName || ''
        },
        // Keep stationName for fallback in component
        stationName: tank.stationName
      })),
      salesByPaymentMethod: (data.salesByPaymentMethod || []).map(payment => ({
        ...payment,
        paymentMethod: mapPaymentMethodToNumber(payment.paymentMethod)
      }))
    };
  };

  const handleGetDataAsync = async () => {
    const response = await _reportService.getDashboardReports();
    console.log(response);
    const transformedData = transformDashboardData(response);
    setDashboardData(transformedData);
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
