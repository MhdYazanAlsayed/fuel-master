import React, { useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Theme from 'Theme';
import App from 'App';
import './components/styles/master.css';
import Loading from 'components/shared/Loading';

const Main = () => {
  const _identityService = DependenciesInjector.services.identityService;
  const _languageService = DependenciesInjector.services.languageService;

  const [render, setRender] = useState(true);

  _identityService.loadIdentity();
  _languageService.configure(render, () => setRender(prev => !prev));
  _languageService.load();

  return (
    <Theme>
      <App />
      <Loading />
    </Theme>
  );
};

export default Main;
