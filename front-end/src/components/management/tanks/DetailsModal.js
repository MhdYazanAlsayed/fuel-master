import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const DetailsModal = ({ open, setOpen, tank }) => {
  console.log(tank);
  return tank === null ? null : (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.details}
    >
      <Modal.Body>
        <div className="row">
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.number}</Form.Label>
              <Form.Control type="text" value={tank.number} readOnly />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.fuelType}</Form.Label>
              <Form.Control
                type="text"
                value={
                  _languageService.isRTL
                    ? tank.fuelType?.arabicName
                    : tank.fuelType?.englishName
                }
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.station}</Form.Label>
              <Form.Control
                type="text"
                value={
                  tank.station === null
                    ? ' - '
                    : _languageService.isRTL
                    ? tank.station.arabicName
                    : tank.station.englishName
                }
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.capacity}</Form.Label>
              <Form.Control
                type="text"
                value={`${tank.capacity} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.maxLimit}</Form.Label>
              <Form.Control
                type="text"
                value={`${tank.maxLimit} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>
          </div>
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.minLimit}</Form.Label>
              <Form.Control
                type="text"
                value={`${tank.minLimit} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.currentLevel}</Form.Label>
              <Form.Control
                type="text"
                value={`${tank.currentLevel} ${_languageService.resources.centimeter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.currentVolume}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${tank.currentVolume} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.hasSensor}</Form.Label>
              <Form.Control
                type="text"
                value={
                  tank.hasSensor
                    ? _languageService.resources.yes
                    : _languageService.resources.no
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
