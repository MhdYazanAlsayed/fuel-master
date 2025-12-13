import React from 'react';
import { Link } from 'react-router-dom';
import { Dropdown } from 'react-bootstrap';
import team3 from 'assets/img/team/avatar.png';
import Avatar from 'components/theme/common/Avatar';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const ProfileDropdown = () => {
  const _languageService = useService(Services.LanguageService);
  const _identityService = useService(Services.IdentityService);

  return (
    <Dropdown navbar={true} as="li">
      <Dropdown.Toggle
        bsPrefix="toggle"
        as={Link}
        to="#!"
        className="pe-0 ps-2 nav-link"
      >
        <div className="d-flex align-items-center gap-2">
          {_identityService.currentUser?.userName}
          <Avatar src={team3} />
        </div>
      </Dropdown.Toggle>

      <Dropdown.Menu className="dropdown-caret dropdown-menu-card  dropdown-menu-end">
        <div className="bg-white rounded-2 py-2 dark__bg-1000">
          {/* <Dropdown.Item className="fw-bold text-warning" href="#!">
            <FontAwesomeIcon icon="crown" className="me-1" />
            <span>Go Pro</span>
          </Dropdown.Item>
          <Dropdown.Divider />
          <Dropdown.Item href="#!">Set status</Dropdown.Item>
          <Dropdown.Item as={Link} to="/user/profile">
            Profile &amp; account
          </Dropdown.Item>
          <Dropdown.Item href="#!">Feedback</Dropdown.Item>
          <Dropdown.Divider /> */}
          {/*  */}
          <Dropdown.Item as={Link} to="/account/settings">
            {_languageService.resources.settings}
          </Dropdown.Item>
          <Dropdown.Item
            as={Link}
            to="/account/logout"
            className="text-danger"
            onClick={() => _identityService.logout()}
          >
            {_languageService.resources.logout}
          </Dropdown.Item>
        </div>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default ProfileDropdown;
