import React, { useState, useEffect } from 'react';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const CreateModal = ({ open, setOpen, handleRefreshPage }) => {
  const _languageService = useService(Services.LanguageService);
  const _areaService = useService(Services.AreaService);
  const _cityService = useService(Services.CityService);

  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: '',
    cityId: -1
  });
  const [cities, setCities] = useState([]);
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open) return;

    setFormData({
      arabicName: '',
      englishName: '',
      cityId: -1
    });
    handleGetCitiesAsync();
  }, [open]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _areaService.createAsync(formData);
    if (!response.succeeded) return;

    handleRefreshPage();
    setOpen(false);
  };

  const handleGetCitiesAsync = async () => {
    const response = await _cityService.getAllAsync();

    setCities(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.createArea}
      size={'md'}
    >
      <Modal.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.city}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnChange('cityId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {cities}
            </Form.Select>
          </Form.Group>

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

          <Form.Group className="mb-4">
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
              formData.englishName.trim() === '' ||
              formData.cityId === -1
            }
          >
            {_languageService.resources.create}
          </Button>
        </form>
      </Modal.Body>
    </ModalCenter>
  );
};

export default CreateModal;
