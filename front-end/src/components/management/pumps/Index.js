import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _pumpService = DependenciesInjector.services.pumpService;
const _roleManager = DependenciesInjector.services.roleManager;
const _stationService = DependenciesInjector.services.stationService;
const _identityService = DependenciesInjector.services.identityService;

const Index = () => {
  if (!_roleManager.check(Permissions.PumpsShow))
    return <Navigate to="/errors/404" />;

  // States
  const [stations, setStations] = useState([]);
  const [pumps, setPumps] = useState([]);
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
      Cell: data => <>{data.nozzles?.length}</>
    },
    {
      header: '',
      headerProps: { className: 'text-start' },
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            {_roleManager.check(Permissions.PumpsEdit) && (
              <Dropdown.Item as={Link} to={`/pumps/${data.id}/edit`}>
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}

            {_roleManager.check(Permissions.PumpsDelete) && (
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
          _identityService.currentUser.stationId === null && (
            <Fragment>
              <select className="form-control" onChange={handleOnStationChange}>
                <option value="-1">
                  {_languageService.resources.selectStation}
                </option>

                {stations.map((x, index) => (
                  <option key={index} value={x.id}>
                    {_languageService.isRTL ? x.arabicName : x.englishName}
                  </option>
                ))}
              </select>
            </Fragment>
          )
        }
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
