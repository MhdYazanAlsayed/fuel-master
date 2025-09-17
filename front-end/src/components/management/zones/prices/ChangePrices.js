import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import Loader from 'components/shared/Loader';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Navigate, useNavigate, useParams } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _zonePriceService = DependenciesInjector.services.zonePriceService;
const _roleManager = DependenciesInjector.services.roleManager;

const ChangePrices = () => {
  if (!_roleManager.check(Permissions.ChangePrices))
    return <Navigate to="/errors/404" />;

  const [formData, setFormData] = useState([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const { zoneId } = useParams();
  if (!zoneId) return <Navigate to={'/errors/404'} />;

  useEffect(() => {
    handleGetPricesAsync();
  }, []);

  const handleGetPricesAsync = async () => {
    setLoading(true);

    const response = await _zonePriceService.getPricesAsync(zoneId);
    if (!response) return;

    setFormData(response);

    setLoading(false);
  };

  const handleOnChange = (fuelType, value) => {
    setFormData(prev => {
      const index = prev.findIndex(x => x.fuelType == fuelType);
      if (index == -1) {
        prev.push({
          fuelType: fuelType,
          zoneId: parseInt(zoneId),
          price: parseFloat(value)
        });
      } else {
        prev[index].price = value;
      }

      return [...prev];
    });
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _zonePriceService.changePriceAsync(zoneId, formData);
    if (!response.succeeded) return;

    navigate('/zones');
  };

  return (
    <FormCard
      header={_languageService.resources.zonePrices}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>{_languageService.resources.fuelTypes[0]}</Form.Label>
            <Form.Control
              type="number"
              step={0.1}
              placeholder={_languageService.resources.fuelTypes[0]}
              value={formData.find(x => x.fuelType == 0)?.price ?? 0}
              onChange={x => handleOnChange(0, x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>{_languageService.resources.fuelTypes[1]}</Form.Label>
            <Form.Control
              type="number"
              step={0.1}
              placeholder={_languageService.resources.fuelTypes[1]}
              value={formData.find(x => x.fuelType == 1)?.price ?? 0}
              onChange={x => handleOnChange(1, x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>{_languageService.resources.fuelTypes[2]}</Form.Label>
            <Form.Control
              type="number"
              step={0.1}
              placeholder={_languageService.resources.fuelTypes[2]}
              value={formData.find(x => x.fuelType == 2)?.price ?? 0}
              onChange={x => handleOnChange(2, x.currentTarget.value)}
            />
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.findIndex(x => x.price.toString().trim() === '') != -1
            }
          >
            {_languageService.resources.update}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default ChangePrices;
