import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalTop from 'components/shared/ModalTop';
import React from 'react';
import { Button, Modal } from 'react-bootstrap';
import { Navigate } from 'react-router-dom';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const DeleteModal = ({ open, setOpen, id, refresh }) => {
  const _languageService = useService(Services.LanguageService);
  const _stationService = useService(Services.StationService);
  const _permissionService = useService(Services.PermissionService);

  const handleOnDeleteAsync = async e => {
    e.preventDefault();

    const response = await _stationService.deleteAsync(id);
    if (!response.succeeded) return;

    refresh();
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
