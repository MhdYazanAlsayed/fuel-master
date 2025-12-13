import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Form } from 'react-bootstrap';

const Nozzle = ({
  number,
  status,
  pumpNumber,
  fuelType,
  volume,
  amount,
  price
}) => {
  const _languageService = DependenciesInjector.services.languageService;

  return (
    <div className="nozzle-status bg-light rounded shadow border overflow-hidden">
      <div
        className={`alert alert-${
          _languageService.resources.nozzleStatuses[status + '_bg']
        } px-2 m-0 rounded-0 fw-bold nozzle-header`}
      >
        <div className="d-flex align-items-center gap-2">
          <span className="nozzle-number p-1 border rounded-circle shadow">
            {number}
          </span>
          <span>-</span>
          {_languageService.resources.nozzleStatuses[status]}
        </div>
      </div>

      <div className="nozzle-body py-1">
        <div className="px-2 py-1">
          <div className="d-flex align-items-center gap-2">
            <p className="mb-0">{_languageService.resources.pump}</p>
            <Form.Control value={pumpNumber} size="sm" readOnly />
          </div>
        </div>
        <div className="px-2 py-1">
          <div className="d-flex align-items-center gap-2">
            <p className="mb-0">{_languageService.resources.fuelType}</p>
            <Form.Control
              value={_languageService.resources.fuelTypes[fuelType]}
              size={'sm'}
              readOnly
            />
          </div>
        </div>
        <div className="px-2 py-1">
          <div className="d-flex align-items-center gap-2">
            <p className="mb-0">{_languageService.resources.amount}</p>
            <Form.Control value={amount} size={'sm'} readOnly />
          </div>
        </div>
        <div className="px-2 py-1">
          <div className="d-flex align-items-center gap-2">
            <p className="mb-0">{_languageService.resources.volume}</p>
            <Form.Control value={volume} size={'sm'} readOnly />
          </div>
        </div>
        <div className="px-2 py-1">
          <div className="d-flex align-items-center gap-2">
            <p className="mb-0">{_languageService.resources.price}</p>
            <Form.Control value={price} size={'sm'} readOnly />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Nozzle;
