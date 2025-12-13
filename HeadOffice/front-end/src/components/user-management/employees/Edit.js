import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import FormCard from 'components/shared/FormCard';
import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import Flex from 'components/theme/common/Flex';
import { Permissions } from 'app/core/enums/Permissions';

const _languageService = DependenciesInjector.services.languageService;
const _employeeService = DependenciesInjector.services.employeeSerivce;
const _groupService = DependenciesInjector.services.groupService;
const _stationService = DependenciesInjector.services.stationService;
const _roleManager = DependenciesInjector.services.roleManager;

const Edit = () => {
  if (!_roleManager.check(Permissions.EmployeesEdit))
    return <Navigate to="/errors/404" />;

  // States
  const [formData, setFormData] = useState({
    fullName: '',
    cardNumber: '',
    userName: '',
    groupId: -1,
    phoneNumber: '',
    emailAddress: '',
    age: '',
    identificationNumber: '',
    address: '',
    stationId: -1,
    isActive: false
  });
  const [groups, setGroups] = useState([]);
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);

  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  const { id } = useParams();
  if (!id) return <Navigate to={'/errors/404'} />;

  useEffect(() => {
    handleLoadComponentAsync();
  }, []);

  const handleLoadComponentAsync = async () => {
    await handleLoadDropdownData();
    await handleGetDetailsAsync();
    setLoading(false);
  };

  const handleLoadDropdownData = async () => {
    const groupResponse = await _groupService.getAllAsync();
    const stationResponse = await _stationService.getAllAsync();
    setGroups(groupResponse);
    setStations(stationResponse);
  };

  const handleGetDetailsAsync = async () => {
    const response = await _employeeService.detailsAsync(id);
    if (!response) return;

    setFormData({
      address: response.address ?? '',
      age: response.age?.toString() ?? '',
      cardNumber: response.cardNumber,
      emailAddress: response.emailAddress ?? '',
      fullName: response.fullName,
      groupId: response.user.groupId,
      identificationNumber: response.identificationNumber ?? '',
      isActive: response.user.isActive,
      phoneNumber: response.phoneNumber ?? '',
      stationId: response.stationId ?? -1,
      userName: response.user.userName
    });
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _employeeService.editAsync(id, {
      fullName: formData.fullName.trim(),
      cardNumber: formData.cardNumber.trim(),
      userName: formData.userName.trim(),
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
      stationId: formData.stationId === -1 ? null : formData.stationId,
      isActive: formData.isActive
    });
    if (!response.succeeded) return;

    navigate('/employees');
  };

  return (
    <FormCard
      header={_languageService.resources.editEmployee}
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
              value={formData.fullName}
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
              value={formData.cardNumber}
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
              value={formData.userName}
              placeholder={_languageService.resources.userName}
              onChange={x => handleOnChange('userName', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.group}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Select
              value={formData.groupId}
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
              value={formData.phoneNumber}
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
              value={formData.emailAddress}
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
              value={formData.age}
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
              value={formData.identificationNumber}
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
              value={formData.address}
              placeholder={_languageService.resources.address}
              onChange={x => handleOnChange('address', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.station}</span>
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

          <Form.Group className="py-3">
            <Flex alignItems={'center'} className={'gap-2'}>
              <Form.Check
                checked={formData.isActive}
                onChange={x =>
                  handleOnChange('isActive', x.currentTarget.checked)
                }
              />
              <Form.Label className={'mb-0'}>
                <span>{_languageService.resources.isActive}</span>
              </Form.Label>
            </Flex>
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.fullName.trim() === '' ||
              formData.cardNumber.trim() === '' ||
              formData.userName.trim() === '' ||
              formData.groupId === -1
            }
          >
            {_languageService.resources.edit}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Edit;
