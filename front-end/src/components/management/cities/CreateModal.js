import ModalTop from 'components/shared/ModalTop';
import React, { useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';

const CreateModal = ({ open, setOpen }) => {
  const _languageService = DependenciesInjector.services.languageService;
  const _cityService = DependenciesInjector.services.cityService;

  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _cityService.createAsync(formData);
    if (!response.succeeded) return;

    // Refresh
  };

  return (
    <ModalTop
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.createCity}
    >
      <Modal.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.arabicName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.arabicName}
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
            {_languageService.resources.create}
          </Button>
        </form>
      </Modal.Body>
    </ModalTop>
  );
};

export default CreateModal;
