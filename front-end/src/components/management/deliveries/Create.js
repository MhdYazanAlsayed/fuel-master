import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { toast } from 'react-toastify';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';
import { Navigate } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _deliveryService = DependenciesInjector.services.deliveryService;
const _stationsSerivce = DependenciesInjector.services.stationService;
const _tankService = DependenciesInjector.services.tankService;
const _roleManager = DependenciesInjector.services.roleManager;

const Create = () => {
  if (!_roleManager.check(Permissions.DeliveriesCreate))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    transport: '',
    invoiceNumber: '',
    paidVolume: 0,
    receivedVolume: 0,
    tankId: -1,
    stationId: -1
  });
  const [tanks, setTanks] = useState([]);
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    handleOnLoadAsync();
  }, []);

  const handleOnLoadAsync = async () => {
    await handleGetStationsAsync();
    setLoading(false);
  };

  const handleGetStationsAsync = async () => {
    const response = await _stationsSerivce.getAllAsync();

    setStations(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleGetTanksAsync = async stationId => {
    handleOnChange('stationId', stationId);

    const response = await _tankService.getAllAsync(stationId);

    setTanks(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.resources.tankNumber + ' ' + item.number} {' - '}
          {_languageService.resources.fuelTypes[item.fuelType]}
        </option>
      ))
    );
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _deliveryService.createAsync({
      transport: formData.transport,
      invoiceNumber: formData.invoiceNumber,
      paidVolume: formData.paidVolume,
      receivedVolume: formData.receivedVolume,
      tankId: formData.tankId
    });
    if (!response.succeeded) return;

    setFormData({
      transport: '',
      invoiceNumber: '',
      paidVolume: 0,
      receivedVolume: 0,
      tankId: -1
    });
    toast.success(_languageService.resources.createdSuccessfully);
  };

  return (
    <FormCard
      header={_languageService.resources.createDelivery}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.transport}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              value={formData.transport}
              placeholder={_languageService.resources.transport}
              onChange={x => handleOnChange('transport', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.invoiceNumber}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              value={formData.invoiceNumber}
              placeholder={_languageService.resources.invoiceNumber}
              onChange={x =>
                handleOnChange('invoiceNumber', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.paidVolume}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              min={0}
              step="0.01"
              placeholder={_languageService.resources.paidVolume}
              value={formData.paidVolume}
              onChange={x =>
                handleOnChange('paidVolume', parseFloat(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.receivedVolume}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              step="0.01"
              min={0}
              value={formData.receivedVolume}
              placeholder={_languageService.resources.receivedVolume}
              onChange={x =>
                handleOnChange(
                  'receivedVolume',
                  parseFloat(x.currentTarget.value)
                )
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.station}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.stationId}
              onChange={x =>
                handleGetTanksAsync(parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {stations}
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.tank}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.tankId}
              onChange={x =>
                handleOnChange('tankId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {tanks}
            </Form.Select>
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.transport.trim() === '' ||
              formData.invoiceNumber.trim() === '' ||
              formData.paidVolume < 0 ||
              formData.receivedVolume < 0 ||
              formData.tankId === -1
            }
          >
            {_languageService.resources.create}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Create;
