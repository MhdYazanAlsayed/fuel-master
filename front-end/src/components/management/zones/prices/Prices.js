import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Loader from 'components/shared/Loader';
import { Badge, Dropdown } from 'react-bootstrap';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Navigate, useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _zonePriceService = DependenciesInjector.services.zonePriceService;
const _roleManager = DependenciesInjector.services.roleManager;

const Prices = () => {
  if (!_roleManager.check(Permissions.ShowPrices))
    return <Navigate to="/errors/404" />;

  const [prices, setPrices] = useState([]);
  const [loading, setLoading] = useState(true);

  const { zoneId } = useParams();
  if (!zoneId) return <Navigate to={'/errors/404'} />;

  const columns = [
    {
      header: _languageService.resources.fuelType,
      Cell: data => <>{_languageService.resources.fuelTypes[data?.fuelType]}</>
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
      Cell: data =>
        _roleManager.check(Permissions.ShowPrices) ? (
          <CardDropdown>
            <div className="py-2">
              <Dropdown.Item
                as={Link}
                to={`/zones/prices/${data.id}/histories`}
              >
                {_languageService.resources.histories}
              </Dropdown.Item>
            </div>
          </CardDropdown>
        ) : null
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
