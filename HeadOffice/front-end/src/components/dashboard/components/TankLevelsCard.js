import React from 'react';
import PropTypes from 'prop-types';
import { Card, ProgressBar, Badge } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const TankLevelsCard = ({ data }) => {
  const _languageService = DependenciesInjector.services.languageService;
  console.log(data);

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(
      tank =>
        tank &&
        typeof tank.tankId === 'number' &&
        typeof tank.capacity === 'number' &&
        typeof tank.currentVolume === 'number'
    ) || [];

  const getFuelTypeName = fuelType => {
    return _languageService?.resources.fuelTypes[fuelType] || 'Unknown';
  };

  const getFuelTypeColor = fuelType => {
    switch (fuelType) {
      case 1:
        return 'success';
      case 2:
        return 'warning';
      case 3:
        return 'info';
      default:
        return 'secondary';
    }
  };

  const getUtilizationVariant = percentage => {
    if (percentage >= 80) return 'danger';
    if (percentage >= 60) return 'warning';
    return 'success';
  };

  return (
    <Card className="h-100">
      <Card.Header className="bg-light">
        <div className="d-flex justify-content-between align-items-center">
          <h6 className="mb-0">
            <FontAwesomeIcon
              icon="tachometer-alt"
              className="me-2 text-primary"
            />
            {_languageService?.resources.tankLevels}
          </h6>
          <Badge bg="primary" className="fs--2">
            {validData.length} {_languageService?.resources.tanks}
          </Badge>
        </div>
      </Card.Header>
      <Card.Body className="p-3">
        <div className="space-y-3">
          {validData.map(tank => (
            <div key={tank.tankId} className="border-bottom pb-3 mb-3">
              <div className="d-flex justify-content-between align-items-center mb-2">
                <div>
                  <h6 className="mb-1">
                    {`${_languageService.resources.station} ${
                      _languageService.isRTL
                        ? tank.station.arabicName
                        : tank.station.englishName
                    } — ${_languageService?.resources.tank} #${
                      tank.number
                    } — ${getFuelTypeName(tank.fuelType)}`}
                  </h6>
                  <small className="text-muted">
                    {_languageService?.resources.station} #{tank.stationId} •{' '}
                    {_languageService?.resources.capacity}:{' '}
                    {tank.capacity.toLocaleString()}L
                  </small>
                </div>
                <Badge
                  bg={getUtilizationVariant(tank.utilizationPercentage)}
                  className="fs--2"
                >
                  {tank.utilizationPercentage.toFixed(1)}%
                </Badge>
              </div>

              <div className="mb-2">
                <ProgressBar
                  now={tank.utilizationPercentage}
                  variant={getUtilizationVariant(tank.utilizationPercentage)}
                  className="mb-1"
                  style={{ height: '8px' }}
                />
                <div className="d-flex justify-content-between align-items-center">
                  <small className="text-muted">
                    {_languageService?.resources.current}:{' '}
                    {tank.currentVolume.toLocaleString()}{' '}
                    {_languageService?.resources.liter}
                  </small>
                  <small className="text-muted">
                    {_languageService?.resources.level}: {tank.currentLevel}{' '}
                    {' — cm'}
                  </small>
                </div>
              </div>

              <div className="d-flex justify-content-between">
                <div className="text-center">
                  <div className="fs--1 text-muted">
                    {_languageService?.resources.available}
                  </div>
                  <div className="fw-bold text-success">
                    {((tank.capacity - tank.currentVolume) / 1000).toFixed(1)}{' '}
                    {` — KL`}
                  </div>
                </div>
                <div className="text-center">
                  <div className="fs--1 text-muted">
                    {_languageService?.resources.used}
                  </div>
                  <div className="fw-bold text-primary">
                    {(tank.currentVolume / 1000).toFixed(1)} {` — KL`}
                  </div>
                </div>
                <div className="text-center">
                  <div className="fs--1 text-muted">
                    {_languageService?.resources.capacity}
                  </div>
                  <div className="fw-bold">
                    {(tank.capacity / 1000).toFixed(1)} {` — KL`}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        {validData.length === 0 && (
          <div className="text-center py-4">
            <FontAwesomeIcon
              icon="tachometer-alt"
              className="fs-1 text-muted mb-3"
            />
            <p className="text-muted mb-0">
              {_languageService?.resources.noTankDataAvailable}
            </p>
          </div>
        )}
      </Card.Body>
    </Card>
  );
};

TankLevelsCard.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      tankId: PropTypes.number.isRequired,
      stationId: PropTypes.number.isRequired,
      fuelType: PropTypes.number.isRequired,
      capacity: PropTypes.number.isRequired,
      currentVolume: PropTypes.number.isRequired,
      currentLevel: PropTypes.number.isRequired,
      utilizationPercentage: PropTypes.number.isRequired
    })
  ).isRequired
};

export default TankLevelsCard;
