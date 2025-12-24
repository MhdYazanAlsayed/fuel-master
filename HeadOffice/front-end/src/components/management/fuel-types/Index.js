import React, { Fragment, useEffect, useState } from 'react';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import CreateModal from './CreateModal';
import EditModal from './EditModal';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const FuelTypes = () => {
  const _languageService = useService(Services.LanguageService);
  const _fuelTypeService = useService(Services.FuelTypeService);
  const _permissionService = useService(Services.PermissionService);

  if (!_permissionService.check(AreaOfAccess.ConfigurationView))
    return <Navigate to="/errors/404" />;

  // Permissions
  const canManageFuelTypes = _permissionService.check(
    AreaOfAccess.ConfigurationManage
  );

  const [fuelTypes, setFuelTypes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [createModal, setCreateModal] = useState(false);
  const [editModal, setEditModal] = useState(false);
  const [currentFuelType, setCurrentFuelType] = useState(null);

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
        canManageFuelTypes && (
          <CardDropdown>
            <div className="py-2">
              <Dropdown.Item
                as="div"
                className="cursor-pointer"
                onClick={() => handleOpenEditModal(data)}
              >
                {_languageService.resources.edit}
              </Dropdown.Item>
            </div>
          </CardDropdown>
        )
    }
  ];

  useEffect(() => {
    handleGetAllAsync();
  }, []);

  const handleGetAllAsync = async () => {
    setLoading(true);
    const response = await _fuelTypeService.getAllAsync();
    setFuelTypes(response ?? []);
    setLoading(false);
  };

  const handleOpenEditModal = fuelType => {
    setCurrentFuelType(fuelType);
    setEditModal(true);
  };

  const handleAddFuelType = fuelType => {
    setFuelTypes(prev => [...prev, fuelType]);
  };

  const handleUpdateFuelType = updatedFuelType => {
    setFuelTypes(prev =>
      prev.map(item =>
        item.id === updatedFuelType.id ? updatedFuelType : item
      )
    );
    setCurrentFuelType(updatedFuelType);
  };

  return (
    <Loader loading={loading}>
      <FuelMasterTable
        title={_languageService.resources.fuelTypesText}
        data={fuelTypes}
        columns={columns}
        buttons={
          canManageFuelTypes && (
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

      {canManageFuelTypes && (
        <>
          <CreateModal
            open={createModal}
            setOpen={setCreateModal}
            handleAddFuelType={handleAddFuelType}
          />

          <EditModal
            open={editModal}
            setOpen={setEditModal}
            fuelType={currentFuelType}
            handleUpdateFuelType={handleUpdateFuelType}
          />
        </>
      )}
    </Loader>
  );
};

export default FuelTypes;
