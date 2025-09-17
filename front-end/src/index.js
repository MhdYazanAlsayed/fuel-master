import React from 'react';
import { createRoot } from 'react-dom/client';
import Main from './Main';
import 'helpers/initFA';

const container = document.getElementById('main');
const root = createRoot(container);

root.render(<Main />);
