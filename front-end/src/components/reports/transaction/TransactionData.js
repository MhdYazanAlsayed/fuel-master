import React, { useState } from 'react';
import { Badge, Dropdown } from 'react-bootstrap';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import CardDropdown from 'components/theme/common/CardDropdown';
import DetailsModal from './DetailsModal';

const _languageService = DependenciesInjector.services.languageService;

const TransactionData = ({ transactions, pagination, setPagination }) => {
  const [current, setCurrent] = useState(null);
  const [detailsModal, setDetailsModal] = useState(false);

  const handleOpenDetailsModal = transaction => {
    setCurrent(transaction);
    setDetailsModal(true);
  };

  const columns = [
    {
      header: _languageService.resources.station,
      Cell: data => data.stationName !== null && <>{data.stationName}</>
    },
    {
      header: _languageService.resources.date,
      Cell: data =>
        data.dateTime !== null && (
          <>{new Date(data.dateTime).toLocaleString()}</>
        )
    },
    {
      header: _languageService.resources.nozzle,
      Cell: data =>
        data.nozzle !== null && (
          <>
            {data.nozzle?.number +
              ' — ' +
              _languageService.resources.fuelTypes[data.nozzle?.tank.fuelType]}
          </>
        )
    },
    {
      header: _languageService.resources.pump,
      Cell: data => data.pump !== null && <>{data.pump?.number}</>
    },
    {
      header: _languageService.resources.paymentMethod,
      Cell: data =>
        data.paymentMethod !== null && (
          <Badge
            color={
              _languageService.resources.paymentMethods[data.paymentMethod].bg
            }
          >
            {
              _languageService.resources.paymentMethods[data.paymentMethod]
                .label
            }
          </Badge>
        )
    },
    {
      header: _languageService.resources.volume,
      Cell: data => (
        <>
          {data.volume} {_languageService.resources.liter}
        </>
      )
    },
    {
      header: _languageService.resources.amount,
      Cell: data => (
        <>
          {data.amount} {_languageService.resources.rial}
        </>
      )
    },
    {
      header: '',
      Cell: data =>
        data.paymentMethod !== null && (
          <CardDropdown>
            <div className="py-2">
              <Dropdown.Item
                className="cursor-pointer"
                as={'div'}
                onClick={() => handleOpenDetailsModal(data)}
              >
                {_languageService.resources.details}
              </Dropdown.Item>
            </div>
          </CardDropdown>
        )
    }
  ];

  return (
    <div>
      <FuelMasterTable
        title={_languageService.resources.transactions}
        columns={columns}
        data={transactions}
        pagination={pagination}
        setPagination={setPagination}
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        transaction={current}
      />
    </div>
  );
};

export default TransactionData;
