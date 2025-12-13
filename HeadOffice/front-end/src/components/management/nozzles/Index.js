import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Badge, Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import DetailsModal from './DetailsModal';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

// Dependencies
const _languageService = DependenciesInjector.services.languageService;
const _nozzleService = DependenciesInjector.services.nozzleService;
const _roleManager = DependenciesInjector.services.roleManager;

const Index = () => {
  if (!_roleManager.check(Permissions.NozzlesShow))
    return <Navigate to="/errors/404" />;

  // States
  const [nozzles, setNozzles] = useState([]);
  const [deleteModal, setDeleteModal] = useState(false);
  const [detailsModal, setDetailsModal] = useState(false);
  const [loading, setLoading] = useState(true);
  const [current, setCurrent] = useState(null);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  const columns = [
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.pump?.station?.arabicName
            : data?.pump?.station?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.tank,
      Cell: data => (
        <>
          {data.tank?.number +
            ' — ' +
            _languageService.resources.fuelTypes[data.tank?.fuelType]}
        </>
      )
    },
    {
      header: _languageService.resources.pump,
      Cell: data => <>{data.pump?.number}</>
    },
    {
      header: _languageService.resources.number,
      Cell: data => <>{data.number}</>
    },
    {
      header: _languageService.resources.nozzleStatus,
      Cell: data => (
        <Badge bg={data.status === 0 ? 'secondary' : 'info'}>
          {data.status === 0
            ? _languageService.resources.nozzleStatuses[data.status]
            : _languageService.resources.inService}
        </Badge>
      )
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            <Dropdown.Item
              as={'div'}
              className="cursor-pointer"
              onClick={() => handleOpenDetailsModal(data)}
            >
              {_languageService.resources.details}
            </Dropdown.Item>

            {_roleManager.check(Permissions.NozzlesEdit) && (
              <Dropdown.Item as={Link} to={`/nozzles/${data.id}/edit`}>
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}

            {_roleManager.check(Permissions.NozzlesDelete) && (
              <Dropdown.Item
                as="div"
                className="cursor-pointer text-danger"
                onClick={() => handleOpenDeleteModal(data)}
              >
                {_languageService.resources.delete}
              </Dropdown.Item>
            )}
          </div>
        </CardDropdown>
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
    const response = await _nozzleService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    handleSortNozzles(response.data);
    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setNozzles(response.data);
    setLoading(false);
  };

  const handleSortNozzles = arr => {
    return _languageService.isRTL ? sortArabic() : sortEnglish();
    function sortArabic() {
      return arr.sort((a, b) => {
        if (a.tank.station.arabicName < b.tank.station.arabicName) {
          return -1;
        }
        if (a.tank.station.arabicName > b.tank.station.arabicName) {
          return 1;
        }
        return 0;
      });
    }
    function sortEnglish() {
      return arr.sort((a, b) => {
        if (a.tank.station.englishName < b.tank.station.englishName) {
          return -1;
        }
        if (a.tank.station.englishName > b.tank.station.englishName) {
          return 1;
        }
        return 0;
      });
    }
  };

  const handleOpenDeleteModal = data => {
    setCurrent(data);
    setDeleteModal(true);
  };

  const handleOpenDetailsModal = data => {
    setCurrent(data);
    setDetailsModal(true);
  };

  const handleRefershDelete = () => {
    setNozzles(prev => {
      const index = prev.findIndex(x => x.id === current.id);
      if (index === -1) throw Error();

      prev.splice(index, 1);

      return [...prev];
    });
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.nozzles}
        data={nozzles}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        nozzle={current}
      />

      <DeleteModal
        open={deleteModal}
        setOpen={setDeleteModal}
        id={current?.id}
        refresh={handleRefershDelete}
      />
    </Loader>
  );
};

export default Index;
