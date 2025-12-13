import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import Loader from 'components/shared/Loader';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import { useEvents } from 'hooks/useEvents';

const _languageService = DependenciesInjector.services.languageService;
const _zonePriceService = DependenciesInjector.services.zonePriceService;
// const _roleManager = DependenciesInjector.services.roleManager;

const ChangePrices = () => {
  // if (!_roleManager.check(Permissions.ChangePrices))
  //   return <Navigate to="/errors/404" />;

  const [formData, setFormData] = useState([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const { zoneId } = useParams();
  if (!zoneId) return <Navigate to={'/errors/404'} />;

  useEffect(() => {
    handleGetPricesAsync();
  }, []);

  const handleOnChange = (id, price) => {
    setFormData(prev => {
      return prev.map(x => (x.id === id ? { ...x, price } : x));
    });
  };

  const handleGetPricesAsync = async () => {
    setLoading(true);

    const response = await _zonePriceService.getPricesAsync(zoneId);
    if (!response) return;

    setFormData(response);
    console.log(response);
    setLoading(false);
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
          {formData.map(x => (
            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.isRTL
                  ? x.fuelType.arabicName
                  : x.fuelType.englishName}
              </Form.Label>
              <Form.Control
                type="number"
                value={x.price}
                onChange={a => handleOnChange(x.id, a.currentTarget.value)}
              />
            </Form.Group>
          ))}

          <Button
            variant="primary"
            type="submit"
            disabled={formData.some(x => x.price.toString().trim() === '')}
          >
            {_languageService.resources.update}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default ChangePrices;
