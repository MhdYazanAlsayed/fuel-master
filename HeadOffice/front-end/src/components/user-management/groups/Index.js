import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import React, { useEffect, useState } from 'react';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Link, Navigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _groupService = DependenciesInjector.services.groupService;
const _roleManager = DependenciesInjector.services.roleManager;

const Index = () => {
  if (!_roleManager.check(Permissions.GroupsShow))
    return <Navigate to="/errors/404" />;

  const [groups, setGroups] = useState([]);
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
            {_roleManager.check(Permissions.GroupsEdit) && (
              <Dropdown.Item as={Link} to={`/groups/${data.id}/edit`}>
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}

            {_roleManager.check(Permissions.EditPermissions) && (
              <Dropdown.Item as={Link} to={`/groups/${data.id}/permissions`}>
                {_languageService.resources.permissions}
              </Dropdown.Item>
            )}
          </div>
        </CardDropdown>
      )
    }
  ];

  useEffect(() => {
    handleGetGroupsAsync();
  }, []);

  const handleGetGroupsAsync = async () => {
    setGroups(await _groupService.getAllAsync());
    setLoading(false);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.groups}
        data={groups}
        columns={columns}
        pagination={pagination}
        setPagination={setPagination}
      />
    </Loader>
  );
};

export default Index;
