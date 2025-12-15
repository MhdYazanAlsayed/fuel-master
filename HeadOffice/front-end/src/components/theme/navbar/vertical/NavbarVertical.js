import React, { useContext, useEffect, Fragment, useState } from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import { Nav, Navbar, Row, Col } from 'react-bootstrap';
import { navbarBreakPoint, topNavbarBreakpoint } from 'config';
import AppContext from 'context/Context';
import Flex from 'components/theme/common/Flex';
import Logo from 'components/theme/common/Logo';
import NavbarVerticalMenu from './NavbarVerticalMenu';
import ToggleButton from './ToggleButton';
import routes from 'routes/routes';
import { capitalize } from 'helpers/utils';
import NavbarTopDropDownMenus from 'components/theme/navbar/top/NavbarTopDropDownMenus';
import bgNavbar from 'assets/img/generic/bg-navbar.png';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

const NavbarVertical = () => {
  const {
    config: {
      navbarPosition,
      navbarStyle,
      isNavbarVerticalCollapsed,
      showBurgerMenu
    }
  } = useContext(AppContext);

  const HTMLClassList = document.getElementsByTagName('html')[0].classList;
  const [sidebarLinks, setSidebarLinks] = useState(null);
  const _languageService = useService(Services.LanguageService);
  const _permissionService = useService(Services.PermissionService);

  useEffect(() => {
    if (isNavbarVerticalCollapsed) {
      HTMLClassList.add('navbar-vertical-collapsed');
    } else {
      HTMLClassList.remove('navbar-vertical-collapsed');
    }
    return () => {
      HTMLClassList.remove('navbar-vertical-collapsed-hover');
    };
  }, [isNavbarVerticalCollapsed, HTMLClassList]);

  //Control mouseEnter event
  let time = null;
  const handleMouseEnter = () => {
    if (isNavbarVerticalCollapsed) {
      time = setTimeout(() => {
        HTMLClassList.add('navbar-vertical-collapsed-hover');
      }, 100);
    }
  };
  const handleMouseLeave = () => {
    clearTimeout(time);
    HTMLClassList.remove('navbar-vertical-collapsed-hover');
  };

  const NavbarLabel = ({ label }) => (
    <Nav.Item as="li">
      <Row className="mt-3 mb-2 navbar-vertical-label-wrapper">
        <Col xs="auto" className="navbar-vertical-label navbar-vertical-label">
          {label}
        </Col>
        <Col className="ps-0">
          <hr className="mb-0 navbar-vertical-divider"></hr>
        </Col>
      </Row>
    </Nav.Item>
  );

  useEffect(() => {
    setSidebarLinks(routes.map(formatRoute));
  }, [_languageService.renderState]);

  function formatRoute(route) {
    if (!_permissionService.check(route.areaOfAccess)) return null;

    return (
      <Fragment key={route.label}>
        {!route.labelDisable &&
          (route.children.length == 0 || (
            <NavbarLabel
              label={capitalize(
                route.keyword
                  ? _languageService.resources[route.keyword]
                  : route.label
              )}
            />
          ))}
        <NavbarVerticalMenu routes={route.children} />
      </Fragment>
    );
  }

  return (
    <Navbar
      expand={navbarBreakPoint}
      className={classNames('navbar-vertical', {
        [`navbar-${navbarStyle}`]: navbarStyle !== 'transparent'
      })}
      variant="light"
    >
      <Flex alignItems="center">
        <ToggleButton />
        <Logo at="navbar-vertical" width={40} />
      </Flex>
      <Navbar.Collapse
        in={showBurgerMenu}
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
        style={{
          backgroundImage:
            navbarStyle === 'vibrant'
              ? `linear-gradient(-45deg, rgba(0, 160, 255, 0.86), #0048a2),url(${bgNavbar})`
              : 'none'
        }}
      >
        <div className="navbar-vertical-content scrollbar">
          <Nav className="flex-column" as="ul">
            {sidebarLinks}
            {/* {routes.map(route => (
              <Fragment key={route.label}>
                {!route.labelDisable && (
                  <NavbarLabel label={capitalize(route.label)} />
                )}
                <NavbarVerticalMenu routes={route.children} />
              </Fragment>
            ))} */}
          </Nav>

          <>
            {navbarPosition === 'combo' && (
              <div className={`d-${topNavbarBreakpoint}-none`}>
                <div className="navbar-vertical-divider">
                  <hr className="navbar-vertical-hr my-2" />
                </div>
                <Nav navbar>
                  <NavbarTopDropDownMenus />
                </Nav>
              </div>
            )}
            {/* <PurchaseCard /> */}
          </>
        </div>
      </Navbar.Collapse>
    </Navbar>
  );
};

NavbarVertical.propTypes = {
  label: PropTypes.string
};

export default NavbarVertical;
