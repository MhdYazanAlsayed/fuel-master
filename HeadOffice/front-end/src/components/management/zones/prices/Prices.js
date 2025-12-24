import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Loader from 'components/shared/Loader';
import { Badge, Dropdown } from 'react-bootstrap';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Navigate, useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const Prices = () => {
  const _languageService = useService(Services.LanguageService);
  const _zonePriceService = useService(Services.ZonePriceService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.PricingView))
    return <Navigate to={'/errors/404'} />;

  const [prices, setPrices] = useState([]);
  const [loading, setLoading] = useState(true);

  const { zoneId } = useParams();
  if (!zoneId) return <Navigate to={'/errors/404'} />;

  const columns = [
    {
      header: _languageService.resources.fuelType,
      Cell: data => (
        <>
          {data.fuelType.englishName} — {data.fuelType.arabicName}
        </>
      )
    },

    {
      header: _languageService.resources.price,
      Cell: data => (
        <Badge bg={'warning'} style={{ fontSize: '0.7rem' }}>
          {data?.price} {_languageService.resources.rial}
        </Badge>
      )
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            <Dropdown.Item as={Link} to={`/zones/prices/${data.id}/histories`}>
              {_languageService.resources.histories}
            </Dropdown.Item>
          </div>
        </CardDropdown>
      )
    }
  ];

  useEffect(() => {
    handleGetPrices();
  }, []);

  const handleGetPrices = async () => {
    const response = await _zonePriceService.getPricesAsync(zoneId);
    if (!response) return;

    setPrices(response);
    setLoading(false);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.prices}
        data={prices}
        columns={columns}
      />
    </Loader>
  );
};

export default Prices;
