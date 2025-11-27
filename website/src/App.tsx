import React, { useState } from 'react';
import { LandingPage } from './components/LandingPage';
import { Dashboard } from './components/Dashboard';

export default function App() {
  const [currentPage, setCurrentPage] = useState<'landing' | 'dashboard'>('landing');

  return (
    <div className="app">
      {currentPage === 'landing' ? (
        <LandingPage onNavigateToDashboard={() => setCurrentPage('dashboard')} />
      ) : (
        <Dashboard onNavigateToLanding={() => setCurrentPage('landing')} />
      )}
    </div>
  );
}
