import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import CreateModal from './CreateModal';
import EditModal from './EditModal';
import Loader from 'components/shared/Loader';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _pumpService = useService(Services.PumpService);
  const _stationService = useService(Services.StationService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to="/errors/404" />;

  const canManagePumps = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );
  // States
  const [stations, setStations] = useState([]);
  const [pumps, setPumps] = useState([]);
  const [deleteModal, setDeleteModal] = useState(false);
  const [createModal, setCreateModal] = useState(false);
  const [editModal, setEditModal] = useState(false);
  const [currentPump, setCurrentPump] = useState(null);
  const [loading, setLoading] = useState(true);
  const [current, setCurrent] = useState(null);
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
      header: _languageService.resources.manufacturer,
      Cell: data => <>{data.manufacturer}</>
    },
    {
      header: _languageService.resources.nozzlesCount,
      Cell: data => <>{data.nozzleCount}</>
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data =>
        canManagePumps && (
          <CardDropdown>
            <div className="py-2">
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
    const response = await _stationService.getAllAsync();
    if (!response) return;
    setStations(response);
  };

  const handleGetPaginationAsync = async stationId => {
    const response = await _pumpService.getPaginationAsync(
      pagination.currentPage,
      stationId
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setPumps(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = id => {
    setCurrent(id);
    setDeleteModal(true);
  };

  const handleRefershDelete = () => {
    setPumps(prev => {
      const index = prev.findIndex(x => x.id === current);
      if (index === -1) throw Error();

      prev.splice(index, 1);

      return [...prev];
    });
  };

  const handleRefreshPage = () => {
    handleGetPaginationAsync();
  };

  const handleOpenEditModal = pump => {
    setCurrentPump(pump);
    setEditModal(true);
  };

  const handleUpdatePump = updatedPump => {
    setPumps(prev =>
      prev.map(pump => (pump.id === updatedPump.id ? updatedPump : pump))
    );
    setCurrentPump(updatedPump);
  };

  const handleOnStationChange = async e => {
    const stationId = e.target.value;
    if (stationId === '-1') {
      setPagination(prev => ({
        ...prev,
        currentPage: 1,
        perform: false
      }));
      await handleGetPaginationAsync(null);
      return;
    }

    setPagination(prev => ({
      ...prev,
      currentPage: 1,
      perform: false
    }));
    await handleGetPaginationAsync(stationId);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.pumps}
        data={pumps}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
        buttons={
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
            {canManagePumps && (
              <button
                className="btn btn-primary ms-2"
                onClick={() => setCreateModal(true)}
              >
                <i className="fa-solid fa-plus"></i>
              </button>
            )}
          </Fragment>
        }
      />

      {canManagePumps && (
        <>
          <CreateModal
            open={createModal}
            setOpen={setCreateModal}
            handleRefreshPage={handleRefreshPage}
          />

          <EditModal
            open={editModal}
            setOpen={setEditModal}
            pump={currentPump}
            handleUpdatePump={handleUpdatePump}
          />

          <DeleteModal
            open={deleteModal}
            setOpen={setDeleteModal}
            id={current}
            refresh={handleRefershDelete}
          />
        </>
      )}
    </Loader>
  );
};

export default Index;
