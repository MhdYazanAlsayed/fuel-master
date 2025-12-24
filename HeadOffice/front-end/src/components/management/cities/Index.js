import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';
import CreateModal from './CreateModal';
import EditModal from './EditModal';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _cityService = useService(Services.CityService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to={'/errors/404'} />;

  // Permissions
  const canManageCities = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );

  // States
  const [cities, setCities] = useState([]);
  const [loading, setLoading] = useState(true);
  const [createModal, setCreateModal] = useState(false);
  const [deleteModal, setDeleteModal] = useState(false);
  const [editModal, setEditModal] = useState(false);
  const [current, setCurrent] = useState(null);
  const [currentCity, setCurrentCity] = useState(null);

  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });
  const columns = [
    {
      header: _languageService.resources.arabicName,
      Cell: data => <>{data?.arabicName}</>
    },
    {
      header: _languageService.resources.englishName,
      Cell: data => <>{data?.englishName}</>
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data =>
        canManageCities && (
          <CardDropdown>
            <div className="py-2">
              <Dropdown.Item
                as={'div'}
                className="cursor-pointer"
                onClick={() => handleOpenEditModal(data)}
              >
                {_languageService.resources.edit}
              </Dropdown.Item>

              <Dropdown.Item
                className="text-danger cursor-pointer"
                as={'div'}
                onClick={() => handleOpenDeleteModal(data.id)}
              >
                {_languageService.resources.delete}
              </Dropdown.Item>
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
    const response = await _cityService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setCities(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = id => {
    setCurrent(id);
    setDeleteModal(true);
  };

  const handleOpenEditModal = city => {
    setCurrentCity(city);
    setEditModal(true);
  };

  const handleUpdateCity = updatedCity => {
    setCities(prev =>
      prev.map(city => (city.id === updatedCity.id ? updatedCity : city))
    );
    setCurrentCity(updatedCity);
  };

  const handleRefreshPage = () => {
    handleGetPaginationAsync();
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.cities}
        data={cities.sort((a, b) => a.arabicName.localeCompare(b.arabicName))}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
        buttons={
          canManageCities && (
            <Fragment>
              <button
                className="btn btn-primary"
                onClick={() => setCreateModal(true)}
              >
                <i className="fa-solid fa-plus"></i>
              </button>
            </Fragment>
          )
        }
      />

      {canManageCities && (
        <>
          <CreateModal
            open={createModal}
            setOpen={setCreateModal}
            handleRefreshPage={handleRefreshPage}
          />

          <EditModal
            open={editModal}
            setOpen={setEditModal}
            city={currentCity}
            handleUpdateCity={handleUpdateCity}
          />

          <DeleteModal
            open={deleteModal}
            setOpen={setDeleteModal}
            id={current}
            handleRefreshPage={handleRefreshPage}
          />
        </>
      )}
    </Loader>
  );
};

export default Index;
