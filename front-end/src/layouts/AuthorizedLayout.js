import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Outlet, Navigate } from 'react-router-dom';

const AuthorizedLayout = () => {
  const _authenticatorService =
    DependenciesInjector.services.authenticatorService;

  if (!_authenticatorService.isAuthenticated)
    return <Navigate to={'/account/login'} />;

  return <Outlet />;
};

export default AuthorizedLayout;
