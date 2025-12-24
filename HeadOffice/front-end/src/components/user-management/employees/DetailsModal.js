import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { Scope } from 'app/core/abstracts/Scope';

const DetailsModal = ({ open, setOpen, employee }) => {
  const _languageService = useService(Services.LanguageService);

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
              <Form.Label>{_languageService.resources.role}</Form.Label>
              <Form.Control
                type="text"
                value={
                  employee.role === null
                    ? ' - '
                    : _languageService.isRTL
                    ? employee.role.arabicName
                    : employee.role.englishName
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

            {employee.scope === Scope.ALL ? null : employee.scope ===
              Scope.City ? (
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.city}</Form.Label>
                <Form.Control
                  type="text"
                  value={
                    employee.city === null
                      ? ' - '
                      : _languageService.isRTL
                      ? employee.city.arabicName
                      : employee.city.englishName
                  }
                  readOnly
                />
              </Form.Group>
            ) : employee.scope === Scope.Area ? (
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.area}</Form.Label>
                <Form.Control
                  type="text"
                  value={
                    employee.area === null
                      ? ' - '
                      : _languageService.isRTL
                      ? employee.area.arabicName
                      : employee.area.englishName
                  }
                  readOnly
                />
              </Form.Group>
            ) : employee.scope === Scope.Station ? (
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
            ) : null}
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
