import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Card, Form } from 'react-bootstrap';

const _languageService = DependenciesInjector.services.languageService;

const OtherPermissions = ({ permissions, onPermissionChange, groupConfig }) => {
  const getPermissionValue = permissionKey => {
    return permissions[permissionKey] || false;
  };

  const handlePermissionChange = (permissionKey, value) => {
    onPermissionChange(permissionKey, value);
  };

  const renderPermissionRow = (permissionKey, label) => {
    return (
      <tr key={permissionKey}>
        <td>{label}</td>
        <td className="text-center">
          <Form.Check
            checked={getPermissionValue(permissionKey)}
            onChange={e =>
              handlePermissionChange(permissionKey, e.currentTarget.checked)
            }
          />
        </td>
      </tr>
    );
  };

  const permissionLabels = {
    ShowPrices: _languageService.resources.seeZonePrices,
    ChangePrices: _languageService.resources.changePrices,
    ShowPricesHistories: _languageService.resources.seeZonePricesHistories,
    EditPermissions: _languageService.resources.changePermissions
  };

  return (
    <Card className="mb-2">
      <table className="table table-stripe">
        <thead>
          <tr>
            <th>{_languageService.resources.otherPermissions}</th>
            <td></td>
          </tr>
        </thead>
        <tbody>
          {Object.entries(groupConfig).map(([permissionKey, actions]) =>
            renderPermissionRow(permissionKey, permissionLabels[permissionKey])
          )}
        </tbody>
      </table>
    </Card>
  );
};

export default OtherPermissions;
