import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import React from 'react';
import { Card, Form } from 'react-bootstrap';
import SelectAllCheckbox from './SelectAllCheckbox';

const _languageService = DependenciesInjector.services.languageService;

const ReportPermissions = ({
  permissions,
  onPermissionChange,
  onSelectAll,
  isGroupSelected,
  isGroupPartiallySelected,
  groupConfig
}) => {
  const getPermissionValue = (entity, action) => {
    const permissionKey = action ? `${entity}${action}` : entity;
    return permissions[permissionKey] || false;
  };

  const handlePermissionChange = (entity, action, value) => {
    const permissionKey = action ? `${entity}${action}` : entity;
    onPermissionChange(permissionKey, value);
  };

  const renderPermissionRow = (entity, actions, label) => {
    const isSelected = isGroupSelected ? isGroupSelected(entity) : false;
    const isPartiallySelected = isGroupPartiallySelected
      ? isGroupPartiallySelected(entity)
      : false;

    // Define the standard report actions in order
    const standardActions = ['Show', 'Filter'];

    return (
      <tr key={entity}>
        <td>{label}</td>
        {standardActions.map(action => {
          const hasAction = actions.includes(action);
          return (
            <td key={action} className="text-center">
              {hasAction ? (
                <Form.Check
                  checked={getPermissionValue(entity, action)}
                  onChange={e =>
                    handlePermissionChange(
                      entity,
                      action,
                      e.currentTarget.checked
                    )
                  }
                />
              ) : (
                '-'
              )}
            </td>
          );
        })}
        <td className="text-center">
          <SelectAllCheckbox
            checked={isSelected}
            indeterminate={isPartiallySelected}
            onChange={e => onSelectAll(entity, e.currentTarget.checked)}
          />
        </td>
      </tr>
    );
  };

  const entityLabels = {
    TimeReport: _languageService.resources.timeReports,
    TransactionReport: _languageService.resources.transactionReports,
    DeliveryReport: _languageService.resources.deliveryReports,
    RealTimeReport: _languageService.resources.realTimeReports
  };

  return (
    <Card className="reports-card mb-2">
      <table className="table table-stripe">
        <thead>
          <tr>
            <th>{_languageService.resources.reports}</th>
            <td>
              <small>{_languageService.resources.showReport}</small>
            </td>
            <td>
              <small>{_languageService.resources.filterByEmployee}</small>
            </td>
            <td>
              <small>{_languageService.resources.checkAll}</small>
            </td>
          </tr>
        </thead>
        <tbody>
          {Object.entries(groupConfig).map(([entity, actions]) =>
            renderPermissionRow(entity, actions, entityLabels[entity])
          )}
        </tbody>
      </table>
    </Card>
  );
};

export default ReportPermissions;
