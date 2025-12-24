import React, { useEffect, useState } from 'react';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const EditModal = ({ open, setOpen, zone, handleUpdateZone }) => {
  const _languageService = useService(Services.LanguageService);
  const _zoneService = useService(Services.ZoneService);

  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: ''
  });

  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !zone) return;

    setFormData({
      arabicName: zone.arabicName ?? '',
      englishName: zone.englishName ?? ''
    });
  }, [open, zone]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!zone) return;

    const response = await _zoneService.editAsync(zone.id, formData);
    if (!response.succeeded) return;

    handleUpdateZone(response.data);
    setOpen(false);
  };

  const disableSubmit =
    formData.arabicName.trim() === '' ||
    formData.englishName.trim() === '' ||
    !zone ||
    (formData.arabicName === (zone?.arabicName ?? '') &&
      formData.englishName === (zone?.englishName ?? ''));

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editZone}
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

          <Button variant="primary" type="submit" disabled={disableSubmit}>
            {_languageService.resources.edit}
          </Button>
        </form>
      </Modal.Body>
    </ModalCenter>
  );
};

export default EditModal;
