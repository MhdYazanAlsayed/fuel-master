import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _roleManager = DependenciesInjector.services.roleManager;
const _identityService = DependenciesInjector.services.identityService;

const Index = () => {
  if (!_roleManager.check(Permissions.StationsShow))
    return <Navigate to="/errors/404" />;

  // States
  const [stations, setStations] = useState([]);
  const [deleteModal, setDeleteModal] = useState(false);
  const [loading, setLoading] = useState(true);
  const [current, setCurrent] = useState(null);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  const columns = [
    {
      header: _languageService.resources.id,
      Cell: data => <>{data?.id}</>
    },
    {
      header: _languageService.resources.arabicName,
      Cell: data => <>{data?.arabicName}</>
    },
    {
      header: _languageService.resources.englishName,
      Cell: data => <>{data?.englishName}</>
    },
    {
      header: _languageService.resources.city,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.city?.arabicName
            : data?.city?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.zone,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.zone?.arabicName
            : data?.zone?.englishName}
        </>
      )
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data =>
        _identityService.currentUser.stationId === null && (
          <CardDropdown>
            <div className="py-2">
              {_roleManager.check(Permissions.StationsEdit) && (
                <Dropdown.Item as={Link} to={`/stations/${data.id}/edit`}>
                  {_languageService.resources.edit}
                </Dropdown.Item>
              )}

              {_roleManager.check(Permissions.StationsDelete) && (
                <Dropdown.Item
                  as="div"
                  className="cursor-pointer text-danger"
                  onClick={() => handleOpenDeleteModal(data.id)}
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
    const response = await _stationService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setStations(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = id => {
    setCurrent(id);
    setDeleteModal(true);
  };

  const handleRefershDelete = () => {
    setStations(prev => {
      const index = prev.findIndex(x => x.id === current);
      if (index === -1) throw Error();

      prev.splice(index, 1);

      return [...prev];
    });
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.stations}
        data={stations}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
      />

      <DeleteModal
        open={deleteModal}
        setOpen={setDeleteModal}
        id={current}
        refresh={handleRefershDelete}
      />
    </Loader>
  );
};

export default Index;
