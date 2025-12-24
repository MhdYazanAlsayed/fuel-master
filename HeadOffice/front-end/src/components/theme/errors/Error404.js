import React from 'react';
import { Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Error404 = () => {
  const _languageService = useService(Services.LanguageService);

  return (
    <Card className="text-center">
      <Card.Body className="p-5">
        <div className="display-1 text-300 fs-error">404</div>
        <p className="lead mt-4 text-800 font-sans-serif fw-semi-bold">
          {_languageService.resources.pageNotFound}
        </p>
        <hr />
        <p>
          {_languageService.resources.makeSureNotFound}
          <a href="mailto:info@exmaple.com" className="ms-1">
            {_languageService.resources.contactUs}
          </a>
          .
        </p>
        <Link className="btn btn-primary btn-sm mt-3" to="/">
          <FontAwesomeIcon icon={faHome} className="me-2" />
          {_languageService.resources.takeMeHome}
        </Link>
      </Card.Body>
    </Card>
  );
};

export default Error404;
