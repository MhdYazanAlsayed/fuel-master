import classNames from 'classnames';
import AppContext from 'context/Context';
import PropTypes from 'prop-types';
import React, { useContext, useState } from 'react';
import { Collapse, Nav } from 'react-bootstrap';
import { NavLink, useLocation } from 'react-router-dom';
import NavbarVerticalMenuItem from './NavbarVerticalMenuItem';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const _roleManager = DependenciesInjector.services.roleManager;

// Enhanced permission checking function
const checkPermissions = permissions => {
  if (!permissions) {
    return true; // No permissions required, allow access
  }

  // Handle array of permissions (ALL must be true)
  if (Array.isArray(permissions)) {
    return permissions.every(permission => {
      const hasPermission = _roleManager.check(permission);
      if (!hasPermission) {
        console.debug(`Permission denied: ${permission}`);
      }
      return hasPermission;
    });
  }

  // Handle single permission string
  if (typeof permissions === 'string') {
    const hasPermission = _roleManager.check(permissions);
    if (!hasPermission) {
      console.debug(`Permission denied: ${permissions}`);
    }
    return hasPermission;
  }

  // Handle object with multiple permission types
  if (typeof permissions === 'object') {
    // Check if any of the permission types are satisfied
    const permissionTypes = Object.keys(permissions);
    return permissionTypes.some(type => {
      const permissionList = permissions[type];
      if (Array.isArray(permissionList)) {
        return permissionList.every(permission =>
          _roleManager.check(permission)
        );
      }
      return _roleManager.check(permissionList);
    });
  }

  return false; // Default to deny if permission format is unknown
};

// Check if any child route is accessible
const hasAccessibleChildren = children => {
  if (!children || !Array.isArray(children)) {
    return false;
  }

  return children.some(child => {
    // Check if this child is accessible
    const childAccessible = checkPermissions(child.permissions);

    // If this child has children, recursively check them
    if (child.children) {
      return childAccessible || hasAccessibleChildren(child.children);
    }

    return childAccessible;
  });
};

const CollapseItems = ({ route }) => {
  const { pathname } = useLocation();

  const openCollapse = childrens => {
    const checkLink = children => {
      if (children.to === pathname) {
        return true;
      }
      return (
        Object.prototype.hasOwnProperty.call(children, 'children') &&
        children.children.some(checkLink)
      );
    };
    return childrens.some(checkLink);
  };

  const [open, setOpen] = useState(openCollapse(route.children));

  return (
    <Nav.Item as="li">
      <Nav.Link
        onClick={() => {
          setOpen(!open);
        }}
        className={classNames('dropdown-indicator cursor-pointer', {
          'text-500': !route.active
        })}
        aria-expanded={open}
      >
        <NavbarVerticalMenuItem route={route} />
      </Nav.Link>
      <Collapse in={open}>
        <Nav className="flex-column nav" as="ul">
          <NavbarVerticalMenu routes={route.children} />
        </Nav>
      </Collapse>
    </Nav.Item>
  );
};

CollapseItems.propTypes = {
  route: PropTypes.shape({
    name: PropTypes.string.isRequired,
    icon: PropTypes.string,
    children: PropTypes.array.isRequired,
    active: PropTypes.bool
  }).isRequired
};

const NavbarVerticalMenu = ({ routes }) => {
  const {
    config: { showBurgerMenu },
    setConfig
  } = useContext(AppContext);

  const { pathname } = useLocation();

  const handleNavItemClick = () => {
    if (showBurgerMenu) {
      setConfig('showBurgerMenu', !showBurgerMenu);
    }
  };

  return routes.map(route => {
    // Check if the route itself is accessible
    const routeAccessible = checkPermissions(route.permissions);

    // If route has children, check if any children are accessible
    const childrenAccessible = route.children
      ? hasAccessibleChildren(route.children)
      : false;

    // Route is accessible if either the route itself or any of its children are accessible
    const isAccessible = routeAccessible || childrenAccessible;

    if (!isAccessible) {
      return null; // Don't render this route
    }

    // If route has children, render as collapsible item
    if (route.children) {
      // Filter children to only show accessible ones
      const accessibleChildren = route.children.filter(child => {
        const childAccessible = checkPermissions(child.permissions);

        if (child.children) {
          return childAccessible || hasAccessibleChildren(child.children);
        }

        return childAccessible;
      });

      // If no children are accessible, don't render the parent
      if (accessibleChildren.length === 0) {
        return null;
      }

      // Create a new route object with filtered children
      const filteredRoute = {
        ...route,
        children: accessibleChildren
      };

      return <CollapseItems route={filteredRoute} key={route.name} />;
    }

    // Render as single nav item
    return (
      <Nav.Item as="li" key={route.name} onClick={handleNavItemClick}>
        <NavLink
          end={route.exact}
          to={route.to}
          state={{ open: route.to === '/authentication-modal' }}
          className={() => {
            const indexVariableMark = pathname.indexOf('?');
            let basePath =
              indexVariableMark > -1
                ? pathname.slice(0, indexVariableMark)
                : pathname;

            return basePath == route.to ? 'active nav-link' : 'nav-link';
          }}
        >
          <NavbarVerticalMenuItem route={route} />
        </NavLink>
      </Nav.Item>
    );
  });
};

NavbarVerticalMenu.propTypes = {
  routes: PropTypes.arrayOf(PropTypes.shape(NavbarVerticalMenuItem.propTypes))
    .isRequired
};

export default NavbarVerticalMenu;
