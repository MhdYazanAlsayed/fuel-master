import { Modal } from 'react-bootstrap';
import React from 'react';

function ModalCenter({ open, setOpen, title, children }) {
  return (
    <>
      <Modal
        show={open}
        onHide={() => setOpen(false)}
        size="lg"
        aria-labelledby="contained-modal-title-vcenter"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title id="contained-modal-title-vcenter">{title}</Modal.Title>
        </Modal.Header>
        {children}
      </Modal>
    </>
  );
}

export default ModalCenter;
