import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState, Fragment } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown, Form } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import EditPasswordModal from './EditPasswordModal';
import Loader from 'components/shared/Loader';
import DetailsModal from './DetailsModal';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _employeeService = DependenciesInjector.services.employeeSerivce;
const _roleManager = DependenciesInjector.services.roleManager;

const Index = () => {
  if (!_roleManager.check(Permissions.EmployeesShow))
    return <Navigate to="/errors/404" />;

  // States
  const [employees, setEmployees] = useState([]);
  const [editPasswordModal, setEditPasswordModal] = useState(false);
  const [detailsModal, setDetailsModal] = useState(false);
  const [current, setCurrent] = useState(null);
  const [loading, setLoading] = useState(true);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pages: 0,
    perform: false
  });

  const columns = [
    {
      header: _languageService.resources.fullName,
      Cell: data => <>{data?.fullName}</>
    },
    {
      header: _languageService.resources.userName,
      Cell: data => <>{data?.user?.userName}</>
    },
    {
      header: _languageService.resources.group,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data?.user?.group?.arabicName
            : data?.user?.group?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.cardNumber,
      Cell: data => <>{data?.cardNumber}</>
    },
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {data.station === null
            ? ' - '
            : _languageService.isRTL
            ? data?.station?.arabicName
            : data?.station?.englishName}
        </>
      )
    },
    {
      header: _languageService.resources.isActive,
      Cell: data => (
        <Form.Check checked={data.user.isActive ?? false} disabled />
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

            {_roleManager.check(Permissions.EmployeesEdit) && (
              <Fragment>
                <Dropdown.Item as={Link} to={`/employees/${data.id}/edit`}>
                  {_languageService.resources.edit}
                </Dropdown.Item>

                <Dropdown.Item
                  as={'div'}
                  onClick={() => handleOpenEditPasswordModal(data)}
                  className={'cursor-pointer'}
                >
                  {_languageService.resources.editPassword}
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
    const response = await _employeeService.getPaginationAsync(
      pagination.currentPage
    );
    if (!response) return;

    setPagination(prev => ({
      ...prev,
      pages: response.pages,
      perform: false
    }));
    setEmployees(response.data);
    setLoading(false);
  };

  const handleOpenDetailsModal = item => {
    setCurrent(item);
    setDetailsModal(true);
  };

  const handleOpenEditPasswordModal = item => {
    setCurrent(item);
    setEditPasswordModal(true);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.employees}
        data={employees}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
      />

      <DetailsModal
        open={detailsModal}
        setOpen={setDetailsModal}
        employee={current}
      />

      <EditPasswordModal
        open={editPasswordModal}
        setOpen={setEditPasswordModal}
        employeeId={current?.id}
      />
    </Loader>
  );
};

export default Index;
