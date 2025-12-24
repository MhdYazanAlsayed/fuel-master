import ModalTop from 'components/shared/ModalTop';
import React from 'react';
import { Button, Modal } from 'react-bootstrap';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const DeleteModal = ({ open, setOpen, id, refresh }) => {
  const _languageService = useService(Services.LanguageService);
  const _pumpService = useService(Services.PumpService);

  const handleOnDeleteAsync = async e => {
    e.preventDefault();

    const response = await _pumpService.deleteAsync(id);
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
