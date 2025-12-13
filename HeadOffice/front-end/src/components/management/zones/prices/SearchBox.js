import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Button, Card, Form } from 'react-bootstrap';

const SearchBox = () => {
  const _languageService = DependenciesInjector.services.languageService;

  return (
    <Card className="mb-2">
      <Card.Body>
        <h5 className="mb-3">{_languageService.resources.reportFormData}</h5>

        <div className="row">
          <div className="col-md-6">
            <Form.Group>
              <Form.Label>{_languageService.resources.station}</Form.Label>
              <Form.Select>
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
              </Form.Select>
            </Form.Group>
          </div>
          <div className="col-md-6">
            <Form.Group>
              <Form.Label>{_languageService.resources.fuelType}</Form.Label>
              <Form.Select>
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                <option value={0}>{_languageService.resources.gaso91}</option>
                <option value={1}>{_languageService.resources.gaso95}</option>
                <option value={2}>{_languageService.resources.diesel}</option>
              </Form.Select>
            </Form.Group>
          </div>

          <div className="col-md-12 mt-2">
            <Button variant="primary">
              {_languageService.resources.search}
            </Button>
          </div>
        </div>
      </Card.Body>
    </Card>
  );
};

export default SearchBox;
