import React, { Fragment } from "react";
import IIdentityService from "../../app/core/interfaces/identity/IIdentityService";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import { Services } from "../../app/core/utils/ServiceCollection";
import { Navigate, Outlet } from "react-router-dom";

const RequiredUnAuthorizedLayout = () => {
  const identityService =
    DependeciesInjector.services.getService<IIdentityService>(
      Services.IdentityService
    );

  if (identityService.isAuthenticated()) {
    return <Navigate to="/dashboard" replace />;
  }

  return (
    <Fragment>
      <Outlet />
    </Fragment>
  );
};

export default RequiredUnAuthorizedLayout;
