import React, { useEffect, useState } from 'react';
import Lottie from 'lottie-react';
import loadingSquare from 'assets/img/animated-icons/loading-square.json';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const Loading = () => {
  const _loadingService = DependenciesInjector.services.loadingService;
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    _loadingService.setLoading = setLoading;
  }, []);

  return (
    <div className="loading" data-status={loading ? 'start' : 'stop'}>
      <div className="loading-overlay"></div>
      <div className="loading-content">
        <Lottie
          animationData={loadingSquare}
          loop={true}
          style={{ height: '130px', width: '130px' }}
        />
      </div>
    </div>
  );
};

export default Loading;
