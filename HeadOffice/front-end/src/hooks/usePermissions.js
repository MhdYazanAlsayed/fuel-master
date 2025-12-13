import { useMemo } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const _roleManager = DependenciesInjector.services.roleManager;

/**
 * Custom hook for permission management
 * @param {string|string[]|object} permissions - Permission(s) to check
 * @returns {object} Permission checking utilities
 */
export const usePermissions = permissions => {
  const permissionUtils = useMemo(() => {
    // Check if user has specific permission(s)
    const hasPermission = perm => {
      if (!perm) return true;

      if (Array.isArray(perm)) {
        return perm.every(p => _roleManager.check(p));
      }

      if (typeof perm === 'string') {
        return _roleManager.check(perm);
      }

      if (typeof perm === 'object') {
        const permissionTypes = Object.keys(perm);
        return permissionTypes.some(type => {
          const permissionList = perm[type];
          if (Array.isArray(permissionList)) {
            return permissionList.every(p => _roleManager.check(p));
          }
          return _roleManager.check(permissionList);
        });
      }

      return false;
    };

    // Check if user has any of the provided permissions
    const hasAnyPermission = perms => {
      if (!perms) return true;

      if (Array.isArray(perms)) {
        return perms.some(p => _roleManager.check(p));
      }

      return _roleManager.check(perms);
    };

    // Check if user has all of the provided permissions
    const hasAllPermissions = perms => {
      if (!perms) return true;

      if (Array.isArray(perms)) {
        return perms.every(p => _roleManager.check(p));
      }

      return _roleManager.check(perms);
    };

    // Get all user permissions
    const getAllPermissions = () => {
      // This would need to be implemented based on your role manager
      // For now, returning an empty object
      return {};
    };

    return {
      hasPermission,
      hasAnyPermission,
      hasAllPermissions,
      getAllPermissions,
      // Check the permissions passed to the hook
      canAccess: hasPermission(permissions)
    };
  }, [permissions]);

  return permissionUtils;
};

/**
 * Hook for checking if a route is accessible
 * @param {object} route - Route object with permissions
 * @returns {boolean} Whether the route is accessible
 */
export const useRouteAccess = route => {
  const { hasPermission } = usePermissions();

  return useMemo(() => {
    if (!route) return false;

    // Check if the route itself is accessible
    const routeAccessible = hasPermission(route.permissions);

    // If route has children, check if any children are accessible
    if (route.children && Array.isArray(route.children)) {
      const hasAccessibleChildren = route.children.some(child => {
        const childAccessible = hasPermission(child.permissions);

        if (child.children) {
          return (
            childAccessible ||
            child.children.some(grandChild =>
              hasPermission(grandChild.permissions)
            )
          );
        }

        return childAccessible;
      });

      return routeAccessible || hasAccessibleChildren;
    }

    return routeAccessible;
  }, [route, hasPermission]);
};

export default usePermissions;
