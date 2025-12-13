import React from 'react';
import { Outlet, Navigate } from 'react-router-dom';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const AuthorizedLayout = () => {
  const _authenticatorService = useService(Services.AuthenticatorService);

  if (!_authenticatorService.isAuthenticated)
    return <Navigate to={'/account/login'} />;

  return <Outlet />;
};

export default AuthorizedLayout;
