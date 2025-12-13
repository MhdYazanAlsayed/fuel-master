import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';

const DetailsModal = ({ open, setOpen, nozzle }) => {
  const _languageService = DependenciesInjector.services.languageService;

  return !nozzle ? null : (
    <ModalCenter
      {...{ open, setOpen }}
      title={_languageService.resources.nozzleDetails}
    >
      <Modal.Body>
        <div className="row">
          <div className="col-md-6 pe-2">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.station}</Form.Label>
              <Form.Control
                readOnly
                value={
                  _languageService.isRTL
                    ? nozzle?.pump?.station?.arabicName
                    : nozzle?.pump?.station?.englishName
                }
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.pump}</Form.Label>
              <Form.Control readOnly value={nozzle.pump?.number} />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.status}</Form.Label>
              <Form.Control
                readOnly
                value={_languageService.resources.nozzleStatuses[nozzle.status]}
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.amount}</Form.Label>
              <Form.Control readOnly value={nozzle.amount} />
            </Form.Group>
          </div>
          <div className="col-md-6 ps-2">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.tank}</Form.Label>
              <Form.Control readOnly value={nozzle.tank?.number} />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.number}</Form.Label>
              <Form.Control readOnly value={nozzle.number} />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.readerNumber}</Form.Label>
              <Form.Control readOnly value={nozzle.readerNumber ?? ' ----- '} />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.volume}</Form.Label>
              <Form.Control readOnly value={nozzle.volume} />
            </Form.Group>
          </div>

          <div className="col-md-12">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.totalizer}</Form.Label>
              <Form.Control readOnly value={nozzle.totalizer} />
            </Form.Group>
          </div>
        </div>
      </Modal.Body>

      <Modal.Footer>
        <Button variant="primary" onClick={() => setOpen(false)}>
          {_languageService.resources.close}
        </Button>
      </Modal.Footer>
    </ModalCenter>
  );
};

export default DetailsModal;
