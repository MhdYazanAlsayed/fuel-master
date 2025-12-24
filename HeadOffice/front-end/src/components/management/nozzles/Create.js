import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const Create = () => {
  const _languageService = useService(Services.LanguageService);
  const _nozzleService = useService(Services.NozzleService);
  const _stationService = useService(Services.StationService);
  const _tankService = useService(Services.TankService);
  const _pumpService = useService(Services.PumpService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationManage))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    tankId: -1,
    number: 0,
    status: 0,
    pumpId: -1,
    readerNumber: '',
    amount: 0,
    volume: 0,
    totalizer: 0,
    stationId: -1
  });
  const [stations, setStations] = useState([]);
  const [tanks, setTanks] = useState([]);
  const [pumps, setPumps] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  useEffect(() => {
    handleOnLoadComponentAsync();
  }, []);

  const handleOnLoadComponentAsync = async () => {
    await handleGetStationsAsync();
    setLoading(false);
  };

  const handleOnStationIdChangesAsync = async stationId => {
    setFormData(prev => ({
      ...prev,
      stationId: stationId,
      tankId: -1,
      pumpId: -1
    }));

    if (stationId === -1) {
      setTanks([]);
      setPumps([]);
      return;
    }

    await handleGetTanksAsync(stationId);
    await handleGetPumpsAsync(stationId);
  };

  const handleGetStationsAsync = async () => {
    const response = await _stationService.getAllAsync();

    setStations(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleGetTanksAsync = async stationId => {
    const response = await _tankService.getAllAsync(stationId);

    setTanks(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.resources.tankNumber + ' ' + item.number} {' - '}
          {_languageService.isRTL
            ? item.fuelType.arabicName
            : item.fuelType.englishName}
        </option>
      ))
    );
  };

  const handleGetPumpsAsync = async stationId => {
    const response = await _pumpService.getAllAsync(stationId);

    setPumps(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {item.number}
        </option>
      ))
    );
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _nozzleService.createAsync({
      tankId: formData.tankId,
      number: formData.number,
      status: formData.status,
      pumpId: formData.pumpId,
      readerNumber: formData.readerNumber,
      amount: formData.amount,
      volume: formData.volume,
      totalizer: formData.totalizer
    });
    if (!response.succeeded) return;

    navigate('/nozzles');
  };

  return (
    <FormCard
      header={_languageService.resources.createNozzle}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.station}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnStationIdChangesAsync(parseInt(x.currentTarget.value))
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

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.pump}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.pumpId}
              onChange={x =>
                handleOnChange('pumpId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {pumps}
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.number}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.number}
              onChange={x =>
                handleOnChange('number', parseInt(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.status}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={e =>
                handleOnChange('status', parseInt(e.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              <option value={0}>
                {_languageService.resources.nozzleStatuses[0]}
              </option>
              <option value={1}>{_languageService.resources.inService}</option>
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.readerNumber}</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.readerNumber}
              onChange={x =>
                handleOnChange('readerNumber', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.amount}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              defaultValue={0}
              type="number"
              placeholder={_languageService.resources.amount}
              onChange={x =>
                handleOnChange('amount', parseInt(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.volume}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              defaultValue={0}
              placeholder={_languageService.resources.volume}
              onChange={x =>
                handleOnChange('volume', parseInt(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.totalizer}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="number"
              defaultValue={0}
              placeholder={_languageService.resources.totalizer}
              onChange={x =>
                handleOnChange('totalizer', parseInt(x.currentTarget.value))
              }
            />
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.tankId === -1 ||
              formData.number <= 0 ||
              formData.status < 0 ||
              formData.pumpId === -1 ||
              formData.amount < 0 ||
              formData.volume < 0 ||
              formData.totalizer < 0
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
