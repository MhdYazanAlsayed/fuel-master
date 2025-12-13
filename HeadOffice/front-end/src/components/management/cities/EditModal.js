import React, { useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';

const _languageService = DependenciesInjector.services.languageService;
const _cityService = DependenciesInjector.services.cityService;

const EditModal = ({ open, setOpen, city, handleUpdateCity }) => {
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !city) return;

    setFormData({
      arabicName: city.arabicName ?? '',
      englishName: city.englishName ?? ''
    });
  }, [open, city]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!city) return;

    const response = await _cityService.editAsync(city.id, formData);
    if (!response.succeeded) return;

    handleUpdateCity(response.data);
    setOpen(false);
  };

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editCity}
      size={'md'}
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
              value={formData.arabicName}
              onChange={x =>
                handleOnChange('arabicName', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-4">
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
              formData.englishName.trim() === '' || 
              (formData.englishName == city.englishName && formData.arabicName == city.arabicName)
            }
          >
            {_languageService.resources.edit}
          </Button>
        </form>
      </Modal.Body>
    </ModalCenter>
  );
};

export default EditModal;
