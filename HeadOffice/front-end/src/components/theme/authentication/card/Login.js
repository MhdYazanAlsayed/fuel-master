import React from 'react';
import LoginForm from 'components/theme/authentication/LoginForm';
import AuthCardLayout from 'layouts/AuthCardLayout';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Login = () => {
  const _languageService = useService(Services.LanguageService);

  return (
    <AuthCardLayout>
      <h3>{_languageService.resources.accountLogin}</h3>
      <LoginForm layout="card" hasLabel />
    </AuthCardLayout>
  );
};

export default Login;
