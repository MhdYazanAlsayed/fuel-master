import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Card } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const Header = () => {
  return (
    <Card className="mb-2">
      <Card.Header>
        <h4>{_languageService.resources.permissions}</h4>
        <p className="text-muted mb-0">
          {_languageService.resources.permissionsDescription}
        </p>
      </Card.Header>
    </Card>
  );
};

export default Header;
