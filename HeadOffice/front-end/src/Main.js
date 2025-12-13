import React, { useState } from 'react';
import Theme from 'Theme';
import App from 'App';
import './components/styles/master.css';
import { injectServices } from './helpers/injectService';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

injectServices();

const Main = () => {
  const _languageService = useService(Services.LanguageService);

  const [reRenderState, setRender] = useState(true);

  // _identityService.loadIdentity();
  _languageService.configure(reRenderState, () => setRender(prev => !prev));
  _languageService.load();

  return (
    <Theme>
      <App />
      {/* <Loading /> */}
    </Theme>
  );
};

export default Main;
