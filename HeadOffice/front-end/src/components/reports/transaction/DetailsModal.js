import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalCenter from 'components/shared/ModalCenter';
import React from 'react';
import { Button, Form, Modal, Badge } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const DetailsModal = ({ open, setOpen, transaction }) => {
  return transaction === null ? null : (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.details}
    >
      <Modal.Body>
        <div className="row">
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.station}</Form.Label>
              <Form.Control
                type="text"
                value={transaction.stationName ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.dateTime}</Form.Label>
              <Form.Control
                type="text"
                value={new Date(transaction.dateTime).toLocaleString()}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.pump}</Form.Label>
              <Form.Control
                type="text"
                value={transaction.pump?.number ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.nozzle}</Form.Label>
              <Form.Control
                type="text"
                value={
                  transaction.nozzle
                    ? `${transaction.nozzle.number} — ${
                        _languageService.resources.fuelTypes[
                          transaction.nozzle.tank?.fuelType
                        ]
                      }`
                    : ' - '
                }
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.paymentMethod}
              </Form.Label>
              <Form.Control
                type="text"
                value={
                  transaction.paymentMethod !== undefined
                    ? _languageService.resources.paymentMethods[
                        transaction.paymentMethod
                      ]?.label
                    : ' - '
                }
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.employee}</Form.Label>
              <Form.Control
                type="text"
                value={transaction.employee?.fullName ?? ' - '}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.pricePerLiter}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.price} ${_languageService.resources.rial}`}
                readOnly
              />
            </Form.Group>
          </div>
          <div className="col-md-6">
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.volume}</Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.volume} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>
            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.amount}</Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.amount} ${_languageService.resources.rial}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>{_languageService.resources.vat}</Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.vat} ${_languageService.resources.rial}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.totalizerBefore}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.totalizer} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.totalizerAfter}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.totalizerAfter} ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.tankVolumeBefore}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${
                  transaction.nozzle?.tank?.currentVolume + transaction.volume
                } ${_languageService.resources.liter}`}
                readOnly
              />
            </Form.Group>

            <Form.Group className="mb-2">
              <Form.Label>
                {_languageService.resources.tankVolumeAfter}
              </Form.Label>
              <Form.Control
                type="text"
                value={`${transaction.nozzle?.tank?.currentVolume} ${_languageService.resources.liter}`}
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
