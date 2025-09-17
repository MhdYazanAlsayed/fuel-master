import React from 'react';
import PropTypes from 'prop-types';
import { Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import logoutImg from 'assets/img/icons/spot-illustrations/45.png';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const LogoutContent = ({ titleTag: TitleTag }) => {
  const _languageService = DependenciesInjector.services.languageService;

  return (
    <>
      <img
        className="d-block mx-auto mb-4"
        src={logoutImg}
        alt="shield"
        width={100}
      />
      <TitleTag>{_languageService.resources.seeUAgain}</TitleTag>
      <p>
        {_languageService.resources.thantUForUsing}
        <br className="d-none d-sm-block" />
        {_languageService.resources.uAreNowSignedOut}
      </p>
      <Button
        as={Link}
        color="primary"
        size="sm"
        className="mt-3"
        to={`/account/login`}
      >
        <FontAwesomeIcon
          icon="chevron-left"
          transform="shrink-4 down-1"
          className="me-1"
        />
        {_languageService.resources.returnMeToLogin}
      </Button>
    </>
  );
};

LogoutContent.propTypes = {
  layout: PropTypes.string,
  titleTag: PropTypes.string
};

LogoutContent.defaultProps = {
  layout: 'simple',
  titleTag: 'h4'
};

export default LogoutContent;
