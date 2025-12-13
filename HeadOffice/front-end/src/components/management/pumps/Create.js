import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _pumpService = DependenciesInjector.services.pumpService;
const _stationService = DependenciesInjector.services.stationService;
const _roleManager = DependenciesInjector.services.roleManager;

const Create = () => {
  if (!_roleManager.check(Permissions.PumpsCreate))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    number: '',
    stationId: -1,
    manufacturer: ''
  });
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  useEffect(() => {
    handleGetStationsAsync();
  }, []);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _pumpService.createAsync(formData);
    if (!response.succeeded) return;

    navigate('/pumps');
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
    setLoading(false);
  };

  return (
    <FormCard
      header={_languageService.resources.createPump}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.number}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.number}
              onChange={x =>
                handleOnChange('number', parseInt(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.station}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnChange('stationId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {stations}
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.manufacturer}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.manufacturer}
              onChange={x =>
                handleOnChange('manufacturer', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.number <= 0 ||
              formData.stationId === -1 ||
              formData.manufacturer.trim() === ''
            }
          >
            {_languageService.resources.create}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Create;
