import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-toastify/dist/ReactToastify.min.css';
import Layout from './layouts/Layout';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const App = () => {
  const userService = useService(Services.UserService);

  const checkAuthHealth = async () => {};

  return (
    <Router basename={process.env.PUBLIC_URL}>
      <Layout />
    </Router>
  );
};

export default App;
