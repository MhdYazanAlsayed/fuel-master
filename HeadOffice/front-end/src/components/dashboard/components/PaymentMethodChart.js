import React, { useRef, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Card, Row, Col } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import BasicECharts from 'components/theme/common/BasicEChart';
import { PieChart } from 'echarts/charts';
import {
  GridComponent,
  TitleComponent,
  TooltipComponent
} from 'echarts/components';
import * as echarts from 'echarts/core';
import { CanvasRenderer } from 'echarts/renderers';
import { getColor } from 'helpers/utils';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

echarts.use([
  TitleComponent,
  TooltipComponent,
  GridComponent,
  PieChart,
  CanvasRenderer
]);

const getOptions = (data, isMobile = false) => {
  // Handle empty or invalid data
  if (!data || data.length === 0) {
    return {
      tooltip: { show: false },
      series: [{ data: [] }]
    };
  }

  const chartData = data.map((item, index) => ({
    name: getPaymentMethodName(item.paymentMethod),
    value: item.totalAmount,
    color: getPaymentMethodColor(item.paymentMethod)
  }));

  return {
    color: chartData.map(item => getColor(item.color)),
    tooltip: {
      padding: [7, 10],
      transitionDuration: 0,
      backgroundColor: getColor('gray-100'),
      borderColor: getColor('gray-300'),
      textStyle: { color: getColor('dark') },
      borderWidth: 1,
      formatter: params =>
        `<strong>${params.data.name}:</strong> $${params.value.toFixed(0)} (${
          params.percent
        }%)`
    },
    series: [
      {
        name: 'Payment Methods',
        type: 'pie',
        radius: isMobile ? ['35%', '60%'] : ['40%', '70%'],
        center: isMobile ? ['50%', '50%'] : ['50%', '50%'],
        avoidLabelOverlap: false,
        emphasis: {
          scale: false
        },
        itemStyle: {
          borderWidth: 2,
          borderColor: getColor('gray-100')
        },
        label: {
          show: false
        },
        data: chartData
      }
    ]
  };
};

const getPaymentMethodName = paymentMethod => {
  const _languageService = useService(Services.LanguageService);
  return (
    _languageService?.resources.paymentMethodTypes[paymentMethod] || 'Other'
  );
};

const getPaymentMethodColor = paymentMethod => {
  switch (paymentMethod) {
    case 1:
      return 'success';
    case 2:
      return 'primary';
    case 3:
      return 'warning';
    default:
      return 'secondary';
  }
};

const getPaymentMethodIcon = paymentMethod => {
  switch (paymentMethod) {
    case 1:
      return 'money-bill-wave';
    case 2:
      return 'credit-card';
    case 3:
      return 'mobile';
    default:
      return 'question-circle';
  }
};

const PaymentMethodChart = ({ data }) => {
  const _languageService = useService(Services.LanguageService);
  const chartRef = useRef(null);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(
      item =>
        item &&
        typeof item.paymentMethod === 'number' &&
        typeof item.totalVolume === 'number' &&
        typeof item.totalAmount === 'number'
    ) || [];

  const totalAmount = validData.reduce(
    (sum, item) => sum + item.totalAmount,
    0
  );
  const totalVolume = validData.reduce(
    (sum, item) => sum + item.totalVolume,
    0
  );

  // Handle window resize
  useEffect(() => {
    const handleResize = () => {
      const mobile = window.innerWidth < 768;
      setIsMobile(mobile);

      // Update chart options on resize
      if (chartRef.current && validData.length > 0) {
        const chartInstance = chartRef.current.getEchartsInstance();
        if (chartInstance) {
          chartInstance.setOption(getOptions(validData, mobile));
        }
      }
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, [validData]);

  return (
    <Card className="h-100 chart-card dashboard-chart">
      <Card.Header className="bg-light">
        <div className="d-flex justify-content-between align-items-center">
          <h6 className="mb-0">
            <FontAwesomeIcon icon="credit-card" className="me-2 text-primary" />
            {_languageService?.resources.paymentMethod}
          </h6>
          <div className="text-end">
            <div className="fs--2 text-muted">
              {_languageService?.resources.totalRevenue}
            </div>
            <div className="fw-bold">
              {totalAmount.toFixed(0)} {_languageService?.resources.rial}
            </div>
          </div>
        </div>
      </Card.Header>
      <Card.Body className="p-3">
        <Row className="justify-content-between g-0">
          <Col
            xs={isMobile ? 12 : 5}
            sm={isMobile ? 12 : 6}
            xxl={isMobile ? 12 : undefined}
            className={isMobile ? 'mb-3' : 'pe-2'}
          >
            <h6 className="mt-1 mb-3">
              {_languageService?.resources.salesDistribution}
            </h6>
            {validData.map((item, index) => {
              const percentage =
                totalAmount > 0
                  ? ((item.totalAmount * 100) / totalAmount).toFixed(1)
                  : '0.0';
              return (
                <div
                  key={item.paymentMethod}
                  className={`d-flex align-items-center justify-content-between fw-semi-bold fs--2 ${
                    index === 0 ? 'mt-3' : 'mt-2'
                  }`}
                >
                  <div className="d-flex align-items-center">
                    <FontAwesomeIcon
                      icon={getPaymentMethodIcon(item.paymentMethod)}
                      className={`me-2 text-${getPaymentMethodColor(
                        item.paymentMethod
                      )}`}
                    />
                    {getPaymentMethodName(item.paymentMethod)}
                  </div>
                  <div className="d-flex align-items-center">
                    <span className="me-2">{percentage}%</span>
                    <span className="text-muted">
                      {item.totalAmount.toFixed(0)}{' '}
                      {_languageService?.resources.rial}
                    </span>
                  </div>
                </div>
              );
            })}
          </Col>
          {!isMobile && (
            <Col xs="auto">
              <div className="ps-0">
                {validData.length > 0 ? (
                  <BasicECharts
                    ref={chartRef}
                    echarts={echarts}
                    options={getOptions(validData, isMobile)}
                    style={{ width: '8rem', height: '8rem' }}
                  />
                ) : (
                  <div className="text-center">
                    <FontAwesomeIcon
                      icon="credit-card"
                      className="fs-1 text-muted"
                    />
                    <p className="text-muted fs--2 mt-2">
                      {_languageService?.resources.noDataAvailable}
                    </p>
                  </div>
                )}
              </div>
            </Col>
          )}
        </Row>

        {isMobile && validData.length > 0 && (
          <div className="mt-3">
            <BasicECharts
              ref={chartRef}
              echarts={echarts}
              options={getOptions(validData, isMobile)}
              style={{ width: '100%', height: '200px' }}
            />
          </div>
        )}

        <div className="mt-3 pt-3 border-top">
          <div className="row text-center">
            <div className="col-6">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalVolume}
              </div>
              <div className="fw-bold text-primary">
                {(totalVolume / 1000).toFixed(1)} kL
              </div>
            </div>
            <div className="col-6">
              <div className="fs--2 text-muted">
                {_languageService?.resources.avgTransaction}
              </div>
              <div className="fw-bold text-success">
                {validData.length > 0
                  ? (totalAmount / validData.length).toFixed(0)
                  : '0'}{' '}
                {_languageService?.resources.rial}
              </div>
            </div>
          </div>
        </div>
      </Card.Body>
    </Card>
  );
};

PaymentMethodChart.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      paymentMethod: PropTypes.number.isRequired,
      totalVolume: PropTypes.number.isRequired,
      totalAmount: PropTypes.number.isRequired
    })
  ).isRequired
};

export default PaymentMethodChart;
