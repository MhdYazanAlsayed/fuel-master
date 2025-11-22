import React, { useState, useEffect } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _zoneService = DependenciesInjector.services.zoneService;
const _cityService = DependenciesInjector.services.cityService;

const CreateModal = ({ open, setOpen, handleRefreshPage }) => {
  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: '',
    zoneId: -1,
    cityId: -1
  });
  const [zones, setZones] = useState([]);
  const [cities, setCities] = useState([]);
  const [loading, setLoading] = useState(false);
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open) return;

    setFormData({
      arabicName: '',
      englishName: '',
      zoneId: -1,
      cityId: -1
    });
    handleOnLoadComponentAsync();
  }, [open]);

  const handleOnLoadComponentAsync = async () => {
    setLoading(true);
    await handleGetZonesAsync();
    await handleGetCitiesAsync();
    setLoading(false);
  };

  const handleGetZonesAsync = async () => {
    const response = await _zoneService.getAllAsync();
    setZones(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
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

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _stationService.createAsync(formData);
    if (!response.succeeded) return;

    handleRefreshPage();
    setOpen(false);
  };

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.createStation}
      size={'md'}
    >
      <Modal.Body>
        {loading ? (
          <div className="text-center py-3">Loading...</div>
        ) : (
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

            <Form.Group className="mb-4">
              <Form.Label>
                <span>{_languageService.resources.zone}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                onChange={x =>
                  handleOnChange('zoneId', parseInt(x.currentTarget.value))
                }
              >
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                {zones}
              </Form.Select>
            </Form.Group>

            <Button
              variant="primary"
              type="submit"
              disabled={
                formData.arabicName.trim() === '' ||
                formData.englishName.trim() === '' ||
                formData.zoneId === -1 ||
                formData.cityId === -1
              }
            >
              {_languageService.resources.create}
            </Button>
          </form>
        )}
      </Modal.Body>
    </ModalCenter>
  );
};

export default CreateModal;
