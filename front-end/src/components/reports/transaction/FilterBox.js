import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterPicker from 'components/shared/FuelMasterPicker';
import Loader from 'components/shared/Loader';
import { useEvents } from 'hooks/useEvents';
import React, { useEffect, useState } from 'react';
import { Button, Card, Form } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _roleManager = DependenciesInjector.services.roleManager;
const _employeeService = DependenciesInjector.services.employeeSerivce;
const _identityService = DependenciesInjector.services.identityService;

const FilterBox = ({ handleGetReportAsync }) => {
  const [stations, setStations] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [formData, setFormData] = useState({
    from: '',
    to: '',
    stationId: -1,
    employeeId: -1
  });
  const [loading, setLoading] = useState(true);
  const [employeeLoading, setEmployeeLoading] = useState(false);

  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    handleOnLoadAsync();
  }, []);

  const handleOnLoadAsync = async () => {
    if (!_identityService.currentUser.stationId) {
      await handleGetStationsAsync();
      setLoading(false);
      return;
    }

    if (_roleManager.check(Permissions.TransactionReportFilter)) {
      await handleGetEmployeesAsync(_identityService.currentUser.stationId);
    }
  };

  const handleOnStationChange = stationId => {
    handleOnChange('stationId', stationId);
    handleOnChange('employeeId', -1);

    handleGetEmployeesAsync(stationId);
  };

  const handleGetEmployeesAsync = async stationId => {
    if (stationId === -1) {
      setEmployees([]);
      return;
    }

    setEmployeeLoading(true);
    const response = await _employeeService.getAllAsync(stationId);

    setEmployees(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {item.fullName}
        </option>
      ))
    );
    setEmployeeLoading(false);
  };

  const handleGetStationsAsync = async () => {
    const response = await _stationService.getAllAsync();
    setStations(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    await handleGetReportAsync({
      ...formData,
      stationId: formData.stationId !== -1 ? formData.stationId : null,
      employeeId: formData.employeeId !== -1 ? formData.employeeId : null
    });
  };

  return (
    <Card className="mb-2">
      <Card.Header>
        <h4>{_languageService.resources.filter}</h4>
      </Card.Header>
      <Card.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <div className="row">
            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.from}</Form.Label>
                <FuelMasterPicker
                  value={formData.from}
                  onChange={x => handleOnChange('from', x)}
                />
              </Form.Group>
            </div>
            {!_identityService.currentUser.stationId && (
              <div className="col-md-6">
                <Loader loading={loading}>
                  <Form.Group className="mb-2">
                    <Form.Label>
                      {_languageService.resources.station}
                    </Form.Label>
                    <Form.Select
                      onChange={x =>
                        handleOnStationChange(parseInt(x.currentTarget.value))
                      }
                    >
                      <option value={-1}>
                        {_languageService.resources.selectOption}
                      </option>
                      {stations}
                    </Form.Select>
                  </Form.Group>
                </Loader>
              </div>
            )}

            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.to}</Form.Label>
                <FuelMasterPicker
                  value={formData.to}
                  onChange={x => handleOnChange('to', x)}
                />
              </Form.Group>
            </div>
            {_roleManager.check(Permissions.TransactionReportFilter) && (
              <div className="col-md-6">
                <Loader loading={employeeLoading}>
                  <Form.Group className="mb-2">
                    <Form.Label>
                      {_languageService.resources.employee}
                    </Form.Label>
                    <Form.Select
                      value={formData.employeeId}
                      onChange={x =>
                        handleOnChange(
                          'employeeId',
                          parseInt(x.currentTarget.value)
                        )
                      }
                    >
                      <option value={-1}>
                        {_languageService.resources.selectOption}
                      </option>
                      {employees}
                    </Form.Select>
                  </Form.Group>
                </Loader>
              </div>
            )}

            <div className="col-md-12">
              <Button variant="primary" type="submit">
                {_languageService.resources.search}
              </Button>
            </div>
          </div>
        </form>
      </Card.Body>
    </Card>
  );
};

export default FilterBox;
