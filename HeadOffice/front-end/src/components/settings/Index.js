import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { useEvents } from 'hooks/useEvents';
import React, { useState } from 'react';
import { Button, Card, Form } from 'react-bootstrap';
import { toast } from 'react-toastify';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _userService = useService(Services.UserService);

  const [formData, setFormData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    if (
      formData.currentPassword.trim().length < 6 ||
      formData.newPassword.trim().length < 6 ||
      formData.confirmNewPassword.trim().length < 6
    ) {
      toast.error(_languageService.resources.passwordsMustBeMoreThan6);
      return;
    }

    if (formData.newPassword.trim() !== formData.confirmNewPassword.trim()) {
      toast.error(_languageService.resources.passwordsDontMatch);
      return;
    }

    await _userService.changePasswordAsync(formData);
  };

  return (
    <Card>
      <Card.Header>
        <h4>{_languageService.resources.settings}</h4>
      </Card.Header>
      <Card.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              {_languageService.resources.currentPassword}
              <span className="text-danger">*</span>
            </Form.Label>
            <Form.Control
              type="password"
              placeholder={_languageService.resources.currentPassword}
              value={formData.currentPassword}
              onChange={x =>
                handleOnChange('currentPassword', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              {_languageService.resources.newPassword}
              <span className="text-danger">*</span>
            </Form.Label>
            <Form.Control
              type="password"
              placeholder={_languageService.resources.newPassword}
              value={formData.newPassword}
              onChange={x =>
                handleOnChange('newPassword', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              {_languageService.resources.confirmNewPassword}
              <span className="text-danger">*</span>
            </Form.Label>
            <Form.Control
              type="password"
              placeholder={_languageService.resources.confirmNewPassword}
              value={formData.confirmNewPassword}
              onChange={x =>
                handleOnChange('confirmNewPassword', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.currentPassword.trim() === '' ||
              formData.newPassword.trim() === '' ||
              formData.confirmNewPassword.trim() === ''
            }
          >
            {_languageService.resources.saveChanges}
          </Button>
        </form>
      </Card.Body>
    </Card>
  );
};

export default Index;
