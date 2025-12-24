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
  // const _roleService = useService(Services.RoleService);
  const _employeeService = useService(Services.EmployeeService);
  const _stationService = useService(Services.StationService);

  // States
  const [formData, setFormData] = useState({
    fullName: '',
    cardNumber: '',
    userName: '',
    password: '',
    confirmPassword: '',
    groupId: -1,
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
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  useEffect(() => {
    loadDropdownData();
  }, []);

  const loadDropdownData = async () => {
    const groupResponse = await _groupService.getAllAsync();
    const stationResponse = await _stationService.getAllAsync();
    setGroups(groupResponse);
    setStations(stationResponse);
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
      groupId: formData.groupId,
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
              <span>{_languageService.resources.group}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              onChange={x =>
                handleOnChange('groupId', parseInt(x.currentTarget.value))
              }
            >
              <option value={-1}>
                {_languageService.resources.selectOption}
              </option>
              {groups.map((group, index) => (
                <option key={index} value={group.id}>
                  {_languageService.isRTL
                    ? group.arabicName
                    : group.englishName}
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
              <span>{_languageService.resources.station}</span>
            </Form.Label>
            <Form.Select
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

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.fullName.trim() === '' ||
              formData.cardNumber.trim() === '' ||
              formData.userName.trim() === '' ||
              formData.password.trim() === '' ||
              formData.confirmPassword.trim() === '' ||
              formData.groupId === -1
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
