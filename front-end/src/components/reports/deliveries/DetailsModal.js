import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const DetailsModal = ({ delivery, open, setOpen }) => {
  return delivery === null ? null : (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.deliveryDetails}
    >
      <Modal.Body>
        <div className="row">
          <div className="col-md-6 mb-2">
            {/* Transport */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.transport}</Form.Label>
              <Form.Control value={delivery.transport} readOnly />
            </Form.Group>

            {/* PaidVolume */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.paidVolume}</Form.Label>
              <Form.Control value={delivery.paidVolume} readOnly />
            </Form.Group>

            {/* TankOldLevel */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.oldLevel}</Form.Label>
              <Form.Control value={delivery.tankOldLevel} readOnly />
            </Form.Group>

            {/* TankOldVolume */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.oldVolume}</Form.Label>
              <Form.Control value={delivery.tankOldVolume} readOnly />
            </Form.Group>

            {/* Tank */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.tank}</Form.Label>
              <Form.Control
                value={
                  delivery.tank.number +
                  ' ' +
                  _languageService.resources.fuelTypes[delivery.tank.fuelType]
                }
                readOnly
              />
            </Form.Group>

            {/* CreatedAt */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.dateTime}</Form.Label>
              <Form.Control
                value={new Date(delivery.createdAt).toLocaleString()}
                readOnly
              />
            </Form.Group>
          </div>

          <div className="col-md-6 mb-2">
            {/* InvoiceNumber */}
            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.invoiceNumber}
              </Form.Label>
              <Form.Control value={delivery.invoiceNumber} readOnly />
            </Form.Group>

            {/* RecivedVolume */}
            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.receivedVolume}
              </Form.Label>
              <Form.Control value={delivery.recivedVolume} readOnly />
            </Form.Group>

            {/* TankNewLevel */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.newLevel}</Form.Label>
              <Form.Control value={delivery.tankNewLevel} readOnly />
            </Form.Group>

            {/* TankNewVolume */}
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.newVolume}</Form.Label>
              <Form.Control value={delivery.tankNewVolume} readOnly />
            </Form.Group>

            {/* GL */}
            <Form.Group className="mb-2">
              <Form.Label>{'GL'}</Form.Label>
              <Form.Control value={delivery.gl} readOnly />
            </Form.Group>
          </div>
        </div>
      </Modal.Body>

      <Modal.Footer>
        <Button variant="primary">{_languageService.resources.close}</Button>
      </Modal.Footer>
    </ModalCenter>
  );
};

export default DetailsModal;
