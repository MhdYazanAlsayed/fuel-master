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

  const hours = data.map(item => `${item.hour}:00`);
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
              ? `${param.value.toFixed(1)}L`
              : `${param.value.toFixed(0)} ${_languageService?.resources.rial}`;
          result += `${param.marker} ${param.seriesName}: ${value}<br/>`;
        });
        return result;
      }
    },
    legend: {
      data: ['Volume', 'Revenue'],
      textStyle: { color: getColor('gray-600') },
      top: isMobile ? 5 : 10,
      textStyle: {
        color: getColor('gray-600'),
        fontSize: isMobile ? 10 : 12
      }
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
      data: hours,
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
        name: 'Volume (L)',
        nameTextStyle: {
          color: getColor('gray-600'),
          fontSize: isMobile ? 8 : 10
        },
        axisLine: { show: false },
        axisLabel: {
          color: getColor('gray-600'),
          formatter: '{value}L',
          fontSize: isMobile ? 8 : 10
        },
        splitLine: {
          lineStyle: { color: getColor('gray-200') }
        }
      },
      {
        type: 'value',
        name: 'Revenue ($)',
        nameTextStyle: {
          color: getColor('gray-600'),
          fontSize: isMobile ? 8 : 10
        },
        axisLine: { show: false },
        axisLabel: {
          color: getColor('gray-600'),
          formatter: '${value}',
          fontSize: isMobile ? 8 : 10
        },
        splitLine: { show: false }
      }
    ],
    series: [
      {
        name: 'Volume',
        type: 'line',
        data: volumes,
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
        data: amounts,
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

const DailySalesChart = ({ data }) => {
  const _languageService = useService(Services.LanguageService);
  const chartRef = useRef(null);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(
      item =>
        item &&
        typeof item.hour === 'number' &&
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

  // Safely calculate peak hour
  const peakHour =
    validData.length > 0
      ? validData.reduce(
          (max, item) => (item.totalVolume > max.totalVolume ? item : max),
          validData[0]
        )
      : null;

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
            <FontAwesomeIcon icon="chart-line" className="me-2 text-primary" />
            {_languageService?.resources.dailySalesTrend}
          </h6>
          <div className="text-end">
            <div className="fs--2 text-muted">
              {_languageService?.resources.peakHour}
            </div>
            <div className="fw-bold">
              {peakHour ? `${peakHour.hour}:00` : 'N/A'}
            </div>
          </div>
        </div>
      </Card.Header>
      <Card.Body className="p-3">
        <div className="row mb-3">
          <div className="col-6">
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalVolume}
              </div>
              <div className="fw-bold text-primary">
                {totalVolume.toFixed(1)}L
              </div>
            </div>
          </div>
          <div className="col-6">
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalRevenue}
              </div>
              <div className="fw-bold text-success">
                {totalAmount.toFixed(0)} {_languageService?.resources.rial}
              </div>
            </div>
          </div>
        </div>

        {validData.length > 0 ? (
          <BasicECharts
            ref={chartRef}
            echarts={echarts}
            options={getOptions(validData, isMobile)}
            style={{ height: isMobile ? '200px' : '250px' }}
          />
        ) : (
          <div className="text-center py-4">
            <FontAwesomeIcon
              icon="chart-line"
              className="fs-1 text-muted mb-3"
            />
            <p className="text-muted mb-0">
              {_languageService?.resources.noSaleDataAvailable}
            </p>
          </div>
        )}
      </Card.Body>
    </Card>
  );
};

DailySalesChart.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      hour: PropTypes.number.isRequired,
      totalVolume: PropTypes.number.isRequired,
      totalAmount: PropTypes.number.isRequired
    })
  ).isRequired
};

export default DailySalesChart;
