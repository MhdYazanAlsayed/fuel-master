import React, { useContext } from 'react';
import { Button } from 'react-bootstrap';
import AppContext from 'context/Context';

// const renderTooltip = props => (
//   <Tooltip id="button-tooltip" {...props}>
//     {_languageService.resources.toggleNavigation}
//   </Tooltip>
// );

const ToggleButton = () => {
  const {
    config: { isNavbarVerticalCollapsed },
    setConfig
  } = useContext(AppContext);

  const handleClick = () => {
    document
      .getElementsByTagName('html')[0]
      .classList.toggle('navbar-vertical-collapsed');
    setConfig('isNavbarVerticalCollapsed', !isNavbarVerticalCollapsed);
  };

  return (
    // <OverlayTrigger
    //   placement={
    //     isFluid ? (isRTL ? 'bottom' : 'right') : isRTL ? 'bottom' : 'left'
    //   }
    // >

    // </OverlayTrigger>
    <div className="toggle-icon-wrapper">
      <Button
        variant="link"
        className="navbar-toggler-humburger-icon navbar-vertical-toggle"
        id="toggleNavigationTooltip"
        onClick={handleClick}
      >
        <span className="navbar-toggle-icon">
          <span className="toggle-line" />
        </span>
      </Button>
    </div>
  );
};

export default ToggleButton;
