import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Badge, Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import DetailsModal from './DetailsModal';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';
import { Fragment } from 'react';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _nozzleService = useService(Services.NozzleService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to="/errors/404" />;

  const canManageNozzles = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );

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
            ? data?.tank?.station?.arabicName
            : data?.tank?.station?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.tank,
      Cell: data => {
        return (
          <>
            {`${data.tank.number} - ${
              _languageService.isRTL
                ? data.fuelType.arabicName
                : data.fuelType.englishName
            }`}
            {/* {data.tank.number + ' — ' + _languageService.isRTL
              ? data.fuelType.arabicName
              : data.fuelType.englishName} */}
          </>
        );
      }
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

            {canManageNozzles && (
              <>
                <Dropdown.Item as={Link} to={`/nozzles/${data.id}/edit`}>
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
        buttons={
          canManageNozzles && (
            <Fragment>
              <Link to="/nozzles/create" className="btn btn-primary">
                <i className="fa-solid fa-plus"></i>
              </Link>
            </Fragment>
          )
        }
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        nozzle={current}
      />

      {canManageNozzles && (
        <DeleteModal
          open={deleteModal}
          setOpen={setDeleteModal}
          id={current?.id}
          refresh={handleRefershDelete}
        />
      )}
    </Loader>
  );
};

export default Index;
