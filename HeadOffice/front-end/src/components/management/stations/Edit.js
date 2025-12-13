import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _zoneService = DependenciesInjector.services.zoneService;
const _cityService = DependenciesInjector.services.cityService;
const _roleManager = DependenciesInjector.services.roleManager;

const Edit = () => {
  if (!_roleManager.check(Permissions.StationsEdit))
    return <Navigate to="/errors/404" />;
  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: '',
    zoneId: -1,
    cityId: -1
  });
  const [zones, setZones] = useState([]);
  const [cities, setCities] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();
  const { id } = useParams();
  if (!id) return <Navigate to="/errors/404" />;

  useEffect(() => {
    handleOnLoadComponentAsync();
  }, []);

  const handleOnLoadComponentAsync = async () => {
    await handleGetDetailsAsync();
    await handleGetZonesAsync();
    await handleGetCitiesAsync();

    setLoading(false);
  };

  const handleGetDetailsAsync = async () => {
    const response = await _stationService.detailsAsync(id);
    if (!response) return;

    setFormData({
      arabicName: response.arabicName,
      englishName: response.englishName,
      zoneId: response.zone.id,
      cityId: response.city.id
    });
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _stationService.editAsync(id, formData);
    if (!response.succeeded) return;

    navigate('/stations');
  };

  const handleGetZonesAsync = async () => {
    const response = await _zoneService.getAllAsync();

    setZones(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleGetCitiesAsync = async () => {
    const response = await _cityService.getAllAsync();

    setCities(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  return (
    <FormCard
      header={_languageService.resources.editStation}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.arabicName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.arabicName}
              value={formData.arabicName}
              onChange={x =>
                handleOnChange('arabicName', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.englishName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.englishName}
              value={formData.englishName}
              onChange={x =>
                handleOnChange('englishName', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.city}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>

            <Form.Select
              value={formData.cityId}
              onChange={x =>
                handleOnChange('cityId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {cities}
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.zone}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.zoneId}
              onChange={x =>
                handleOnChange('zoneId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {zones}
            </Form.Select>
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.arabicName.trim() === '' ||
              formData.englishName.trim() === '' ||
              formData.zoneId === -1 ||
              formData.cityId === -1
            }
          >
            {_languageService.resources.edit}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Edit;
