import FormCard from 'components/shared/FormCard';
import Loader from 'components/shared/Loader';
import React, { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const ChangePrices = () => {
  const _languageService = useService(Services.LanguageService);
  const _zonePriceService = useService(Services.ZonePriceService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.PricingManage))
    return <Navigate to={'/errors/404'} />;

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
    setLoading(false);
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const data = formData.map(x => ({
      fuelTypeId: x.fuelTypeId,
      price: parseFloat(x.price)
    }));
    const response = await _zonePriceService.changePriceAsync(zoneId, data);
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
            <Form.Group className="mb-2" key={x.id}>
              <Form.Label>
                {_languageService.isRTL
                  ? x.fuelType.arabicName
                  : x.fuelType.englishName}
              </Form.Label>
              <Form.Control
                type="number"
                value={x.price}
                onChange={a =>
                  handleOnChange(x.id, parseFloat(a.currentTarget.value))
                }
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
