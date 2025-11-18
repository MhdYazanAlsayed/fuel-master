import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalTop from 'components/shared/ModalTop';
import React from 'react';
import { Button, Modal } from 'react-bootstrap';
// import { Navigate } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _citySerivce = DependenciesInjector.services.cityService;
// const _roleManager = DependenciesInjector.services.roleManager;

const DeleteModal = ({ open, setOpen, id, handleRefreshPage }) => {
  // if (!_roleManager.check(Permissions.CitiesDelete))
  //   return <Navigate to="/errors/404" />;

  const handleOnDeleteAsync = async e => {
    e.preventDefault();

    const response = await _citySerivce.deleteAsync(id);

    if (!response.succeeded) return;

    handleRefreshPage();
    setOpen(false);
  };

  return (
    <ModalTop
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.deleteMsg}
    >
      <Modal.Body>
        <div className="d-flex align-items-center gap-2">
          <form onSubmit={handleOnDeleteAsync}>
            <Button variant="danger" type="submit">
              {_languageService.resources.delete}
            </Button>
          </form>

          <Button variant="secondary" onClick={() => setOpen(false)}>
            {_languageService.resources.cancle}
          </Button>
        </div>
      </Modal.Body>
    </ModalTop>
  );
};

export default DeleteModal;
