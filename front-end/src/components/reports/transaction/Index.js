import React, { useEffect, useState } from 'react';
import FilterBox from './FilterBox';
import TransactionData from './TransactionData';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Permissions } from 'app/core/enums/Permissions';
import { Navigate } from 'react-router-dom';

const _roleManager = DependenciesInjector.services.roleManager;
const _reportService = DependenciesInjector.services.reportService;

const Index = () => {
  if (!_roleManager.check(Permissions.TransactionReportShow))
    return <Navigate to="/errors/404" />;

  const [transactions, setTransactions] = useState([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  useEffect(() => {
    if (!pagination.perform) return;

    handleGetReportAsync();
  }, [pagination.perform]);

  const handleGetReportAsync = async formData => {
    const response = await _reportService.getTransactionsAsync(
      pagination.currentPage,
      formData.from,
      formData.to,
      formData.stationId,
      formData.employeeId
    );
    if (!response) return;

    response.data.push({
      amount: response.data.reduce((acc, curr) => acc + curr.amount, 0),
      dateTime: null,
      employee: null,
      nozzle: null,
      paymentMethod: null,
      price: null,
      pump: null,
      stationName: null,
      totalizer: null,
      totalizerAfter: null,
      vat: null,
      volume: response.data.reduce((acc, curr) => acc + curr.volume, 0)
    });

    setTransactions(response.data);
    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
  };

  return (
    <div className="transaction-report">
      <FilterBox handleGetReportAsync={handleGetReportAsync} />
      <TransactionData
        pagination={pagination}
        setPagination={setPagination}
        transactions={transactions}
      />
    </div>
  );
};

export default Index;
