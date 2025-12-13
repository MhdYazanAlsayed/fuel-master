# Permission System Documentation

## Overview

The FuelMaster application implements a comprehensive permission-based access control system that allows you to control visibility and access to different parts of the application based on user permissions.

## Architecture

### Core Components

1. **RoleManager** - Central service for checking permissions
2. **NavbarVerticalMenu** - Sidebar with permission-based visibility
3. **usePermissions Hook** - Custom hook for permission checking
4. **ProtectedComponent** - HOC and components for protecting routes/content
5. **Routes Configuration** - Permission definitions in route objects

## Permission Types

### 1. Report Permissions

- `TimeReportShow` - View time reports
- `TimeReportFilterByStation` - Filter time reports by station
- `TransactionReportShow` - View transaction reports
- `TransactionReportFilter` - Filter transaction reports
- `DeliveryReportShow` - View delivery reports
- `DeliveryReportFilterByStation` - Filter delivery reports by station
- `RealTimeReportShow` - View real-time reports
- `RealTimeReportFilterByStation` - Filter real-time reports by station

### 2. CRUD Permissions

- `CitiesShow/Create/Edit/Delete` - City management
- `ZonesShow/Create/Edit/Delete` - Zone management
- `StationsShow/Create/Edit/Delete` - Station management
- `TanksShow/Create/Edit/Delete` - Tank management
- `PumpsShow/Create/Edit/Delete` - Pump management
- `NozzlesShow/Create/Edit/Delete` - Nozzle management
- `DeliveriesCreate` - Create deliveries
- `EmployeesShow/Create/Edit` - Employee management
- `GroupsShow/Create/Edit` - Group management

### 3. Other Permissions

- `ShowPrices` - View zone prices
- `ChangePrices` - Modify prices
- `ShowPricesHistories` - View price history
- `EditPermissions` - Manage permissions

## Usage Examples

### 1. Sidebar Navigation

The sidebar automatically hides/shows menu items based on permissions:

```javascript
// In routes.js
{
  name: 'Cities',
  permissions: ['CitiesShow', 'CitiesCreate'], // Show if user has ANY of these
  children: [
    {
      name: 'Show all',
      permissions: 'CitiesShow', // Single permission
      to: '/cities'
    },
    {
      name: 'Create new',
      permissions: 'CitiesCreate',
      to: '/cities/create'
    }
  ]
}
```

### 2. Using the usePermissions Hook

```javascript
import { usePermissions } from 'hooks/usePermissions';

const MyComponent = () => {
  const { canAccess, hasPermission, hasAllPermissions } = usePermissions([
    'CitiesShow',
    'CitiesCreate'
  ]);

  if (!canAccess) {
    return <div>Access denied</div>;
  }

  return (
    <div>
      {hasPermission('CitiesCreate') && <button>Create New City</button>}

      {hasAllPermissions(['CitiesShow', 'CitiesEdit']) && (
        <button>Advanced Options</button>
      )}
    </div>
  );
};
```

### 3. Using ProtectedComponent HOC

```javascript
import { withPermissions } from 'components/shared/ProtectedComponent';

const CitiesList = () => {
  return <div>List of cities</div>;
};

// Protect the component
const ProtectedCitiesList = withPermissions(
  CitiesList,
  'CitiesShow',
  '/errors/404'
);

// Or with multiple permissions (require ALL)
const ProtectedCitiesList = withPermissions(
  CitiesList,
  ['CitiesShow', 'CitiesEdit'],
  '/errors/404'
);
```

### 4. Using PermissionGate Component

```javascript
import { PermissionGate } from 'components/shared/ProtectedComponent';

const MyComponent = () => {
  return (
    <div>
      <h1>Dashboard</h1>

      <PermissionGate permissions="CitiesShow">
        <CitiesWidget />
      </PermissionGate>

      <PermissionGate
        permissions={['CitiesCreate', 'CitiesEdit']}
        requireAll={true}
        fallback={<div>You need both create and edit permissions</div>}
      >
        <AdvancedCitiesWidget />
      </PermissionGate>
    </div>
  );
};
```

### 5. Using ProtectedComponent Wrapper

```javascript
import { ProtectedComponent } from 'components/shared/ProtectedComponent';

const MyComponent = () => {
  return (
    <ProtectedComponent
      permissions="CitiesShow"
      fallback={<div>Access denied</div>}
    >
      <CitiesList />
    </ProtectedComponent>
  );
};
```

## Route Configuration

### Permission Formats

1. **Single Permission**

```javascript
{
  name: 'Cities',
  permissions: 'CitiesShow'
}
```

2. **Multiple Permissions (ANY)**

```javascript
{
  name: 'Cities',
  permissions: ['CitiesShow', 'CitiesCreate'] // Show if user has ANY
}
```

3. **Multiple Permissions (ALL)**

```javascript
{
  name: 'Advanced Cities',
  permissions: ['CitiesShow', 'CitiesEdit'] // Show if user has ALL
}
```

4. **Object Format**

```javascript
{
  name: 'Cities',
  permissions: {
    view: 'CitiesShow',
    manage: ['CitiesCreate', 'CitiesEdit']
  }
}
```

### Parent-Child Permission Logic

- If a parent route has permissions, it will only show if the user has those permissions
- If a parent route has children, it will show if ANY child is accessible
- Children are filtered to only show accessible items
- If no children are accessible, the parent is hidden

## Best Practices

### 1. Permission Naming

- Use descriptive names: `CitiesShow`, `CitiesCreate`, etc.
- Follow the pattern: `{Entity}{Action}`
- Be consistent across the application

### 2. Route Organization

- Group related permissions together
- Use parent permissions to control section access
- Keep child permissions specific to actions

### 3. Performance

- The permission system is optimized with memoization
- Permission checks are cached and only re-evaluated when needed
- Use the `usePermissions` hook for reactive permission checking

### 4. Security

- Always check permissions on both client and server side
- Don't rely solely on UI hiding for security
- Use the permission system as a UX enhancement, not security control

## Debugging

### Console Logging

The permission system includes debug logging:

```javascript
// Enable debug logging
console.debug = console.log; // In development

// Check permissions in console
const { hasPermission } = usePermissions();
console.log('Can access cities:', hasPermission('CitiesShow'));
```

### Common Issues

1. **Menu items not showing**

   - Check if permissions are correctly defined in routes
   - Verify user has the required permissions
   - Check console for permission denial messages

2. **Incorrect permission checks**

   - Ensure permission names match exactly
   - Check if using array vs string format correctly
   - Verify role manager is working properly

3. **Performance issues**
   - Use `usePermissions` hook instead of direct role manager calls
   - Avoid checking permissions in render loops
   - Use memoization for expensive permission checks

## Migration Guide

### From Old Permission System

1. **Update route configurations**

   ```javascript
   // Old
   permissions: 'CitiesShow';

   // New (same, but more flexible)
   permissions: 'CitiesShow';
   // or
   permissions: ['CitiesShow', 'CitiesCreate'];
   ```

2. **Use new hooks**

   ```javascript
   // Old
   const hasPermission = _roleManager.check('CitiesShow');

   // New
   const { hasPermission } = usePermissions();
   const canAccess = hasPermission('CitiesShow');
   ```

3. **Protect components**

   ```javascript
   // Old
   if (!_roleManager.check('CitiesShow')) return null;

   // New
   const { canAccess } = usePermissions('CitiesShow');
   if (!canAccess) return null;
   ```

## API Reference

### usePermissions Hook

```javascript
const {
  canAccess, // boolean - if the passed permissions are satisfied
  hasPermission, // function(permission) - check single permission
  hasAnyPermission, // function(permissions) - check if any permission is satisfied
  hasAllPermissions, // function(permissions) - check if all permissions are satisfied
  getAllPermissions // function() - get all user permissions
} = usePermissions(permissions);
```

### ProtectedComponent HOC

```javascript
withPermissions(
  Component, // React component to protect
  requiredPermissions, // string|array|object - required permissions
  redirectTo, // string - redirect path (default: '/errors/404')
  fallbackComponent // React component - show instead of redirecting
);
```

### PermissionGate Component

```javascript
<PermissionGate
  permissions={permissions} // required - permissions to check
  requireAll={false} // boolean - require all permissions (default: false)
  fallback={null} // React node - show if no permissions
>
  {children}
</PermissionGate>
```

This permission system provides a robust, flexible, and performant way to control access throughout your application while maintaining good user experience and security practices.
