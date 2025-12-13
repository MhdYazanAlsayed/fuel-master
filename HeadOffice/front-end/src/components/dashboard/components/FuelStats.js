import React from 'react';
import PropTypes from 'prop-types';
import { Card, Row, Col } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const FuelStatItem = ({ stat }) => {
  return (
    <Col xs={6} md={3} className={stat.className}>
      <div className="text-center">
        <div className="mb-2">
          <FontAwesomeIcon
            icon={stat.icon}
            className={classNames('fs-2', {
              'text-primary': stat.type === 'primary',
              'text-success': stat.type === 'success',
              'text-warning': stat.type === 'warning',
              'text-info': stat.type === 'info'
            })}
          />
        </div>
        <h6 className="pb-1 text-700">{stat.title}</h6>
        <p className="font-sans-serif lh-1 mb-1 fs-3 fw-bold">{stat.amount}</p>
        <div className="d-flex align-items-center justify-content-center">
          <h6 className="fs--1 text-500 mb-0">{stat.subAmount}</h6>
          <h6
            className={classNames('fs--2 ps-2 mb-0', {
              'text-success': stat.trend === 'up',
              'text-danger': stat.trend === 'down',
              'text-primary': stat.trend === 'stable'
            })}
          >
            <FontAwesomeIcon
              icon={
                stat.trend === 'up'
                  ? 'caret-up'
                  : stat.trend === 'down'
                  ? 'caret-down'
                  : 'minus'
              }
              className="me-1"
            />
            {stat.percent}%
          </h6>
        </div>
      </div>
    </Col>
  );
};

FuelStatItem.propTypes = {
  stat: PropTypes.shape({
    title: PropTypes.string,
    amount: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    subAmount: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    type: PropTypes.string,
    trend: PropTypes.string,
    percent: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    icon: PropTypes.string,
    className: PropTypes.string
  })
};

const FuelStats = ({ data }) => {
  const _languageService = DependenciesInjector.services.languageService;

  // Calculate totals from data
  const totalVolume =
    data?.dailySales?.reduce((sum, item) => sum + item.totalVolume, 0) || 0;
  const totalAmount =
    data?.dailySales?.reduce((sum, item) => sum + item.totalAmount, 0) || 0;
  const avgTankUtilization =
    data?.tankLevels?.reduce(
      (sum, tank) => sum + tank.utilizationPercentage,
      0
    ) / (data?.tankLevels?.length || 1) || 0;
  const totalStations = data?.stationsComparison?.length || 0;

  const stats = [
    {
      title: _languageService?.resources.dailyVolume,
      amount: `${totalVolume.toFixed(1)} ${_languageService?.resources.liter}`,
      subAmount: _languageService?.resources.totalFuelSold,
      type: 'primary',
      trend: '',
      percent: '',
      icon: '',
      className: 'border-end border-bottom pb-2'
    },
    {
      title: _languageService?.resources.dailyRevenue,
      amount: `${totalAmount.toFixed(0)} ${_languageService?.resources.rial}`,
      subAmount: _languageService?.resources.totalSales,
      type: '',
      trend: '',
      percent: '',
      icon: '',
      className: 'border-bottom border-end pb-2'
    },
    {
      title: _languageService?.resources.tankUtilization,
      amount: `${avgTankUtilization.toFixed(1)}%`,
      subAmount: _languageService?.resources.averageLevel,
      type: '',
      trend: '',
      percent: '',
      icon: '',
      className: 'border-bottom border-end pb-2'
    },
    {
      title: _languageService?.resources.activeStations,
      amount: totalStations,
      subAmount: _languageService?.resources.totalLocations,
      type: '',
      trend: '',
      percent: '',
      icon: '',
      className: 'border-bottom pb-2'
    }
  ];

  return (
    <Card className="mb-3">
      <Card.Body className="py-4">
        <Row className="g-0">
          {stats.map((stat, index) => (
            <FuelStatItem
              key={stat.title}
              stat={stat}
              index={index}
              lastIndex={stats.length - 1}
            />
          ))}
        </Row>
      </Card.Body>
    </Card>
  );
};

FuelStats.propTypes = {
  data: PropTypes.object
};

export default FuelStats;
