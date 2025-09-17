import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';
import { toast } from 'react-toastify';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _tankService = DependenciesInjector.services.tankService;
const _roleManager = DependenciesInjector.services.roleManager;

const Create = () => {
  if (!_roleManager.check(Permissions.TanksCreate))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    stationId: -1,
    fuelType: -1,
    number: 0,
    capacity: 0,
    maxLimit: 0,
    minLimit: 0,
    currentLevel: 0,
    currentVolume: 0,
    hasSensor: false
  });
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  useEffect(() => {
    handleOnLoadComponentAsync();
  }, []);

  const handleOnLoadComponentAsync = async () => {
    await handleGetStationsAsync();
    setLoading(false);
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
    if (
      formData.capacity <= 0 ||
      formData.maxLimit <= 0 ||
      formData.minLimit <= 0
    ) {
      toast.error(_languageService.resources.valuesNotValid);
      return;
    }

    if (formData.maxLimit >= formData.capacity) {
      toast.error(_languageService.resources.maxLimitMustBeLessThanCapacity);
      return;
    }

    if (formData.minLimit >= formData.maxLimit) {
      toast.error(_languageService.resources.minLimitMustBeLessThanMaxLimit);
      return;
    }

    if (formData.currentVolume > formData.maxLimit) {
      toast.error(
        _languageService.resources.currentVolumeMustBeLessThanMaxLimit
      );
      return;
    }

    if (formData.currentVolume < formData.minLimit) {
      toast.error(
        _languageService.resources.currentVolumeMustBeGreaterThanMinLimit
      );
      return;
    }

    const response = await _tankService.createAsync(formData);
    if (!response.succeeded) return;

    navigate('/tanks');
  };

  return (
    <FormCard
      header={_languageService.resources.createTank}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
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
              <span>{_languageService.resources.fuelType}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnChange('fuelType', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>

              <option value={0}>
                {_languageService.resources.fuelTypes[0]}
              </option>
              <option value={1}>
                {_languageService.resources.fuelTypes[1]}
              </option>
              <option value={2}>
                {_languageService.resources.fuelTypes[2]}
              </option>
            </Form.Select>
          </Form.Group>

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
              <span>{_languageService.resources.capacity}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.capacity}
              onChange={x =>
                handleOnChange('capacity', parseFloat(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.maxLimit}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.maxLimit}
              onChange={x =>
                handleOnChange('maxLimit', parseFloat(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.minLimit}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.minLimit}
              onChange={x =>
                handleOnChange('minLimit', parseFloat(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.currentLevel}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.currentLevel}
              onChange={x =>
                handleOnChange(
                  'currentLevel',
                  parseFloat(x.currentTarget.value)
                )
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.currentVolume}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.currentVolume}
              onChange={x =>
                handleOnChange(
                  'currentVolume',
                  parseFloat(x.currentTarget.value)
                )
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Check
              type="checkbox"
              label={_languageService.resources.hasSensor}
              onChange={x =>
                handleOnChange('hasSensor', x.currentTarget.checked)
              }
            />
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.stationId === -1 ||
              formData.fuelType === -1 ||
              formData.number <= 0 ||
              formData.capacity < 0 ||
              formData.maxLimit < 0 ||
              formData.minLimit < 0 ||
              formData.currentLevel < 0 ||
              formData.currentVolume < 0 ||
              formData.maxLimit >= formData.capacity ||
              formData.minLimit >= formData.maxLimit ||
              formData.currentVolume > formData.maxLimit ||
              formData.currentVolume < formData.minLimit
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
