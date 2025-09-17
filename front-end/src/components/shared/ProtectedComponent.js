import React from 'react';
import PropTypes from 'prop-types';
import { Navigate } from 'react-router-dom';
import usePermissions from 'hooks/usePermissions';

/**
 * Higher-Order Component to protect components based on permissions
 * @param {React.Component} Component - The component to protect
 * @param {string|string[]|object} requiredPermissions - Required permissions
 * @param {string} redirectTo - Where to redirect if no permissions (default: '/errors/404')
 * @param {React.Component} fallbackComponent - Component to show instead of redirecting
 * @returns {React.Component} Protected component
 */
export const withPermissions = (
  Component,
  requiredPermissions,
  redirectTo = '/errors/404',
  fallbackComponent = null
) => {
  const ProtectedComponent = props => {
    const { canAccess } = usePermissions(requiredPermissions);

    if (!canAccess) {
      if (fallbackComponent) {
        return React.createElement(fallbackComponent, props);
      }
      return <Navigate to={redirectTo} replace />;
    }

    return <Component {...props} />;
  };

  ProtectedComponent.displayName = `withPermissions(${
    Component.displayName || Component.name
  })`;

  return ProtectedComponent;
};

/**
 * Component wrapper for conditional rendering based on permissions
 */
export const ProtectedComponent = ({
  children,
  permissions,
  fallback = null,
  redirectTo = null
}) => {
  const { canAccess } = usePermissions(permissions);

  if (!canAccess) {
    if (redirectTo) {
      return <Navigate to={redirectTo} replace />;
    }
    return fallback;
  }

  return children;
};

ProtectedComponent.propTypes = {
  children: PropTypes.node.isRequired,
  permissions: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.arrayOf(PropTypes.string),
    PropTypes.object
  ]),
  fallback: PropTypes.node,
  redirectTo: PropTypes.string
};

/**
 * Component for showing content only if user has specific permissions
 */
export const PermissionGate = ({
  children,
  permissions,
  fallback = null,
  requireAll = false
}) => {
  const { hasPermission, hasAllPermissions } = usePermissions();

  const canAccess = requireAll
    ? hasAllPermissions(permissions)
    : hasPermission(permissions);

  if (!canAccess) {
    return fallback;
  }

  return children;
};

PermissionGate.propTypes = {
  children: PropTypes.node.isRequired,
  permissions: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.arrayOf(PropTypes.string),
    PropTypes.object
  ]).isRequired,
  fallback: PropTypes.node,
  requireAll: PropTypes.bool
};

export default ProtectedComponent;
