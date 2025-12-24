import React, { useEffect, useState } from 'react';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const EditModal = ({ open, setOpen, pump, handleUpdatePump }) => {
  const _languageService = useService(Services.LanguageService);
  const _pumpService = useService(Services.PumpService);

  const [formData, setFormData] = useState({
    number: '',
    manufacturer: ''
  });
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(false);
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !pump) return;

    setFormData({
      number: pump.number ?? '',
      manufacturer: pump.manufacturer ?? ''
    });
  }, [open, pump]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!pump) return;

    const response = await _pumpService.editAsync(pump.id, formData);
    if (!response.succeeded) return;

    handleUpdatePump(response.entity);
    setOpen(false);
  };

  const isDisabled =
    !pump ||
    formData.number <= 0 ||
    formData.stationId === -1 ||
    formData.manufacturer.trim() === '' ||
    (formData.number === (pump?.number ?? '') &&
      formData.stationId === (pump?.station?.id ?? -1) &&
      formData.manufacturer === (pump?.manufacturer ?? ''));

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editPump}
      size={'md'}
    >
      <Modal.Body>
        {loading ? (
          <div className="text-center py-3">Loading...</div>
        ) : (
          <form onSubmit={handleOnSubmitAsync}>
            <Form.Group className="mb-2">
              <Form.Label>
                <span>{_languageService.resources.number}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Control
                type="number"
                placeholder={_languageService.resources.number}
                value={formData.number}
                onChange={x =>
                  handleOnChange('number', parseInt(x.currentTarget.value))
                }
              />
            </Form.Group>

            <Form.Group className="mb-4">
              <Form.Label>
                <span>{_languageService.resources.manufacturer}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Control
                type="text"
                placeholder={_languageService.resources.manufacturer}
                value={formData.manufacturer}
                onChange={x =>
                  handleOnChange('manufacturer', x.currentTarget.value)
                }
              />
            </Form.Group>

            <Button variant="primary" type="submit" disabled={isDisabled}>
              {_languageService.resources.edit}
            </Button>
          </form>
        )}
      </Modal.Body>
    </ModalCenter>
  );
};

export default EditModal;
