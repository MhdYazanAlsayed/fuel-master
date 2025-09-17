import { Permissions } from 'app/core/enums/Permissions';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FuelMasterPicker from 'components/shared/FuelMasterPicker';
import Loader from 'components/shared/Loader';
import FuelMasterTable from 'components/shared/FuelMasterTable';
import { useEvents } from 'hooks/useEvents';
import React, { useEffect, useState } from 'react';
import { Button, Card, Form, Badge } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;
const _stationService = DependenciesInjector.services.stationService;
const _reportService = DependenciesInjector.services.reportService;
const _identityService = DependenciesInjector.services.identityService;

const TimeFilterBox = ({ handleGetReportAsync }) => {
  const [stations, setStations] = useState([]);
  const [formData, setFormData] = useState({
    from: '',
    to: '',
    stationId: -1
  });
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    handleOnLoadAsync();
  }, []);

  const handleOnLoadAsync = async () => {
    if (!_identityService.currentUser.stationId) {
      await handleGetStationsAsync();
    }

    setLoading(false);
  };

  const handleGetStationsAsync = async () => {
    const response = await _stationService.getAllAsync();
    setStations(
      response.map((item, index) => (
        <option value={item.id} key={index}>
          {_languageService.isRTL ? item.arabicName : item.englishName}
        </option>
      ))
    );
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    await handleGetReportAsync({
      ...formData,
      stationId: formData.stationId !== -1 ? formData.stationId : null
    });
  };

  return (
    <Card className="mb-2">
      <Card.Header>
        <h4>{_languageService.resources.filter}</h4>
      </Card.Header>
      <Card.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <div className="row">
            <div className="col-md-4">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.from}</Form.Label>
                <FuelMasterPicker
                  value={formData.from}
                  onChange={x => handleOnChange('from', x)}
                />
              </Form.Group>
            </div>
            <div className="col-md-4">
              <Form.Group className="mb-2">
                <Form.Label>{_languageService.resources.to}</Form.Label>
                <FuelMasterPicker
                  value={formData.to}
                  onChange={x => handleOnChange('to', x)}
                />
              </Form.Group>
            </div>
            <div className="col-md-4">
              {!_identityService.currentUser.stationId && (
                <Loader loading={loading}>
                  <Form.Group className="mb-2">
                    <Form.Label>
                      {_languageService.resources.station}
                    </Form.Label>
                    <Form.Select
                      value={formData.stationId}
                      onChange={x =>
                        handleOnChange(
                          'stationId',
                          parseInt(x.currentTarget.value)
                        )
                      }
                    >
                      <option value={-1}>
                        {_languageService.resources.selectOption}
                      </option>
                      {stations}
                    </Form.Select>
                  </Form.Group>
                </Loader>
              )}
            </div>
            <div className="col-md-12">
              <Button variant="primary" type="submit">
                {_languageService.resources.search}
              </Button>
            </div>
          </div>
        </form>
      </Card.Body>
    </Card>
  );
};

const TimeReportData = ({ reportData }) => {
  const employeesColumns = [
    {
      header: _languageService.resources.employee,
      Cell: data => <>{data.fullName}</>
    },
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data.stationArabicName
            : data.stationEnglishName}
        </>
      )
    },
    {
      header: _languageService.resources.volume,
      Cell: data => (
        <>
          {data.volume} {_languageService.resources.liter}
        </>
      )
    },
    {
      header: _languageService.resources.amount,
      Cell: data => (
        <>
          {data.amount} {_languageService.resources.rial}
        </>
      )
    }
  ];

  const paymentTransactionsColumns = [
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>{_languageService.isRTL ? data.arabicName : data.englishName}</>
      )
    },
    {
      header: _languageService.resources.paymentMethod,
      Cell: data => {
        return <Badge
          bg={_languageService.resources.paymentMethods[data.paymentMethod].bg}
        >
          {_languageService.resources.paymentMethods[data.paymentMethod].label}
        </Badge>
      }
    },
    {
      header: _languageService.resources.transactionCount,
      Cell: data => <>{data.transactionCount}</>
    },
    {
      header: _languageService.resources.volume,
      Cell: data => (
        <>
          {data.volume} {_languageService.resources.liter}
        </>
      )
    },
    {
      header: _languageService.resources.amount,
      Cell: data => (
        <>
          {data.amount} {_languageService.resources.rial}
        </>
      )
    }
  ];

  const stationsColumns = [
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>{_languageService.isRTL ? data.arabicName : data.englishName}</>
      )
    },
    {
      header: _languageService.resources.volume,
      Cell: data => (
        <>
          {data.volume} {_languageService.resources.liter}
        </>
      )
    },
    {
      header: _languageService.resources.amount,
      Cell: data => (
        <>
          {data.amount} {_languageService.resources.rial}
        </>
      )
    }
  ];

  const nozzlesColumns = [
    {
      header: _languageService.resources.station,
      Cell: data => (
        <>
          {_languageService.isRTL
            ? data.stationArabicName
            : data.stationEnglishName}
        </>
      )
    },
    {
      header: _languageService.resources.nozzle,
      Cell: data => <>{data.number}</>
    },
    {
      header: _languageService.resources.fuelType,
      Cell: data => <>{data.fuelType}</>
    },
    {
      header: _languageService.resources.volume,
      Cell: data => (
        <>
          {data.volume} {_languageService.resources.liter}
        </>
      )
    },
    {
      header: _languageService.resources.amount,
      Cell: data => (
        <>
          {data.amount} {_languageService.resources.rial}
        </>
      )
    }
  ];

  return (
    <div>
      {reportData.stationsReport && reportData.stationsReport.length > 0 && (
        <div className="mb-4">
          <FuelMasterTable
            title={_languageService.resources.stationsReport}
            columns={stationsColumns}
            data={reportData.stationsReport}
            showPagination={false}
          />
        </div>
      )}

      {reportData.nozzlesReport && reportData.nozzlesReport.length > 0 && (
        <div className="mb-4">
          <FuelMasterTable
            title={_languageService.resources.nozzlesReport}
            columns={nozzlesColumns}
            data={reportData.nozzlesReport}
            showPagination={false}
          />
        </div>
      )}

      {reportData.employeesReport && reportData.employeesReport.length > 0 && (
        <div className="mb-4">
          <FuelMasterTable
            title={_languageService.resources.employeesReport}
            columns={employeesColumns}
            data={reportData.employeesReport}
            showPagination={false}
          />
        </div>
      )}

      {reportData.paymentTransactionsReport &&
        reportData.paymentTransactionsReport.length > 0 && (
          <div className="mb-4">
            <FuelMasterTable
              title={_languageService.resources.paymentTransactionsReport}
              columns={paymentTransactionsColumns}
              data={reportData.paymentTransactionsReport}
              showPagination={false}
            />
          </div>
        )}
    </div>
  );
};

const Index = () => {
  const [reportData, setReportData] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleGetReportAsync = async filters => {
    setLoading(true);
    try {
      const response = await _reportService.getTimeReportAsync(
        filters.from,
        filters.to,
        filters.stationId
      );

      if (response) {
        setReportData(response);
      }
    } catch (error) {
      console.error('Error fetching time report:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <TimeFilterBox handleGetReportAsync={handleGetReportAsync} />

      <Loader loading={loading}>
        {reportData && <TimeReportData reportData={reportData} />}
      </Loader>
    </div>
  );
};

export default Index;
