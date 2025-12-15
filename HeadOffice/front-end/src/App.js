import React, { useState, useEffect } from 'react';
import { Spinner } from 'react-bootstrap';
import { BrowserRouter as Router } from 'react-router-dom';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-toastify/dist/ReactToastify.min.css';
import Layout from './layouts/Layout';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const App = () => {
  const [loading, setLoading] = useState(true);
  const userService = useService(Services.UserService);
  const languageService = useService(Services.LanguageService);

  const checkAuthHealth = async () => {
    await userService.checkAuthHealthAsync();
    setLoading(false);
  };

  useEffect(() => {
    checkAuthHealth();
  }, []);

  return loading ? (
    <div
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh'
      }}
    >
      <Spinner animation="border" variant="warning" />
      <span className="ms-2">
        {languageService.resources.checkingAuthenticationHealth}
      </span>
    </div>
  ) : (
    <Router basename={process.env.PUBLIC_URL}>
      <Layout />
    </Router>
  );
};

export default App;
