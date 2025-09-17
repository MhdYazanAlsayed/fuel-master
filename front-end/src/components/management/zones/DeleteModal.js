import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalTop from 'components/shared/ModalTop';
import React from 'react';
import { Button, Modal } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;
const _zoneService = DependenciesInjector.services.zoneService;
const _roleManager = DependenciesInjector.services.roleManager;

const DeleteModal = ({ open, setOpen, id, refresh }) => {
  if (!_roleManager.check(Permissions.ZonesDelete)) return <></>;

  const handleOnDeleteAsync = async e => {
    e.preventDefault();

    const response = await _zoneService.deleteAsync(id);
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
