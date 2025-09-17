# Permissions Management System

## Overview

This is a refactored permissions management system for the FuelMaster application. The system provides a clean, maintainable, and bug-free way to manage user permissions across different modules.

## Architecture

### Main Components

1. **Index.js** - Main container component that orchestrates the entire permissions system
2. **CrudPermissions.js** - Handles CRUD operations permissions (Cities, Zones, Stations, etc.)
3. **ReportPermissions.js** - Handles report-related permissions
4. **OtherPermissions.js** - Handles miscellaneous permissions (prices, permissions editing)
5. **SelectAllCheckbox.js** - Utility component for select-all functionality with indeterminate state

### Permission Groups Configuration

The system uses a centralized configuration object `PERMISSION_GROUPS` that defines how permissions are organized:

```javascript
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
    Stations: ['Show', 'Create', 'Edit', 'Delete']
    // ... more entities
  },
  other: {
    ShowPrices: [''],
    ChangePrices: [''],
    ShowPricesHistories: [''],
    EditPermissions: ['']
  }
};
```

## Key Features

### 1. Centralized State Management

- All permissions are managed in a single state object
- Consistent permission key naming based on the Permissions enum
- Automatic initialization of missing permissions

### 2. Select-All Functionality

- Each entity has a select-all checkbox
- Supports indeterminate state (partially selected)
- Automatically updates when individual permissions change

### 3. Error Handling

- Comprehensive error handling for API calls
- User-friendly error messages with retry functionality
- Loading states for better UX

### 4. Performance Optimizations

- Uses `useCallback` for memoized functions
- Efficient state updates
- Minimal re-renders

## Usage

### Basic Usage

```javascript
import PermissionsIndex from './components/user-management/groups/permissions/Index';

// The component automatically handles:
// - Loading permissions from API
// - Managing permission state
// - Handling select-all functionality
// - Submitting changes to the server
```

### Adding New Permissions

1. **Add to Permissions Enum** (`src/app/core/enums/Permissions.js`):

```javascript
export const Permissions = {
  // ... existing permissions
  NewPermission: 'NewPermission'
};
```

2. **Add to Permission Groups Configuration** (in `Index.js`):

```javascript
const PERMISSION_GROUPS = {
  // ... existing groups
  newGroup: {
    NewEntity: ['Show', 'Create', 'Edit', 'Delete']
  }
};
```

3. **Add to Component** (if needed):

```javascript
// Add to the appropriate component (CrudPermissions, ReportPermissions, etc.)
// The system will automatically handle the rest
```

## API Integration

### Expected API Response Format

```javascript
[
  { key: 'CitiesShow', value: true },
  { key: 'CitiesCreate', value: false }
  // ... more permissions
];
```

### API Endpoints

- **GET** `/api/permissions/{groupId}` - Fetch permissions for a group
- **PUT** `/api/permissions/{groupId}` - Update permissions for a group

## Styling

The component uses Bootstrap classes and custom CSS:

```css
.permissions .card .card-header p {
  font-size: 0.9rem;
}
.permissions small {
  font-size: 0.9rem;
  font-weight: 600;
}
.permissions table thead td {
  width: 100px;
  text-align: center;
}
```

## Dependencies

- React 16.8+ (for hooks)
- React Bootstrap
- React Router (for navigation)
- React Toastify (for notifications)

## Security

- Permission checks are performed at component level
- Users without `EditPermissions` are redirected to 404
- All API calls are validated and error-handled

## Testing

The refactored system is designed to be easily testable:

- Pure functions for permission logic
- Separated concerns for easier unit testing
- Clear input/output contracts
- Minimal side effects

## Migration from Old System

The refactored system maintains backward compatibility with the existing API while providing:

- Better error handling
- Improved performance
- Cleaner code structure
- Easier maintenance
- Better user experience

## Troubleshooting

### Common Issues

1. **Permissions not loading**: Check API endpoint and network connectivity
2. **Select-all not working**: Verify permission keys match the enum
3. **State not updating**: Ensure proper use of `useCallback` dependencies

### Debug Mode

Enable console logging by adding `console.log` statements in the main component for debugging permission state changes.
