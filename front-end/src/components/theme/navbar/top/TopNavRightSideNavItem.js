import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Languages } from 'app/core/enums/Languages';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
// import NotificationDropdown from 'components/theme/navbar/top/NotificationDropdown';
import ProfileDropdown from 'components/theme/navbar/top/ProfileDropdown';
import AppContext from 'context/Context';
import React, { useContext } from 'react';
import { Nav, OverlayTrigger, Tooltip } from 'react-bootstrap';

const TopNavRightSideNavItem = () => {
  const _languageService = DependenciesInjector.services.languageService;

  const {
    config: { isDark, isRTL },
    setConfig
  } = useContext(AppContext);

  const handleChangeLanguage = () => {
    setConfig('isRTL', _languageService.isRTL ? false : true);

    _languageService.changeLanguage(
      _languageService.isRTL ? Languages.English : Languages.Arabic
    );
  };
  return (
    <Nav
      navbar
      className="navbar-nav-icons ms-auto flex-row align-items-center"
      as="ul"
    >
      <Nav.Item as={'li'}>
        <Nav.Link
          className="px-2 theme-control-toggle"
          onClick={() => setConfig('isDark', !isDark)}
        >
          <OverlayTrigger
            key="left"
            placement={isRTL ? 'bottom' : 'left'}
            overlay={
              <Tooltip id="ThemeColor">
                {isDark ? 'Switch to light theme' : 'Switch to dark theme'}
              </Tooltip>
            }
          >
            <div className="theme-control-toggle-label">
              <FontAwesomeIcon
                icon={isDark ? 'sun' : 'moon'}
                className="fs-0"
              />
            </div>
          </OverlayTrigger>
        </Nav.Link>
      </Nav.Item>

      <Nav.Item as={'li'}>
        <Nav.Link
          className="px-2 theme-control-toggle"
          onClick={handleChangeLanguage}
        >
          <OverlayTrigger
            key="left"
            placement={isRTL ? 'bottom' : 'left'}
            overlay={
              <Tooltip id="ThemeColor">
                {_languageService.resources.switchLanguage}
              </Tooltip>
            }
          >
            <div className="theme-control-toggle-label">
              {_languageService.isRTL ? (
                <p className="mb-0">En</p>
              ) : (
                <p className="mb-0">Ar</p>
              )}
            </div>
          </OverlayTrigger>
        </Nav.Link>
      </Nav.Item>

      {/* <CartNotification /> */}
      {/* <NineDotMenu /> */}
      {/* <NotificationDropdown /> */}
      <ProfileDropdown />
    </Nav>
  );
};

export default TopNavRightSideNavItem;
