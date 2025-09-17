import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import DatePicker from 'react-datepicker';

const _languageService = DependenciesInjector.services.languageService;

const FuelMasterPicker = ({ value, onChange }) => {
  return (
    <DatePicker
      selected={value}
      onChange={onChange}
      formatWeekDay={day => day.slice(0, 3)}
      className="form-control"
      placeholderText={_languageService.resources.selectDateOrTime}
      timeIntervals={5}
      dateFormat="MM/dd/yyyy h:mm aa"
      showTimeSelect
      fixedHeight
    />
  );
};

export default FuelMasterPicker;
