import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React, { useEffect, useState } from 'react';
import { Card } from 'react-bootstrap';
import Nozzle from './Nozzle';
import '../../styles/reports/realtime.css';
import Loader from 'components/shared/Loader';
import StationSelect from './StationSelect';
import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';

const _languageService = DependenciesInjector.services.languageService;
const _reportService = DependenciesInjector.services.reportService;
const _httpService = DependenciesInjector.services.httpService;
const _identityService = DependenciesInjector.services.identityService;

const Index = () => {
  // States
  const [nozzles, setNozzles] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    handleOnLoadComponentAsync();
    handleRealTimeConnectionAsync();
  }, []);

  const handleRealTimeConnectionAsync = async () => {
    const connection = new HubConnectionBuilder()
      .withUrl(_httpService._api + 'realtime?tenantId=foryou', {
        accessTokenFactory: () => _identityService.accessToken,
        withCredentials: true
      })
      .build();

    await connection.start();

    connection.on('realtime-report', data => {
      setNozzles(prev => {
        for (let currentNozzle of data.nozzles) {
          const index = prev.findIndex(x => x.id === currentNozzle.id);
          if (index === -1) throw new Error();
          console.log(currentNozzle);
          prev[index].status = currentNozzle.status;
          prev[index].amount = currentNozzle.amount;
          prev[index].volume = currentNozzle.volume;
        }

        return [...prev];
      });
    });
  };

  const handleOnLoadComponentAsync = async () => {
    setLoading(true);

    await handleGetReportAsync();

    setLoading(false);
  };

  const handleGetReportAsync = async stationId => {
    const response = await _reportService.getRealTimeAsync(stationId);
    if (!response) return;

    setNozzles(response.nozzles);
  };

  const handleChangeStationAsync = stationId => {
    handleGetReportAsync(stationId);
  };

  return (
    <div className="realtime-report">
      <Card>
        <Card.Header>
          <div className="d-flex align-items-center justify-content-between">
            <h4>{_languageService.resources.nozzles}</h4>
            <StationSelect handleChangeStation={handleChangeStationAsync} />
          </div>
        </Card.Header>
        <Card.Body>
          <Loader loading={loading}>
            <div className="row">
              {nozzles.length === 0 ? (
                <div className="p-2">
                  <div className="alert alert-warning">
                    {_languageService.resources.thereIsNoDate}
                  </div>
                </div>
              ) : (
                nozzles.map((item, index) => (
                  <div className="col-md-3" key={index}>
                    <Nozzle
                      number={item.number}
                      status={item.status}
                      pumpNumber={item.pump.number}
                      fuelType={item.fuelType}
                      amount={item.amount}
                      volume={item.volume}
                      price={item.price}
                    />
                  </div>
                ))
              )}
            </div>
          </Loader>
        </Card.Body>
      </Card>
    </div>
  );
};

export default Index;
