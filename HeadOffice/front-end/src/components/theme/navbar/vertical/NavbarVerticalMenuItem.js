import React from 'react';
import Flex from 'components/theme/common/Flex';
import SoftBadge from 'components/theme/common/SoftBadge';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const NavbarVerticalMenuItem = ({ route }) => {
  const _languageService = useService(Services.LanguageService);

  return (
    <Flex alignItems="center">
      {route.icon && (
        <span className="nav-link-icon">
          <i className={`fa-solid fa-${route.icon}`}></i>
          {/* <FontAwesomeIcon icon={route.icon} /> */}
        </span>
      )}
      <span className="nav-link-text ps-1">
        {route.keyword ? _languageService.resources[route.keyword] : route.name}
      </span>
      {route.badge && (
        <SoftBadge pill bg={route.badge.type} className="ms-2">
          {route.badge.keyword
            ? _languageService.resources[route.badge.keyword]
            : route.badge.text}
        </SoftBadge>
      )}
    </Flex>
  );
};

// prop-types
// const routeShape = {
//   active: PropTypes.bool,
//   name: PropTypes.string.isRequired,
//   to: PropTypes.string,
//   icon: PropTypes.oneOfType([PropTypes.array, PropTypes.string])
// };
// routeShape.children = PropTypes.arrayOf(PropTypes.shape(routeShape));
// NavbarVerticalMenuItem.propTypes = {
//   route: PropTypes.shape(routeShape).isRequired
// };

export default NavbarVerticalMenuItem;
