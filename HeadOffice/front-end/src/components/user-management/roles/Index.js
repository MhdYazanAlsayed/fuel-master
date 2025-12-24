import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import DeleteModal from './DeleteModal';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const Index = () => {
  const _languageService = useService(Services.LanguageService);
  const _roleService = useService(Services.RoleService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to={'/errors/404'} />;

  // Permissions
  const canManageRoles = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );

  // States
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleteModal, setDeleteModal] = useState(false);
  const [current, setCurrent] = useState(null);

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
            <Dropdown.Item as={Link} to={`/roles/${data.id}/details`}>
              {_languageService.resources.details}
            </Dropdown.Item>

            {canManageRoles && (
              <Fragment>
                <Dropdown.Item as={Link} to={`/roles/${data.id}/edit`}>
                  {_languageService.resources.edit}
                </Dropdown.Item>

                <Dropdown.Item
                  className="text-danger cursor-pointer"
                  as={'div'}
                  onClick={() => handleOpenDeleteModal(data.id)}
                >
                  {_languageService.resources.delete}
                </Dropdown.Item>
              </Fragment>
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
    const response = await _roleService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setRoles(response.data);
    setLoading(false);
  };

  const handleOpenDeleteModal = id => {
    setCurrent(id);
    setDeleteModal(true);
  };

  const handleRefreshPage = () => {
    handleGetPaginationAsync();
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.roles}
        data={roles.sort((a, b) => a.arabicName.localeCompare(b.arabicName))}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
        buttons={
          canManageRoles && (
            <Fragment>
              <Link to="/roles/create" className="btn btn-primary">
                <i className="fa-solid fa-plus"></i>
              </Link>
            </Fragment>
          )
        }
      />

      {canManageRoles && (
        <DeleteModal
          open={deleteModal}
          setOpen={setDeleteModal}
          id={current}
          handleRefreshPage={handleRefreshPage}
        />
      )}
    </Loader>
  );
};

export default Index;

