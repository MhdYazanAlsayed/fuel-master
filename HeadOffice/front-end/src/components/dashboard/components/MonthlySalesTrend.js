import React, { useRef, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import BasicECharts from 'components/theme/common/BasicEChart';
import { LineChart } from 'echarts/charts';
import {
  GridComponent,
  TitleComponent,
  TooltipComponent,
  LegendComponent
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
  LegendComponent,
  LineChart,
  CanvasRenderer
]);

const getOptions = (data, isMobile = false) => {
  // Handle empty or invalid data
  if (!data || data.length === 0) {
    return {
      tooltip: { show: false },
      xAxis: { data: [] },
      yAxis: [{ data: [] }],
      series: []
    };
  }

  const months = data.map(item => {
    const date = new Date(item.year, item.month - 1);
    return date.toLocaleDateString('en-US', {
      month: 'short',
      year: '2-digit'
    });
  });
  const volumes = data.map(item => item.totalVolume);
  const amounts = data.map(item => item.totalAmount);

  return {
    tooltip: {
      trigger: 'axis',
      padding: [7, 10],
      backgroundColor: getColor('gray-100'),
      borderColor: getColor('gray-300'),
      textStyle: { color: getColor('dark') },
      borderWidth: 1,
      formatter: function (params) {
        let result = `<strong>${params[0].axisValue}</strong><br/>`;
        params.forEach(param => {
          const value =
            param.seriesName === 'Volume'
              ? `${(param.value / 1000).toFixed(1)}kL`
              : `$${(param.value / 1000).toFixed(1)}k`;
          result += `${param.marker} ${param.seriesName}: ${value}<br/>`;
        });
        return result;
      }
    },
    legend: {
      data: ['Volume', 'Revenue'],
      textStyle: {
        color: getColor('gray-600'),
        fontSize: isMobile ? 10 : 12
      },
      top: isMobile ? 5 : 10
    },
    grid: {
      left: isMobile ? '8%' : '3%',
      right: isMobile ? '8%' : '4%',
      bottom: isMobile ? '8%' : '3%',
      top: isMobile ? '20%' : '15%',
      containLabel: true
    },
    xAxis: {
      type: 'category',
      data: months,
      axisLine: {
        lineStyle: { color: getColor('gray-300') }
      },
      axisLabel: {
        color: getColor('gray-600'),
        fontSize: isMobile ? 8 : 10,
        rotate: isMobile ? 45 : 0
      }
    },
    yAxis: [
      {
        type: 'value',
        name: 'Volume (kL)',
        nameTextStyle: {
          color: getColor('gray-600'),
          fontSize: isMobile ? 8 : 10
        },
        axisLine: { show: false },
        axisLabel: {
          color: getColor('gray-600'),
          formatter: '{value}kL',
          fontSize: isMobile ? 8 : 10
        },
        splitLine: {
          lineStyle: { color: getColor('gray-200') }
        }
      },
      {
        type: 'value',
        name: 'Revenue (k$)',
        nameTextStyle: {
          color: getColor('gray-600'),
          fontSize: isMobile ? 8 : 10
        },
        axisLine: { show: false },
        axisLabel: {
          color: getColor('gray-600'),
          formatter: '${value}k',
          fontSize: isMobile ? 8 : 10
        },
        splitLine: { show: false }
      }
    ],
    series: [
      {
        name: 'Volume',
        type: 'line',
        data: volumes.map(v => v / 1000),
        smooth: true,
        lineStyle: {
          color: getColor('primary'),
          width: isMobile ? 2 : 3
        },
        itemStyle: {
          color: getColor('primary')
        },
        areaStyle: {
          color: {
            type: 'linear',
            x: 0,
            y: 0,
            x2: 0,
            y2: 1,
            colorStops: [
              { offset: 0, color: getColor('primary') },
              { offset: 1, color: getColor('gray-100') }
            ]
          }
        }
      },
      {
        name: 'Revenue',
        type: 'line',
        yAxisIndex: 1,
        data: amounts.map(a => a / 1000),
        smooth: true,
        lineStyle: {
          color: getColor('success'),
          width: isMobile ? 2 : 3
        },
        itemStyle: {
          color: getColor('success')
        },
        areaStyle: {
          color: {
            type: 'linear',
            x: 0,
            y: 0,
            x2: 0,
            y2: 1,
            colorStops: [
              { offset: 0, color: getColor('success') },
              { offset: 1, color: getColor('gray-100') }
            ]
          }
        }
      }
    ]
  };
};

const MonthlySalesTrend = ({ data }) => {
  const _languageService = useService(Services.LanguageService);
  const chartRef = useRef(null);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(
      item =>
        item &&
        typeof item.year === 'number' &&
        typeof item.month === 'number' &&
        typeof item.totalVolume === 'number' &&
        typeof item.totalAmount === 'number'
    ) || [];

  const totalVolume = validData.reduce(
    (sum, item) => sum + item.totalVolume,
    0
  );
  const totalAmount = validData.reduce(
    (sum, item) => sum + item.totalAmount,
    0
  );
  const avgMonthlyVolume =
    validData.length > 0 ? totalVolume / validData.length : 0;
  const avgMonthlyRevenue =
    validData.length > 0 ? totalAmount / validData.length : 0;

  // Calculate growth rate safely
  let volumeGrowth = '0.0';
  let revenueGrowth = '0.0';

  if (validData.length >= 2) {
    const firstMonth = validData[0];
    const lastMonth = validData[validData.length - 1];

    if (firstMonth.totalVolume > 0) {
      volumeGrowth = (
        ((lastMonth.totalVolume - firstMonth.totalVolume) /
          firstMonth.totalVolume) *
        100
      ).toFixed(1);
    }

    if (firstMonth.totalAmount > 0) {
      revenueGrowth = (
        ((lastMonth.totalAmount - firstMonth.totalAmount) /
          firstMonth.totalAmount) *
        100
      ).toFixed(1);
    }
  }

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
    <Card className="chart-card dashboard-chart">
      <Card.Header className="bg-light">
        <div className="d-flex justify-content-between align-items-center">
          <h6 className="mb-0">
            <FontAwesomeIcon icon="chart-line" className="me-2 text-primary" />
            {_languageService?.resources.monthlySalesTrend}
          </h6>
          <div className={`d-flex ${isMobile ? 'flex-column gap-2' : 'gap-3'}`}>
            {/* <div className="text-end">
              <div className="fs--2 text-muted">
                {_languageService?.resources.volumeGrowth}
              </div>
              <div
                className={`fw-bold ${
                  volumeGrowth >= 0 ? 'text-success' : 'text-danger'
                }`}
              >
                {volumeGrowth >= 0 ? '+' : ''}
                {volumeGrowth}%
              </div>
            </div>
            <div className="text-end">
              <div className="fs--2 text-muted">
                {_languageService?.resources.revenueGrowth}
              </div>
              <div
                className={`fw-bold ${
                  revenueGrowth >= 0 ? 'text-success' : 'text-danger'
                }`}
              >
                {revenueGrowth >= 0 ? '+' : ''}
                {revenueGrowth}%
              </div>
            </div> */}
          </div>
        </div>
      </Card.Header>
      <Card.Body className="p-3">
        <div className={`row mb-3 ${isMobile ? 'g-2' : ''}`}>
          <div className={`${isMobile ? 'col-6' : 'col-md-3'}`}>
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalVolume}
              </div>
              <div className="fw-bold text-primary">
                {(totalVolume / 1000).toFixed(1)}kL
              </div>
            </div>
          </div>
          <div className={`${isMobile ? 'col-6' : 'col-md-3'}`}>
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalRevenue}
              </div>
              <div className="fw-bold text-success">
                {(totalAmount / 1000).toFixed(1)}K{' '}
                {_languageService?.resources.rial}
              </div>
            </div>
          </div>
          <div className={`${isMobile ? 'col-6' : 'col-md-3'}`}>
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.avgMonthlyVolume}
              </div>
              <div className="fw-bold text-info">
                {(avgMonthlyVolume / 1000).toFixed(1)}kL
              </div>
            </div>
          </div>
          <div className={`${isMobile ? 'col-6' : 'col-md-3'}`}>
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.avgMonthlyRevenue}
              </div>
              <div className="fw-bold text-warning">
                {(avgMonthlyRevenue / 1000).toFixed(1)}K{' '}
                {_languageService?.resources.rial}
              </div>
            </div>
          </div>
        </div>

        {validData.length > 0 ? (
          <BasicECharts
            ref={chartRef}
            echarts={echarts}
            options={getOptions(validData, isMobile)}
            style={{ height: isMobile ? '250px' : '300px' }}
          />
        ) : (
          <div className="text-center py-4">
            <FontAwesomeIcon
              icon="chart-area"
              className="fs-1 text-muted mb-3"
            />
            <p className="text-muted mb-0">
              {_languageService?.resources.noMonthlyDataAvailable}
            </p>
          </div>
        )}
      </Card.Body>
    </Card>
  );
};

MonthlySalesTrend.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      year: PropTypes.number.isRequired,
      month: PropTypes.number.isRequired,
      totalVolume: PropTypes.number.isRequired,
      totalAmount: PropTypes.number.isRequired
    })
  ).isRequired
};

export default MonthlySalesTrend;
