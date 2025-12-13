import React, { useEffect, useState } from 'react';
import FilterBox from './FilterBox';
import Deliveries from './Deliveries';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const _reportService = DependenciesInjector.services.reportService;
const Index = () => {
  const [deliveries, setDeliveries] = useState([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  useEffect(() => {
    if (!pagination.perform) return;

    handleGetReportAsync();
  }, [pagination.perform]);

  const handleGetReportAsync = async ({ from, to, stationId }) => {
    const response = await _reportService.getDeliveriesAsync(
      pagination.currentPage,
      from,
      to,
      stationId
    );
    if (!response) return;

    setDeliveries(response.data);
    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
  };

  return (
    <div>
      <FilterBox handleGetReportAsync={handleGetReportAsync} />
      <Deliveries
        deliveries={deliveries}
        pagination={pagination}
        setPagination={setPagination}
      />
    </div>
  );
};

export default Index;
