import React from 'react';
import { Modal } from 'react-bootstrap';
import FalconCloseButton from 'components/theme/common/FalconCloseButton';

const ModalTop = ({ open, setOpen, title, children }) => {
  return (
    <>
      <Modal
        show={open}
        onHide={() => setOpen(false)}
        backdrop="static"
        keyboard={false}
      >
        <Modal.Header>
          <Modal.Title>{title}</Modal.Title>
          <FalconCloseButton onClick={() => setOpen(false)} />
        </Modal.Header>
        {children}
      </Modal>
    </>
  );
};

export default ModalTop;
