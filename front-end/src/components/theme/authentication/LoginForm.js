import React, { useState } from 'react';
import { Button, Col, Form, Row } from 'react-bootstrap';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { useNavigate } from 'react-router-dom';

const LoginForm = () => {
  const _languageService = DependenciesInjector.services.languageService;
  const _userService = DependenciesInjector.services.userService;

  // State
  const [formData, setFormData] = useState({
    userName: '',
    password: '',
    rememberMe: false
  });

  const navigate = useNavigate();

  // Handler
  const handleSubmit = async e => {
    e.preventDefault();
    const response = await _userService.loginAsync(formData);

    if (!response.succeeded) return;

    navigate('/');
  };

  const handleFieldChange = e => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  return (
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3">
        <Form.Label>{_languageService.resources.userName}</Form.Label>
        <Form.Control
          placeholder={_languageService.resources.userName}
          value={formData.userName}
          name="userName"
          onChange={handleFieldChange}
          type="text"
        />
      </Form.Group>

      <Form.Group className="mb-3">
        <Form.Label>{_languageService.resources.password}</Form.Label>
        <Form.Control
          placeholder={_languageService.resources.password}
          value={formData.password}
          name="password"
          onChange={handleFieldChange}
          type="password"
        />
      </Form.Group>

      <Row className="justify-content-between align-items-center">
        <Col xs="auto">
          <Form.Check type="checkbox" id="rememberMeMe" className="mb-0">
            <Form.Check.Input
              type="checkbox"
              name="rememberMe"
              checked={formData.rememberMe}
              onChange={e =>
                setFormData({
                  ...formData,
                  rememberMe: e.target.checked
                })
              }
            />
            <Form.Check.Label className="mb-0 text-700">
              {_languageService.resources.rememberMe}
            </Form.Check.Label>
          </Form.Check>
        </Col>
      </Row>

      <Form.Group>
        <Button
          type="submit"
          color="primary"
          className="mt-3 w-100"
          disabled={!formData.userName || !formData.password}
        >
          {_languageService.resources.login}
        </Button>
      </Form.Group>
    </Form>
  );
};

// LoginForm.propTypes = {
//   layout: PropTypes.string,
//   hasLabel: PropTypes.bool
// };

LoginForm.defaultProps = {
  layout: 'simple',
  hasLabel: false
};

export default LoginForm;
