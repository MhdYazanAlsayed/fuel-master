import React, { Fragment, useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import CardDropdown from 'components/theme/common/CardDropdown';
import { Dropdown } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { Permissions } from 'app/core/enums/Permissions';
import CreateModal from './CreateModal';
import EditModal from './EditModal';

const _languageService = DependenciesInjector.services.languageService;
const _fuelTypeService = DependenciesInjector.services.fuelTypeService;
const _roleManager = DependenciesInjector.services.roleManager;

const FuelTypes = () => {
  if (!_roleManager.check(Permissions.FuelTypesShow))
    return <Navigate to="/errors/404" />;

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
      Cell: data => (
        <CardDropdown>
          <div className="py-2">
            {_roleManager.check(Permissions.FuelTypesEdit) && (
              <Dropdown.Item
                as="div"
                className="cursor-pointer"
                onClick={() => handleOpenEditModal(data)}
              >
                {_languageService.resources.edit}
              </Dropdown.Item>
            )}
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
        handleAddFuelType={handleAddFuelType}
      />

      <EditModal
        open={editModal}
        setOpen={setEditModal}
        fuelType={currentFuelType}
        handleUpdateFuelType={handleUpdateFuelType}
      />
    </Loader>
  );
};

export default FuelTypes;
