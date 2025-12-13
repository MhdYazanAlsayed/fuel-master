import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { Fragment, useEffect, useState } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import CreateModal from './CreateModal';
import EditModal from './EditModal';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _zoneService = DependenciesInjector.services.zoneService;
const _roleManager = DependenciesInjector.services.roleManager;

const Index = () => {
  // if (!_roleManager.check(Permissions.ZonesShow))
  //   return <Navigate to="/errors/404" />;

  // States
  const [zones, setZones] = useState([]);
  const [deleteModal, setDeleteModal] = useState(false);
  const [createModal, setCreateModal] = useState(false);
  const [editModal, setEditModal] = useState(false);
  const [current, setCurrent] = useState(null);
  const [currentZone, setCurrentZone] = useState(null);
  const [loading, setLoading] = useState(true);
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
            {_roleManager.check(Permissions.ZonesEdit) && (
              <Dropdown.Item
                as="div"
                className="cursor-pointer"
                onClick={() => handleOpenEditModal(data)}
              >
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}

            {_roleManager.check(Permissions.ShowPrices) && (
              <Dropdown.Item as={Link} to={`/zones/${data.id}/prices`}>
                {_languageService.resources.zonePrices}
              </Dropdown.Item>
            )}

            {_roleManager.check(Permissions.ChangePrices) && (
              <Dropdown.Item as={Link} to={`/zones/${data.id}/change-prices`}>
                {_languageService.resources.changePrices}
              </Dropdown.Item>
            )}

            {data.canDelete && _roleManager.check(Permissions.ZonesDelete) && (
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
    const response = await _zoneService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setZones(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = id => {
    setCurrent(id);
    setDeleteModal(true);
  };

  const handleOpenEditModal = zone => {
    setCurrentZone(zone);
    setEditModal(true);
  };

  const handleRefershDelete = () => {
    setZones(prev => {
      const index = prev.findIndex(x => x.id === current);
      if (index === -1) throw Error();

      prev.splice(index, 1);

      return [...prev];
    });
  };

  const handleRefreshData = () => {
    handleGetPaginationAsync();
  };

  const handleUpdateZone = updatedZone => {
    setZones(prev =>
      prev.map(zone => (zone.id === updatedZone.id ? updatedZone : zone))
    );
    setCurrentZone(updatedZone);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.zones}
        data={zones}
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
        handleRefreshData={handleRefreshData}
      />

      <EditModal
        open={editModal}
        setOpen={setEditModal}
        zone={currentZone}
        handleUpdateZone={handleUpdateZone}
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
