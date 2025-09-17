import React from 'react';
import PropTypes from 'prop-types';
import { Card, Table, Badge } from 'react-bootstrap';
import Avatar from 'components/theme/common/Avatar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const EmployeePerformanceTable = ({ data }) => {
  const _languageService = DependenciesInjector.services.languageService;

  // Ensure data is valid and filter out invalid entries
  const validData =
    data?.filter(emp => emp && emp.fullName && emp.employeeId) || [];

  // Sort employees by total amount (performance)
  const sortedData = [...validData].sort(
    (a, b) => b.totalAmount - a.totalAmount
  );

  const getPerformanceColor = rank => {
    if (rank === 1) return 'success';
    if (rank === 2) return 'primary';
    if (rank === 3) return 'warning';
    return 'secondary';
  };

  const getPerformanceIcon = rank => {
    if (rank === 1) return 'trophy';
    if (rank === 2) return 'medal';
    if (rank === 3) return 'award';
    return 'user';
  };

  const getInitials = fullName => {
    if (!fullName || typeof fullName !== 'string') {
      return 'U'; // Default initial for unknown/empty names
    }

    const initials = fullName
      .split(' ')
      .map(name => name.charAt(0))
      .join('')
      .toUpperCase()
      .slice(0, 2);

    return initials || 'U'; // Return 'U' if no initials were extracted
  };

  const totalTeamVolume = validData.reduce(
    (sum, emp) => sum + emp.totalVolume,
    0
  );
  const totalTeamRevenue = validData.reduce(
    (sum, emp) => sum + emp.totalAmount,
    0
  );

  return (
    <Card>
      <Card.Header className="bg-light">
        <div className="d-flex justify-content-between align-items-center">
          <h6 className="mb-0">
            <FontAwesomeIcon icon="users" className="me-2 text-primary" />
            {_languageService?.resources.employeePerformance}
          </h6>
          <div className="d-flex gap-3">
            <div className="text-end">
              <div className="fs--2 text-muted">
                {_languageService?.resources.teamVolume}
              </div>
              <div className="fw-bold text-primary">
                {(totalTeamVolume / 1000).toFixed(1)}kL
              </div>
            </div>
            <div className="text-end">
              <div className="fs--2 text-muted">
                {_languageService?.resources.teamRevenue}
              </div>
              <div className="fw-bold text-success">
                {(totalTeamRevenue / 1000).toFixed(1)}K{' '}
                {_languageService?.resources.rial}
              </div>
            </div>
          </div>
        </div>
      </Card.Header>
      <Card.Body className="p-0">
        <Table responsive className="mb-0">
          <thead className="bg-light">
            <tr>
              <th className="border-0 ps-3">
                {_languageService?.resources.rank}
              </th>
              <th className="border-0">
                {_languageService?.resources.employee}
              </th>
              <th className="border-0 text-center">
                {_languageService?.resources.volumeSold}
              </th>
              <th className="border-0 text-center">
                {_languageService?.resources.revenueGenerated}
              </th>
              <th className="border-0 text-center">
                {_languageService?.resources.performance}
              </th>
              <th className="border-0 text-center">
                {_languageService?.resources.marketShare}
              </th>
            </tr>
          </thead>
          <tbody>
            {sortedData.map((employee, index) => {
              const rank = index + 1;
              const performancePercentage = (
                (employee.totalAmount / totalTeamRevenue) *
                100
              ).toFixed(1);
              const volumePercentage = (
                (employee.totalVolume / totalTeamVolume) *
                100
              ).toFixed(1);

              return (
                <tr key={employee.employeeId}>
                  <td className="ps-3">
                    <div className="d-flex align-items-center">
                      <Badge
                        bg={getPerformanceColor(rank)}
                        className="me-2"
                        style={{
                          width: '24px',
                          height: '24px',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center'
                        }}
                      >
                        <FontAwesomeIcon
                          icon={getPerformanceIcon(rank)}
                          className="fs--2"
                        />
                      </Badge>
                      <span className="fw-bold">#{rank}</span>
                    </div>
                  </td>
                  <td>
                    <div className="d-flex align-items-center">
                      <Avatar
                        size="s"
                        className="me-2"
                        name={getInitials(employee.fullName)}
                      />
                      <div>
                        <div className="fw-bold">
                          {employee.fullName || 'Unknown Employee'}
                        </div>
                        <small className="text-muted">
                          ID: {employee.employeeId || 'N/A'}
                        </small>
                      </div>
                    </div>
                  </td>
                  <td className="text-center">
                    <div className="fw-bold text-primary">
                      {(employee.totalVolume / 1000).toFixed(1)}kL
                    </div>
                    <small className="text-muted">
                      {volumePercentage}% {_languageService?.resources.ofTotal}
                    </small>
                  </td>
                  <td className="text-center">
                    <div className="fw-bold text-success">
                      {employee.totalAmount.toFixed(0)}{' '}
                      {_languageService?.resources.rial}
                    </div>
                    <small className="text-muted">
                      {performancePercentage}%{' '}
                      {_languageService?.resources.ofTotal}
                    </small>
                  </td>
                  <td className="text-center">
                    <div className="d-flex align-items-center justify-content-center">
                      <div className="me-2">
                        <div
                          className="progress"
                          style={{ width: '60px', height: '6px' }}
                        >
                          <div
                            className={`progress-bar bg-${getPerformanceColor(
                              rank
                            )}`}
                            style={{ width: `${performancePercentage}%` }}
                          ></div>
                        </div>
                      </div>
                      <span className="fw-bold">{performancePercentage}%</span>
                    </div>
                  </td>
                  <td className="text-center">
                    <Badge bg={getPerformanceColor(rank)} className="fs--2">
                      {volumePercentage}%
                    </Badge>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </Table>

        {validData.length === 0 && (
          <div className="text-center py-4">
            <FontAwesomeIcon icon="users" className="fs-1 text-muted mb-3" />
            <p className="text-muted mb-0">
              {_languageService?.resources.noEmployeeDataAvailable}
            </p>
          </div>
        )}
      </Card.Body>
    </Card>
  );
};

EmployeePerformanceTable.propTypes = {
  data: PropTypes.arrayOf(
    PropTypes.shape({
      employeeId: PropTypes.number.isRequired,
      fullName: PropTypes.string.isRequired,
      totalVolume: PropTypes.number.isRequired,
      totalAmount: PropTypes.number.isRequired
    })
  ).isRequired
};

export default EmployeePerformanceTable;
