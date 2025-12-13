import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterPicker from 'components/shared/FuelMasterPicker';
import Loader from 'components/shared/Loader';
import { useEvents } from 'hooks/useEvents';
import React, { useEffect, useState } from 'react';
import { Button, Card, Form } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _identityService = DependenciesInjector.services.identityService;

const FilterBox = ({ handleGetReportAsync }) => {
  const [stations, setStations] = useState([]);
  const [formData, setFormData] = useState({
    from: '',
    to: '',
    stationId: -1
  });
  const [loading, setLoading] = useState(true);

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

  const handleOnSubmit = e => {
    e.preventDefault();

    handleGetReportAsync({
      ...formData,
      stationId: formData.stationId !== -1 ? formData.stationId : null
    });
  };

  return (
    <Card className="mb-2">
      <Card.Header>
        <h4>{_languageService.resources.filter}</h4>
      </Card.Header>
      <Card.Body>
        <form onSubmit={handleOnSubmit}>
          <div className="row">
            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.from}</Form.Label>
                <FuelMasterPicker
                  value={formData.from}
                  onChange={x => handleOnChange('from', x)}
                />
              </Form.Group>

              {!_identityService.currentUser.stationId && (
                <Loader loading={loading}>
                  <Form.Group className="mb-2">
                    <Form.Label>
                      {_languageService.resources.station}
                    </Form.Label>
                    <Form.Select
                      value={formData.stationId}
                      onChange={x =>
                        handleOnChange(
                          'stationId',
                          parseInt(x.currentTarget.value)
                        )
                      }
                    >
                      <option value={-1}>
                        {_languageService.resources.selectOption}
                      </option>
                      {stations}
                    </Form.Select>
                  </Form.Group>
                </Loader>
              )}
            </div>
            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.to}</Form.Label>
                <FuelMasterPicker
                  value={formData.to}
                  onChange={x => handleOnChange('to', x)}
                />
              </Form.Group>
            </div>
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
