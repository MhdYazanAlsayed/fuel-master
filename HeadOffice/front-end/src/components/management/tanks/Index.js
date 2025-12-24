import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import DetailsModal from './DetailsModal';
import CreateModal from './CreateModal';
import EditModal from './EditModal';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _tankService = useService(Services.TankService);
  const _permissionService = useService(Services.PermissionService);
  const _stationService = useService(Services.StationService);
  const _identityService = useService(Services.IdentityService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to="/errors/404" />;

  const canManageTanks = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );
  // States
  const [stations, setStations] = useState([]);
  const [tanks, setTanks] = useState([]);
  const [deleteModal, setDeleteModal] = useState(false);
  const [detailsModal, setDetailsModal] = useState(false);
  const [createModal, setCreateModal] = useState(false);
  const [editModal, setEditModal] = useState(false);
  const [currentTank, setCurrentTank] = useState(null);
  const [current, setCurrent] = useState(null);
  const [loading, setLoading] = useState(true);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  const columns = [
    {
      header: _languageService.resources.number,
      Cell: data => <>{data?.number}</>
    },
    {
      header: _languageService.resources.fuelType,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.fuelType?.arabicName
            : data?.fuelType?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.station?.arabicName
            : data?.station?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.currentLevel,
      Cell: data => <>{data.currentLevel}</>
    },
    {
      header: _languageService.resources.currentVolume,
      Cell: data => <>{data.currentVolume}</>
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            <Dropdown.Item
              as="div"
              className="cursor-pointer"
              onClick={() => handleOpenDetailsModal(data)}
            >
              {_languageService.resources.details}
            </Dropdown.Item>
            {canManageTanks && (
              <>
                <Dropdown.Item
                  as="div"
                  className="cursor-pointer"
                  onClick={() => handleOpenEditModal(data)}
                >
                  {_languageService.resources.edit}
                </Dropdown.Item>

                <Dropdown.Item
                  as="div"
                  className="cursor-pointer text-danger"
                  onClick={() => handleOpenDeleteModal(data)}
                >
                  {_languageService.resources.delete}
                </Dropdown.Item>
              </>
            )}
          </div>
        </CardDropdown>
      )
    }
  ];

  useEffect(() => {
    handleOnLoadAsync();
  }, []);

  useEffect(() => {
    if (!pagination.perform) return;

    handleGetPaginationAsync();
  }, [pagination.perform]);

  const handleOnLoadAsync = async () => {
    setLoading(true);
    await handleGetStationsAsync();
    await handleGetPaginationAsync();
    setLoading(false);
  };

  const handleGetStationsAsync = async () => {
    if (_identityService.currentUser.stationId !== null) {
      return;
    }

    const response = await _stationService.getAllAsync();
    if (!response) return;
    setStations(response);
  };

  const handleGetPaginationAsync = async stationId => {
    const response = await _tankService.getPaginationAsync(
      pagination.currentPage,
      stationId
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setTanks(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = entity => {
    setCurrent(entity);
    setDeleteModal(true);
  };

  const handleOpenDetailsModal = entity => {
    setCurrent(entity);
    setDetailsModal(true);
  };

  const handleRefershDelete = () => {
    setTanks(prev => {
      const index = prev.findIndex(x => x.id === current.id);
      if (index === -1) throw Error();

      prev.splice(index, 1);

      return [...prev];
    });
  };

  const handleRefreshPage = () => {
    handleGetPaginationAsync();
  };

  const handleOpenEditModal = tank => {
    setCurrentTank(tank);
    setEditModal(true);
  };

  const handleUpdateTank = updatedTank => {
    setTanks(prev =>
      prev.map(tank => (tank.id === updatedTank.id ? updatedTank : tank))
    );
    setCurrentTank(updatedTank);
  };

  // const handleOnStationChange = async e => {
  //   const stationId = e.target.value;
  //   if (stationId === '-1') {
  //     setPagination(prev => ({
  //       ...prev,
  //       currentPage: 1,
  //       perform: false
  //     }));
  //     await handleGetPaginationAsync(null);
  //     return;
  //   }

  //   setPagination(prev => ({
  //     ...prev,
  //     currentPage: 1,
  //     perform: false
  //   }));
  //   await handleGetPaginationAsync(stationId);
  // };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.tanks}
        data={tanks}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
        buttons={
          canManageTanks && (
            <Fragment>
              {/* <select className="form-control" onChange={handleOnStationChange}>
                <option value="-1">
                  {_languageService.resources.selectStation}
                </option>

                {stations.map((x, index) => (
                  <option key={index} value={x.id}>
                    {_languageService.isRTL ? x.arabicName : x.englishName}
                  </option>
                ))}
              </select> */}
              <button
                className="btn btn-primary ms-2"
                onClick={() => setCreateModal(true)}
              >
                <i className="fa-solid fa-plus"></i>
              </button>
            </Fragment>
          )
        }
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        tank={current}
      />

      {canManageTanks && (
        <>
          <CreateModal
            open={createModal}
            setOpen={setCreateModal}
            handleRefreshPage={handleRefreshPage}
          />

          <EditModal
            open={editModal}
            setOpen={setEditModal}
            tank={currentTank}
            handleUpdateTank={handleUpdateTank}
          />

          <DeleteModal
            open={deleteModal}
            setOpen={setDeleteModal}
            entity={current}
            refresh={handleRefershDelete}
          />
        </>
      )}
    </Loader>
  );
};

export default Index;
