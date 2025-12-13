# Dependency Injection (DI) Documentation

## Overview

This project uses a custom dependency injection (DI) container pattern implemented through the `DI` class and `ServiceCollection`. The DI container manages service registration and automatic dependency resolution, supporting both singleton and transient service lifetimes.

## Core Components

### DI Class

The `DI` class provides a static container instance that serves as the global dependency injection container for the application.

**Location:** `src/app/core/utils/DI.ts`

```typescript
import ServiceCollection from "./ServiceCollection";

export default class DI {
  static container = new ServiceCollection();
}
```

### ServiceCollection

The `ServiceCollection` class is the underlying implementation that manages service registration and resolution. It supports:

- **Singleton Services**: One instance per service, created on first access and reused
- **Transient Services**: New instance created every time the service is requested
- **Automatic Dependency Resolution**: Uses TypeScript decorators and `reflect-metadata` to automatically resolve constructor dependencies

**Location:** `src/app/core/utils/ServiceCollection.ts`

## Service Registration

Services must be registered in the DI container before they can be used. Registration typically happens in the application entry point (`src/main.tsx`).

### Registering Singleton Services

Singleton services are created once and reused throughout the application lifecycle:

```typescript
import DI from "./app/core/utils/DI";
import Logger from "./app/services/loggers/Logger";
import HttpService from "./app/services/http/HttpService";

// Register services as singletons
DI.container.addSingleton(Logger);
DI.container.addSingleton(HttpService);
DI.container.addSingleton(HostEnvironment);
```

### Registering Transient Services

Transient services create a new instance each time they are requested:

```typescript
DI.container.addTransient(SomeService);
```

## Service Resolution

To retrieve a service from the container, use the `get` method:

```typescript
const logger = DI.container.get(Logger);
const httpService = DI.container.get(HttpService);
```

## Constructor Injection

The DI container automatically resolves dependencies through constructor injection. When a service is requested, the container:

1. Inspects the constructor parameters using `reflect-metadata`
2. Recursively resolves each dependency
3. Instantiates the service with resolved dependencies

### Example: Service with Dependencies

```typescript
export default class HttpService implements IHttpService {
  constructor(
    private readonly _logger: ILoggerService,
    private readonly _hostEnviroment: IHostEnvironment
  ) {
    // Dependencies are automatically injected
    this._api = this._hostEnviroment.Api;
  }
}
```

When `HttpService` is registered and resolved, the container will automatically:

1. Resolve `ILoggerService` (likely `Logger`)
2. Resolve `IHostEnvironment` (likely `HostEnvironment`)
3. Instantiate `HttpService` with these dependencies

### Example: Service Using Another Service

```typescript
export default class AuthService implements IAuthService {
  constructor(private readonly _httpService: IHttpService) {}

  async login(loginDto: LoginDto): Promise<void> {
    // Use injected HttpService
    await this._httpService.postData("/auth/login", loginDto);
  }
}
```

## Best Practices

### 1. Register Services in Application Entry Point

Register all services in `src/main.tsx` before the application renders:

```typescript
// Register services in dependency order
// (Services with no dependencies first)
DI.container.addSingleton(Logger);
DI.container.addSingleton(HostEnvironment);

// Then register services that depend on others
DI.container.addSingleton(HttpService); // Depends on Logger and HostEnvironment
DI.container.addSingleton(AuthService); // Depends on HttpService
```

### 2. Use Interfaces for Dependencies

Define interfaces for your services to enable loose coupling and easier testing:

```typescript
// Define interface
export default interface IHttpService {
  postData<T>(url: string, data?: T): Promise<Response | null>;
}

// Implement interface
export default class HttpService implements IHttpService {
  // Implementation
}
```

### 3. Constructor Injection Only

The DI container only supports constructor injection. Dependencies must be declared in the constructor:

```typescript
// ✅ Good: Constructor injection
constructor(private readonly _logger: ILoggerService) {}

// ❌ Bad: Property injection (not supported)
private _logger: ILoggerService;
```

### 4. Service Lifetime Selection

- **Use Singleton** for:

  - Stateless services
  - Services that maintain application-wide state
  - Expensive-to-create services (HTTP clients, loggers, etc.)

- **Use Transient** for:
  - Services that need fresh state per request
  - Services that should not be shared

### 5. Error Handling

If a service is not registered, the container will throw an error:

```
Error: Service not registered: ServiceName
```

Always ensure services are registered before they are used.

## Requirements

The DI container requires the `reflect-metadata` package for automatic dependency resolution. Ensure it's installed:

```bash
npm install reflect-metadata
```

The `reflect-metadata` import is included in `ServiceCollection.ts`:

```typescript
import "reflect-metadata";
```

## TypeScript Configuration

For `reflect-metadata` to work properly, ensure your `tsconfig.json` includes:

```json
{
  "compilerOptions": {
    "experimentalDecorators": true,
    "emitDecoratorMetadata": true
  }
}
```

## Example: Complete Service Setup

1. **Define the interface:**

```typescript
// src/app/core/interfaces/http/IHttpService.ts
export default interface IHttpService {
  postData<T>(url: string, data?: T): Promise<Response | null>;
}
```

2. **Implement the service:**

```typescript
// src/app/services/http/HttpService.ts
import IHttpService from "../../core/interfaces/http/IHttpService";
import ILoggerService from "../../core/interfaces/loggers/ILoggerService";

export default class HttpService implements IHttpService {
  constructor(private readonly _logger: ILoggerService) {}

  async postData<T>(url: string, data?: T): Promise<Response | null> {
    // Implementation
  }
}
```

3. **Register in main.tsx:**

```typescript
import DI from "./app/core/utils/DI";
import Logger from "./app/services/loggers/Logger";
import HttpService from "./app/services/http/HttpService";

DI.container.addSingleton(Logger);
DI.container.addSingleton(HttpService);
```

4. **Use the service:**

```typescript
import DI from "./app/core/utils/DI";
import HttpService from "./app/services/http/HttpService";

const httpService = DI.container.get(HttpService);
await httpService.postData("/api/data", { key: "value" });
```

## Architecture Benefits

- **Loose Coupling**: Services depend on interfaces, not concrete implementations
- **Testability**: Easy to mock dependencies for unit testing
- **Maintainability**: Centralized service management
- **Automatic Resolution**: No manual dependency wiring required
- **Type Safety**: Full TypeScript support with type inference
