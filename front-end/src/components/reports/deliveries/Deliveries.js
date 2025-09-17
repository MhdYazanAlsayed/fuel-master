import React, { useState } from 'react';
import { Dropdown } from 'react-bootstrap';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Permissions } from 'app/core/enums/Permissions';
import DetailsModal from './DetailsModal';
import { Navigate } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _roleManager = DependenciesInjector.services.roleManager;

const Deliveries = ({ deliveries, pagination, setPagination }) => {
  if (!_roleManager.check(Permissions.DeliveryReportShow))
    return <Navigate to="/errors/404" />;

  const [current, setCurrent] = useState(null);
  const [detailsModal, setDetailsModal] = useState(false);

  const columns = [
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data.tank?.station?.arabicName
            : data.tank?.station?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.tank,
      Cell: data => <>{data.tank?.number}</>
    },
    {
      header: _languageService.resources.transport,
      Cell: data => <>{data.transport}</>
    },
    {
      header: _languageService.resources.oldLevel,
      Cell: data => <>{data.tankOldLevel}</>
    },
    {
      header: _languageService.resources.newLevel,
      Cell: data => <>{data.tankNewLevel}</>
    },
    {
      header: _languageService.resources.oldVolume,
      Cell: data => <>{data.tankOldVolume}</>
    },
    {
      header: _languageService.resources.newVolume,
      Cell: data => <>{data.tankNewVolume}</>
    },
    {
      header: 'GL',
      Cell: data => <>{data.gl}</>
    },
    {
      header: '',
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            <Dropdown.Item
              className="cursor-pointer"
              as={'div'}
              onClick={() => handleOpenDetails(data)}
            >
              {_languageService.resources.details}
            </Dropdown.Item>
          </div>
        </CardDropdown>
      )
    }
  ];

  const handleOpenDetails = current => {
    setCurrent(current);
    setDetailsModal(true);
  };

  return (
    <div>
      <FuelMasterTable
        title={_languageService.resources.deliveries}
        columns={columns}
        data={deliveries}
        pagination={pagination}
        setPagination={setPagination}
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        delivery={current}
      />
    </div>
  );
};

export default Deliveries;
