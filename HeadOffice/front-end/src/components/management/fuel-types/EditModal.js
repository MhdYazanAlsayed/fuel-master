import React, { useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';

const _languageService = DependenciesInjector.services.languageService;
const _fuelTypeService = DependenciesInjector.services.fuelTypeService;

const EditModal = ({ open, setOpen, fuelType, handleUpdateFuelType }) => {
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !fuelType) return;

    setFormData({
      arabicName: fuelType.arabicName ?? '',
      englishName: fuelType.englishName ?? ''
    });
  }, [open, fuelType]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!fuelType) return;

    const response = await _fuelTypeService.updateAsync(fuelType.id, formData);
    if (!response.succeeded) return;

    handleUpdateFuelType(response.data);
    setOpen(false);
  };

  const isDisabled =
    !fuelType ||
    formData.arabicName.trim() === '' ||
    formData.englishName.trim() === '' ||
    (formData.arabicName === (fuelType?.arabicName ?? '') &&
      formData.englishName === (fuelType?.englishName ?? ''));

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editFuelType}
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

          <Button variant="primary" type="submit" disabled={isDisabled}>
            {_languageService.resources.edit}
          </Button>
        </form>
      </Modal.Body>
    </ModalCenter>
  );
};

export default EditModal;
