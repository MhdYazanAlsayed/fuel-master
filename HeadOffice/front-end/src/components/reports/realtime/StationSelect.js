import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Loader from 'components/shared/Loader';
import React, { useEffect, useState } from 'react';
import { Form } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _identityService = DependenciesInjector.services.identityService;

const StationSelect = ({ handleChangeStation }) => {
  if (_identityService.currentUser.stationId) {
    return <></>;
  }

  // States
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [stationId, setStationId] = useState(-1);

  useEffect(() => {
    handleGetStationsAsync();
  }, []);

  const handleGetStationsAsync = async () => {
    const response = await _stationService.getAllAsync();

    setStations(
      response.map((x, index) => (
        <option key={index} value={x.id}>
          {_languageService.isRTL ? x.arabicName : x.englishName}
        </option>
      ))
    );

    setLoading(false);
  };

  const handleOnStationChange = id => {
    setStationId(id);
    handleChangeStation(id === -1 ? undefined : id);
  };

  return (
    <Loader loading={loading}>
      <Form.Select
        value={stationId}
        onChange={x => handleOnStationChange(parseInt(x.currentTarget.value))}
      >
        <option value={-1}>{_languageService.resources.all}</option>
        {stations}
      </Form.Select>
    </Loader>
  );
};

export default StationSelect;
