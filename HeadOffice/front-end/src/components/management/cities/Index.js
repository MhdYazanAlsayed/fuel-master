import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
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

const _languageService = DependenciesInjector.services.languageService;
const _cityService = DependenciesInjector.services.cityService;
const _roleManager = DependenciesInjector.services.roleManager;

const Index = () => {
  if (!_roleManager.check(Permissions.CitiesShow))
    return <Navigate to={'/errors/404'} />;

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
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            {_roleManager.check(Permissions.CitiesEdit) && (
              <Dropdown.Item
                as={'div'}
                className="cursor-pointer"
                onClick={() => handleOpenEditModal(data)}
              >
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}

            {data.canDelete && _roleManager.check(Permissions.CitiesDelete) && (
              <Dropdown.Item
                className="text-danger cursor-pointer"
                as={'div'}
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
          <Fragment>
            <button
              className="btn btn-primary"
              onClick={() => setCreateModal(true)}
            >
              <i className="fa-solid fa-plus"></i>
            </button>
          </Fragment>
        }
      />

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
    </Loader>
  );
};

export default Index;
