import React, { useRef, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import BasicECharts from 'components/theme/common/BasicEChart';
import { BarChart } from 'echarts/charts';
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
  BarChart,
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

  const stationNames = data.map(item => item.stationName);
  const volumes = data.map(item => item.totalVolume);
  const amounts = data.map(item => item.totalAmount);

  return {
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow'
      },
      padding: [7, 10],
      backgroundColor: getColor('gray-100'),
      borderColor: getColor('gray-300'),
      textStyle: { color: getColor('dark') },
      borderWidth: 1,
      formatter: function (params) {
        let result = `<strong>${params[0].name}</strong><br/>`;
        params.forEach(param => {
          const value =
            param.seriesName === 'Volume'
              ? `${param.value.toFixed(1)}L`
              : `${param.value.toFixed(0)}`;
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
      bottom: isMobile ? '15%' : '3%',
      top: isMobile ? '20%' : '15%',
      containLabel: true
    },
    xAxis: {
      type: 'category',
      data: stationNames,
      axisLine: {
        lineStyle: { color: getColor('gray-300') }
      },
      axisLabel: {
        color: getColor('gray-600'),
        fontSize: isMobile ? 8 : 10,
        rotate: isMobile ? 45 : 45,
        interval: isMobile ? 0 : 'auto'
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
        type: 'bar',
        data: volumes,
        itemStyle: {
          color: getColor('primary'),
          borderRadius: [4, 4, 0, 0]
        },
        barWidth: isMobile ? '30%' : '40%'
      },
      {
        name: 'Revenue',
        type: 'bar',
        yAxisIndex: 1,
        data: amounts,
        itemStyle: {
          color: getColor('success'),
          borderRadius: [4, 4, 0, 0]
        },
        barWidth: isMobile ? '30%' : '40%'
      }
    ]
  };
};

const StationComparisonChart = ({ data }) => {
  const _languageService = useService(Services.LanguageService);
  const chartRef = useRef(null);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(
      item =>
        item &&
        item.stationName &&
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

  // Safely calculate top station
  const topStation =
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
            <FontAwesomeIcon icon="building" className="me-2 text-primary" />
            {_languageService?.resources.stationPerformance}
          </h6>
          <div className="text-end">
            <div className="fs--2 text-muted">
              {_languageService?.resources.topStation}
            </div>
            <div className="fw-bold">
              {topStation ? topStation.stationName : 'N/A'}
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
                {(totalVolume / 1000).toFixed(1)}kL
              </div>
            </div>
          </div>
          <div className="col-6">
            <div className="text-center">
              <div className="fs--2 text-muted">
                {_languageService?.resources.totalRevenue}
              </div>
              <div className="fw-bold text-success">
                {(totalAmount / 1000).toFixed(1)}k{' '}
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
            style={{ height: isMobile ? '200px' : '250px' }}
          />
        ) : (
          <div className="text-center py-4">
            <FontAwesomeIcon icon="building" className="fs-1 text-muted mb-3" />
            <p className="text-muted mb-0">No station data available</p>
          </div>
        )}
      </Card.Body>
    </Card>
  );
};

StationComparisonChart.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      stationId: PropTypes.number,
      stationName: PropTypes.string.isRequired,
      totalVolume: PropTypes.number.isRequired,
      totalAmount: PropTypes.number.isRequired
    })
  ).isRequired
};

export default StationComparisonChart;
