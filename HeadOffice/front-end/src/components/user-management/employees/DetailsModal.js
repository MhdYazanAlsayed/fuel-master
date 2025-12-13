import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const DetailsModal = ({ open, setOpen, employee }) => {
  return employee === null ? null : (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.details}
    >
      <Modal.Body>
        <div className="row">
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.fullName}</Form.Label>
              <Form.Control type="text" value={employee.fullName} readOnly />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.phoneNumber}</Form.Label>
              <Form.Control
                type="text"
                value={employee.phoneNumber ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.identificationNumber}
              </Form.Label>
              <Form.Control
                type="text"
                value={employee.identificationNumber ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.cardNumber}</Form.Label>
              <Form.Control
                type="text"
                value={employee.cardNumber ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.group}</Form.Label>
              <Form.Control
                type="text"
                value={
                  employee.user?.group === null
                    ? ' - '
                    : _languageService.isRTL
                    ? employee.user.group.arabicName
                    : employee.user.group.englishName
                }
                readOnly
              />
            </Form.Group>
          </div>
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.emailAddress}</Form.Label>
              <Form.Control
                type="text"
                value={employee.emailAddress ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.age}</Form.Label>
              <Form.Control
                type="text"
                value={employee.age ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.address}</Form.Label>
              <Form.Control
                type="text"
                value={employee.address ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.station}</Form.Label>
              <Form.Control
                type="text"
                value={
                  employee.station === null
                    ? ' - '
                    : _languageService.isRTL
                    ? employee.station.arabicName
                    : employee.station.englishName
                }
                readOnly
              />
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
