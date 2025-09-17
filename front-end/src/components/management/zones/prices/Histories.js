import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import Loader from 'components/shared/Loader';
import React, { useEffect, useState } from 'react';
import { Badge } from 'react-bootstrap';
import { Navigate, useParams } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _zonePriceHistoryService =
  DependenciesInjector.services.zonePriceHistoryService;
const _roleManager = DependenciesInjector.services.roleManager;

const Histories = () => {
  if (!_roleManager.check(Permissions.ShowPricesHistories))
    return <Navigate to="/errors/404" />;
  // States
  const [histories, setHistories] = useState([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });
  const [loading, setLoading] = useState(true);

  const { zonePriceId } = useParams();
  if (!zonePriceId) return <Navigate to="/errors/404" />;

  const columns = [
    {
      header: _languageService.resources.fuelType,
      Cell: data => <>{_languageService.resources.fuelTypes[data.fuelType]}</>
    },
    {
      header: _languageService.resources.dateTime,
      Cell: data => <>{new Date(data.createdAt).toLocaleString()}</>
    },
    {
      header: _languageService.resources.employee,
      Cell: data => <>{data.userName}</>
    },
    {
      header: _languageService.resources.priceBeforeChange,
      Cell: data => (
        <Badge bg="warning">
          {data?.priceBeforeChange} {_languageService.resources.rial}
        </Badge>
      )
    }
  ];

  useEffect(() => {
    handleGetPaginationAsync();
  }, []);

  useEffect(() => {
    if (!pagination.perform) return;

    handleGetPaginationAsync();
  }, [pagination.perform]);

  const handleGetPaginationAsync = async () => {
    const response = await _zonePriceHistoryService.getHistoriesPaginationAsync(
      zonePriceId,
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setHistories(response.data);
    setLoading(false);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.zoneHistories}
        data={histories}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
      />
    </Loader>
  );
};

export default Histories;
