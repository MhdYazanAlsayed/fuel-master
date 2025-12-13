import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate } from 'react-router-dom';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _groupService = DependenciesInjector.services.groupService;
const _roleManager = DependenciesInjector.services.roleManager;

const Create = () => {
  if (!_roleManager.check(Permissions.GroupsCreate))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _groupService.createAsync(formData);
    if (!response.succeeded) return;

    navigate('/groups');
  };

  return (
    <FormCard
      header={_languageService.resources.createGroup}
      smallHeader={_languageService.resources.fillFields}
    >
      <form onSubmit={handleOnSubmitAsync}>
        <Form.Group className="mb-2">
          <Form.Label>
            <span>{_languageService.resources.arabicName}</span>
            <span className="text-danger fw-bold">*</span>
          </Form.Label>
          <Form.Control
            type="text"
            placeholder={_languageService.resources.arabicName}
            onChange={x => handleOnChange('arabicName', x.currentTarget.value)}
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
            onChange={x => handleOnChange('englishName', x.currentTarget.value)}
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
          {_languageService.resources.create}
        </Button>
      </form>
    </FormCard>
  );
};

export default Create;
