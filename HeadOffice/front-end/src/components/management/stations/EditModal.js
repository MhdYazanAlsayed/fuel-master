import React, { useEffect, useState } from 'react';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const EditModal = ({ open, setOpen, station, handleUpdateStation }) => {
  const _languageService = useService(Services.LanguageService);
  const _stationService = useService(Services.StationService);
  const _zoneService = useService(Services.ZoneService);
  const _areaService = useService(Services.AreaService);

  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: '',
    zoneId: -1,
    areaId: -1
  });
  const [zones, setZones] = useState([]);
  const [areas, setAreas] = useState([]);
  const [loading, setLoading] = useState(false);
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !station) return;

    setFormData({
      arabicName: station.arabicName ?? '',
      englishName: station.englishName ?? '',
      zoneId: station.zone?.id ?? -1,
      areaId: station.area?.id ?? -1
    });
    handleOnLoadComponentAsync();
  }, [open, station]);

  const handleOnLoadComponentAsync = async () => {
    setLoading(true);
    await handleGetZonesAsync();
    await handleGetAreasAsync();
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

  const handleGetAreasAsync = async () => {
    const response = await _areaService.getAllAsync();
    setAreas(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!station) return;

    const response = await _stationService.editAsync(station.id, {
      ...formData,
      areaId: formData.areaId === -1 ? null : formData.areaId
    });
    if (!response.succeeded) return;

    handleUpdateStation(response.entity);
    setOpen(false);
  };

  const isDisabled =
    !station ||
    formData.arabicName.trim() === '' ||
    formData.englishName.trim() === '' ||
    formData.zoneId === -1 ||
    (formData.arabicName === (station?.arabicName ?? '') &&
      formData.englishName === (station?.englishName ?? '') &&
      formData.zoneId === (station?.zone?.id ?? -1) &&
      formData.areaId === (station?.area?.id ?? -1));

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editStation}
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
                value={formData.arabicName}
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
                value={formData.englishName}
                onChange={x =>
                  handleOnChange('englishName', x.currentTarget.value)
                }
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                <span>{_languageService.resources.zone}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                value={formData.zoneId}
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

            <Form.Group className="mb-4">
              <Form.Label>
                <span>{_languageService.resources.area}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                value={formData.areaId}
                onChange={x =>
                  handleOnChange('areaId', parseInt(x.currentTarget.value))
                }
              >
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                {areas}
              </Form.Select>
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
