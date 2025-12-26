import FormCard from 'components/shared/FormCard';
import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { useNavigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import { Scope } from 'app/core/abstracts/Scope';

const Create = () => {
  const _languageService = useService(Services.LanguageService);
  const _roleService = useService(Services.RoleService);
  const _employeeService = useService(Services.EmployeeService);
  const _stationService = useService(Services.StationService);
  const _cityService = useService(Services.CityService);
  const _areaService = useService(Services.AreaService);

  // States
  const [formData, setFormData] = useState({
    fullName: '',
    cardNumber: '',
    userName: '',
    password: '',
    confirmPassword: '',
    roleId: -1,
    phoneNumber: '',
    emailAddress: '',
    age: '',
    identificationNumber: '',
    address: '',
    stationId: -1,
    cityId: -1,
    areaId: -1,
    scope: Scope.ALL
  });
  const [roles, setRoles] = useState([]);
  const [stations, setStations] = useState([]);
  const [cities, setCities] = useState([]);
  const [areas, setAreas] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  const handleScopeChange = value => {
    const scopeValue = parseInt(value);
    handleOnChange('scope', scopeValue);
    // Reset dependent fields when scope changes
    handleOnChange('cityId', -1);
    handleOnChange('areaId', -1);
    handleOnChange('stationId', -1);
  };

  useEffect(() => {
    loadDropdownData();
  }, []);

  const loadDropdownData = async () => {
    const roleResponse = await _roleService.getAllAsync();
    const stationResponse = await _stationService.getAllAsync();
    const cityResponse = await _cityService.getAllAsync();
    const areaResponse = await _areaService.getAllAsync();
    setRoles(roleResponse);
    setStations(stationResponse);
    setCities(cityResponse);
    setAreas(areaResponse);
    setLoading(false);
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _employeeService.createAsync({
      fullName: formData.fullName.trim(),
      cardNumber: formData.cardNumber.trim(),
      userName: formData.userName.trim(),
      password: formData.password.trim(),
      confirmPassword: formData.confirmPassword.trim(),
      roleId: formData.roleId,
      phoneNumber:
        formData.phoneNumber.trim() === '' ? null : formData.phoneNumber,
      emailAddress:
        formData.emailAddress.trim() === '' ? null : formData.emailAddress,
      age: formData.age.trim() === '' ? null : parseInt(formData.age),

      identificationNumber:
        formData.identificationNumber.trim() === ''
          ? null
          : formData.identificationNumber,
      address: formData.address.trim() === '' ? null : formData.address,
      scope: formData.scope,
      cityId: formData.cityId === -1 ? null : formData.cityId,
      areaId: formData.areaId === -1 ? null : formData.areaId,
      stationId: formData.stationId === -1 ? null : formData.stationId
    });
    if (!response.succeeded) return;

    navigate('/employees');
  };

  return (
    <FormCard
      header={_languageService.resources.createEmployee}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.fullName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.fullName}
              onChange={x => handleOnChange('fullName', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.cardNumber}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.cardNumber}
              onChange={x =>
                handleOnChange('cardNumber', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.userName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.userName}
              onChange={x => handleOnChange('userName', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.password}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="password"
              placeholder={_languageService.resources.password}
              onChange={x => handleOnChange('password', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.confirmPassword}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="password"
              placeholder={_languageService.resources.confirmPassword}
              onChange={x =>
                handleOnChange('confirmPassword', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.role}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnChange('roleId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {roles.map((role, index) => (
                <option key={index} value={role.id}>
                  {_languageService.isRTL ? role.arabicName : role.englishName}
                </option>
              ))}
            </Form.Select>
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.phoneNumber}</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.phoneNumber}
              onChange={x =>
                handleOnChange('phoneNumber', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.emailAddress}</span>
            </Form.Label>
            <Form.Control
              type="email"
              placeholder={_languageService.resources.emailAddress}
              onChange={x =>
                handleOnChange('emailAddress', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.age}</span>
            </Form.Label>
            <Form.Control
              type="number"
              placeholder={_languageService.resources.age}
              onChange={x => handleOnChange('age', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.identificationNumber}</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.identificationNumber}
              onChange={x =>
                handleOnChange('identificationNumber', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.address}</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.address}
              onChange={x => handleOnChange('address', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.scope}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.scope}
              onChange={x => handleScopeChange(x.currentTarget.value)}
            >
              <option value={Scope.ALL}>
                {_languageService.resources.scopes[Scope.ALL]}
              </option>
              <option value={Scope.City}>
                {_languageService.resources.scopes[Scope.City]}
              </option>
              <option value={Scope.Area}>
                {_languageService.resources.scopes[Scope.Area]}
              </option>
              <option value={Scope.Station}>
                {_languageService.resources.scopes[Scope.Station]}
              </option>
              <option value={Scope.Self}>
                {_languageService.resources.scopes[Scope.Self]}
              </option>
            </Form.Select>
          </Form.Group>

          {formData.scope === Scope.City && (
            <Form.Group className="mb-2">
              <Form.Label>
                <span>{_languageService.resources.city}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                value={formData.cityId}
                onChange={x =>
                  handleOnChange('cityId', parseInt(x.currentTarget.value))
                }
              >
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                {cities.map((city, index) => (
                  <option key={index} value={city.id}>
                    {_languageService.isRTL
                      ? city.arabicName
                      : city.englishName}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
          )}

          {formData.scope === Scope.Area && (
            <Form.Group className="mb-2">
              <Form.Label>
                <span>{_languageService.resources.area}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                value={formData.areaId}
                onChange={x =>
                  handleOnChange('areaId', parseInt(x.currentTarget.value))
                }
              >
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                {areas.map((area, index) => (
                  <option key={index} value={area.id}>
                    {_languageService.isRTL
                      ? area.arabicName
                      : area.englishName}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
          )}

          {formData.scope === Scope.Station && (
            <Form.Group className="mb-2">
              <Form.Label>
                <span>{_languageService.resources.station}</span>
                <span className="text-danger fw-bold">*</span>
              </Form.Label>
              <Form.Select
                value={formData.stationId}
                onChange={x =>
                  handleOnChange('stationId', parseInt(x.currentTarget.value))
                }
              >
                <option value={-1}>
                  {_languageService.resources.selectOption}
                </option>
                {stations.map((station, index) => (
                  <option key={index} value={station.id}>
                    {_languageService.isRTL
                      ? station.arabicName
                      : station.englishName}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
          )}

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.fullName.trim() === '' ||
              formData.cardNumber.trim() === '' ||
              formData.userName.trim() === '' ||
              formData.password.trim() === '' ||
              formData.confirmPassword.trim() === '' ||
              formData.roleId === -1 ||
              (formData.scope === Scope.City && formData.cityId === -1) ||
              (formData.scope === Scope.Area && formData.areaId === -1) ||
              (formData.scope === Scope.Station && formData.stationId === -1)
            }
          >
            {_languageService.resources.create}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Create;
