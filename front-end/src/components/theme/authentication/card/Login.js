import React from 'react';
import LoginForm from 'components/theme/authentication/LoginForm';
import AuthCardLayout from 'layouts/AuthCardLayout';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const Login = () => {
  const _languageService = DependenciesInjector.services.languageService;

  return (
    <AuthCardLayout>
      <h3>{_languageService.resources.accountLogin}</h3>
      <LoginForm layout="card" hasLabel />
    </AuthCardLayout>
  );
};

export default Login;
