import React, { useEffect, useState, useCallback, useMemo } from 'react';
import CrudPermissions from './CrudPermissions';
import Header from './Header';
import ReportPermissions from './ReportPermissions';
import OtherPermissions from './OtherPermissions';
import 'components/styles/permissions/permissions.css';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Loader from 'components/shared/Loader';
import { Navigate, useParams } from 'react-router-dom';
import { Permissions } from 'app/core/enums/Permissions';
import { Button } from 'react-bootstrap';
import { toast } from 'react-toastify';

const _permissionService = DependenciesInjector.services.permissionService;
const _languageService = DependenciesInjector.services.languageService;
const _roleManager = DependenciesInjector.services.roleManager;

// Permission groups configuration
const PERMISSION_GROUPS = {
  reports: {
    TimeReport: ['Show', 'FilterByStation'],
    TransactionReport: ['Show', 'Filter'],
    DeliveryReport: ['Show', 'FilterByStation'],
    RealTimeReport: ['Show', 'FilterByStation']
  },
  crud: {
    Cities: ['Show', 'Create', 'Edit', 'Delete'],
    Zones: ['Show', 'Create', 'Edit', 'Delete'],
    Stations: ['Show', 'Create', 'Edit', 'Delete'],
    Tanks: ['Show', 'Create', 'Edit', 'Delete'],
    Pumps: ['Show', 'Create', 'Edit', 'Delete'],
    Nozzles: ['Show', 'Create', 'Edit', 'Delete'],
    Deliveries: ['Create'],
    Employees: ['Show', 'Create', 'Edit'],
    Groups: ['Show', 'Create', 'Edit']
  },
  other: {
    ShowPrices: [''],
    ChangePrices: [''],
    ShowPricesHistories: [''],
    EditPermissions: ['']
  }
};

const Index = () => {
  const { id } = useParams();

  // Early return for missing permissions or ID
  if (!_roleManager.check(Permissions.EditPermissions)) {
    return <Navigate to="/errors/404" />;
  }

  if (!id) {
    return <Navigate to="/errors/404" />;
  }

  // States
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [permissions, setPermissions] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Initialize permissions state
  const initializePermissions = useCallback(() => {
    const initialPermissions = {};

    // Initialize all permissions from the enum
    Object.values(Permissions).forEach(permission => {
      initialPermissions[permission] = false;
    });

    setPermissions(initialPermissions);
  }, []);

  // Load permissions from API
  const loadPermissions = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const response = await _permissionService.getAsync(id);

      if (!response) {
        setError('Failed to load permissions');
        return;
      }

      if (response.length === 0) {
        initializePermissions();
        return;
      }

      // Convert API response to permissions object
      const permissionsMap = {};
      response.forEach(permission => {
        permissionsMap[permission.key] = permission.value;
      });

      // Initialize missing permissions as false
      Object.values(Permissions).forEach(permission => {
        if (!(permission in permissionsMap)) {
          permissionsMap[permission] = false;
        }
      });

      setPermissions(permissionsMap);
    } catch (err) {
      console.error('Error loading permissions:', err);
      setError('Failed to load permissions');
    } finally {
      setLoading(false);
    }
  }, [id, initializePermissions]);

  // Handle permission change
  const handlePermissionChange = useCallback((permissionKey, value) => {
    setPermissions(prev => ({
      ...prev,
      [permissionKey]: value
    }));
  }, []);

  // Handle select all for an entity
  const handleSelectAll = useCallback((entity, value) => {
    // Check all permission groups for the entity
    const entityPermissions =
      PERMISSION_GROUPS.crud[entity] ||
      PERMISSION_GROUPS.reports[entity] ||
      PERMISSION_GROUPS.other[entity];

    if (!entityPermissions) {
      console.warn(`No permissions found for entity: ${entity}`);
      return;
    }

    const updates = {};

    entityPermissions.forEach(action => {
      const permissionKey = action ? `${entity}${action}` : entity;
      if (permissionKey in Permissions) {
        updates[permissionKey] = value;
      }
    });

    console.log(`Select all for ${entity}:`, { value, updates });

    setPermissions(prev => ({
      ...prev,
      ...updates
    }));
  }, []);

  // Check if all permissions for an entity are selected
  const isEntitySelected = useCallback(
    entity => {
      // Check all permission groups for the entity
      const entityPermissions =
        PERMISSION_GROUPS.crud[entity] ||
        PERMISSION_GROUPS.reports[entity] ||
        PERMISSION_GROUPS.other[entity];

      if (!entityPermissions) return false;

      const entityPermissionKeys = entityPermissions
        .map(action => {
          const permissionKey = action ? `${entity}${action}` : entity;
          return permissionKey;
        })
        .filter(key => key in Permissions);

      return (
        entityPermissionKeys.length > 0 &&
        entityPermissionKeys.every(key => permissions[key])
      );
    },
    [permissions]
  );

  // Check if any permission for an entity is selected
  const isEntityPartiallySelected = useCallback(
    entity => {
      // Check all permission groups for the entity
      const entityPermissions =
        PERMISSION_GROUPS.crud[entity] ||
        PERMISSION_GROUPS.reports[entity] ||
        PERMISSION_GROUPS.other[entity];

      if (!entityPermissions) return false;

      const entityPermissionKeys = entityPermissions
        .map(action => {
          const permissionKey = action ? `${entity}${action}` : entity;
          return permissionKey;
        })
        .filter(key => key in Permissions);

      const selectedCount = entityPermissionKeys.filter(
        key => permissions[key]
      ).length;
      return selectedCount > 0 && selectedCount < entityPermissionKeys.length;
    },
    [permissions]
  );

  // Handle form submission
  const handleSubmit = useCallback(
    async e => {
      e.preventDefault();

      try {
        setIsSubmitting(true);

        // Convert permissions object to API format
        const permissionsArray = Object.entries(permissions).map(
          ([key, value]) => ({
            key,
            value
          })
        );

        const response = await _permissionService.updateAsync(
          id,
          permissionsArray
        );

        if (!response.succeeded) {
          toast.error(
            _languageService.resources.taskFailed ||
              'Failed to update permissions'
          );
          return;
        }

        toast.success(
          _languageService.resources.taskSuccessfully ||
            'Permissions updated successfully'
        );
      } catch (err) {
        console.error('Error updating permissions:', err);
        toast.error(
          _languageService.resources.taskFailed ||
            'Failed to update permissions'
        );
      } finally {
        setIsSubmitting(false);
      }
    },
    [permissions, id]
  );

  // Load permissions on component mount
  useEffect(() => {
    loadPermissions();
  }, [loadPermissions]);

  // Show error state
  if (error) {
    return (
      <div className="permissions">
        <div className="alert alert-danger">
          {error}
          <Button variant="link" onClick={loadPermissions} className="p-0 ml-2">
            Retry
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="permissions">
      <form onSubmit={handleSubmit}>
        <Header />

        <Loader loading={loading}>
          <ReportPermissions
            permissions={permissions}
            onPermissionChange={handlePermissionChange}
            onSelectAll={handleSelectAll}
            isGroupSelected={isEntitySelected}
            isGroupPartiallySelected={isEntityPartiallySelected}
            groupConfig={PERMISSION_GROUPS.reports}
          />

          <CrudPermissions
            permissions={permissions}
            onPermissionChange={handlePermissionChange}
            onSelectAll={handleSelectAll}
            isGroupSelected={isEntitySelected}
            isGroupPartiallySelected={isEntityPartiallySelected}
            groupConfig={PERMISSION_GROUPS.crud}
          />

          <OtherPermissions
            permissions={permissions}
            onPermissionChange={handlePermissionChange}
            groupConfig={PERMISSION_GROUPS.other}
          />

          <Button
            variant="primary"
            type="submit"
            disabled={isSubmitting}
            className="mt-3"
          >
            {isSubmitting
              ? 'Saving...'
              : _languageService.resources.saveChanges || 'Save Changes'}
          </Button>
        </Loader>
      </form>
    </div>
  );
};

export default Index;
