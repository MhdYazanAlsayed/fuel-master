import React, { useEffect, useState } from 'react';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const EditModal = ({ open, setOpen, area, handleUpdateArea }) => {
  const _languageService = useService(Services.LanguageService);
  const _areaService = useService(Services.AreaService);

  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !area) return;

    setFormData({
      arabicName: area.arabicName ?? '',
      englishName: area.englishName ?? ''
    });
  }, [open, area]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!area) return;

    const response = await _areaService.editAsync(area.id, formData);
    if (!response.succeeded) return;

    handleUpdateArea(response.data);
    setOpen(false);
  };

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editArea}
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
              (formData.englishName == area.englishName &&
                formData.arabicName == area.arabicName)
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

