import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _cityService = DependenciesInjector.services.cityService;
const _roleManager = DependenciesInjector.services.roleManager;

const Edit = () => {
  if (!_roleManager.check(Permissions.CitiesEdit))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });
  const [loading, setLoading] = useState(false);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();
  const { id } = useParams();
  if (!id) return <Navigate to="/errors/404" />;

  useEffect(() => {
    handleGetDetailsAsync();
  }, []);

  const handleGetDetailsAsync = async () => {
    setLoading(true);

    const response = await _cityService.detailsAsync(id);
    if (!response) return;

    setFormData({
      arabicName: response.arabicName,
      englishName: response.englishName
    });

    setLoading(false);
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _cityService.editAsync(id, formData);
    if (!response.succeeded) return;

    navigate('/cities');
  };

  return (
    <FormCard
      header={_languageService.resources.editCity}
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

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.arabicName.trim() === '' ||
              formData.englishName.trim() === ''
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
