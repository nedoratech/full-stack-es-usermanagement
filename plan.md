# Full Stack Event Sourcing Demo - User Registration

## Overview

A complete Event Sourcing demonstration showcasing:

1. React form where each field completion appends an event to IndexedDB
2. .NET Web API with User Registration aggregate using Event Sourcing with PostgreSQL
3. Client-side aggregate building from event replay
4. Component test demonstrating event stream replay to identify errors

## Project Structure

### Backend (.NET)

- `UserRegistration.UserManagement/` - Vertical slices implementation + Minimal API endpoints and extension methods
- `UserRegistration.Hosting/` - Web API hosting, Swagger setup, .http files, composition root
- `UserRegistration.Aggregate/` - Aggregate Root (User) with business logic
- `UserRegistration.Events/` - All domain events
- `UserRegistration.Storage/` - PostgreSQL event store using event sourcing library
- `UserRegistration.ComponentTests/` - Component/integration tests including replay demonstration

### Frontend (React)

- `user-registration-app/` - React application with form component and IndexedDB

## Implementation Details

### Events Layer (`UserRegistration.Events`)

- **Events:**
- `UserNameEntered` - field event
- `EmailEntered` - field event
- `PhoneEntered` - field event
- `AddressEntered` - field event
- `UserRegistered` - aggregate creation event
- `EmailVerified` - operation event
- `AccountActivated` - operation event
- `ProfileUpdated` - operation event
- **Base Interfaces:**
- `IEvent` - base interface for all events

### Aggregate Layer (`UserRegistration.Aggregate`)

- **Aggregate Root (`User`):**
- State: Name, Email, Phone, Address, IsEmailVerified, IsActivated
- Apply methods for each event
- Business logic: prevent activation before email verification, prevent duplicate registration
- **Interfaces:**
- `IAggregateRoot` - aggregate interface
- `IEventStore` - event store abstraction

### Storage Layer (`UserRegistration.Storage`)

- **PostgreSQL Event Store** - using event sourcing library (e.g., Marten, EventStore.Client, or custom)
- Store events by aggregate ID in PostgreSQL
- Retrieve event stream by aggregate ID
- Support event replay
- Migrations and database setup
- **Event Store Implementation:**
- Implement `IEventStore` interface
- PostgreSQL connection and schema setup
- Serialization/deserialization of events

### User Management Layer (`UserRegistration.UserManagement`)

- **Vertical Slices:**
- `RegisterUser/` - Register user slice (receive event stream, create aggregate)
- Handler, Request/Response models, endpoint registration
- `VerifyEmail/` - Verify email slice
- Handler, Request/Response models, endpoint registration
- `ActivateAccount/` - Activate account slice
- Handler, Request/Response models, endpoint registration
- `UpdateProfile/` - Update profile slice
- Handler, Request/Response models, endpoint registration
- `GetUserState/` - Get current user state slice
- Handler, Request/Response models, endpoint registration
- `GetUserEvents/` - Get event stream slice (for debugging)
- Handler, Request/Response models, endpoint registration
- **API Extensions:**
- `UserManagementExtensions.cs` - Minimal API endpoint registrations
- **Minimal API Endpoints:**
- `POST /api/users/{userId}/register` - receive event stream and create aggregate
- `POST /api/users/{userId}/verify-email` - verify email operation
- `POST /api/users/{userId}/activate` - activate account operation
- `POST /api/users/{userId}/update-profile` - update profile operation
- `GET /api/users/{userId}` - get current state
- `GET /api/users/{userId}/events` - get event stream (for debugging)

### Hosting Layer (`UserRegistration.Hosting`)

- **Composition Root:**
- `Program.cs` - Application startup and configuration
- Register all dependencies (IEventStore, services)
- Configure PostgreSQL connection string
- Configure CORS for React frontend
- **Swagger/OpenAPI:**
- Swagger configuration and setup
- API documentation
- **HTTP Files:**
- `.http` files for testing endpoints
- Sample requests for each endpoint
- **Middleware Configuration:**
- Request pipeline setup
- Error handling
- Logging configuration

### Frontend (`user-registration-app`)

- **React App:**
- `RegistrationForm` component
- Fields: Name, Email, Phone, Address
- On each field change/blur:
- Create event (e.g., `EmailEntered`)
- Store event in IndexedDB (events store)
- On Submit:
- Retrieve all events from IndexedDB
- Send complete event stream to API
- Build aggregate on frontend by replaying events
- Store aggregate in IndexedDB (aggregates store)
- Show current state from aggregate
- **IndexedDB Setup:**
- `events` objectStore - stores field events with timestamp and userId
- `aggregates` objectStore - stores built aggregates (userId as key)
- Database initialization and migration logic
- **Event Replay Service:**
- Client-side event replay logic to build aggregate state
- Applies events in order to reconstruct User aggregate
- Same replay logic as backend for consistency
- Display current user state from IndexedDB aggregate

### Component Tests (`UserRegistration.ComponentTests`)

- **Event Replay Test (`EventReplayTests`):**
- Test that replays event stream from PostgreSQL to find error
- Scenario: Try to activate account before email verification
- Demonstrate how replaying events shows exactly where business rule was violated
- Test that shows how events tell the "story" of what happened
- Integration tests with actual PostgreSQL event store
- Show how replay reveals the exact sequence of events that led to an error

## Key Files to Create

1. `UserRegistration.Events/Events/*.cs` - All domain events
2. `UserRegistration.Events/IEvent.cs` - Base event interface
3. `UserRegistration.Aggregate/User.cs` - Aggregate root
4. `UserRegistration.Aggregate/IAggregateRoot.cs` - Aggregate interface
5. `UserRegistration.Storage/IEventStore.cs` - Event store interface
6. `UserRegistration.Storage/PostgresEventStore.cs` - PostgreSQL event store implementation
7. `UserRegistration.Storage/DbContext/Migrations/` - Database migrations
8. `UserRegistration.UserManagement/RegisterUser/RegisterUserHandler.cs` - Register user handler
9. `UserRegistration.UserManagement/RegisterUser/RegisterUserRequest.cs` - Register user request model
10. `UserRegistration.UserManagement/VerifyEmail/VerifyEmailHandler.cs` - Verify email handler
11. `UserRegistration.UserManagement/VerifyEmail/VerifyEmailRequest.cs` - Verify email request model
12. `UserRegistration.UserManagement/ActivateAccount/ActivateAccountHandler.cs` - Activate account handler
13. `UserRegistration.UserManagement/ActivateAccount/ActivateAccountRequest.cs` - Activate account request model
14. `UserRegistration.UserManagement/UpdateProfile/UpdateProfileHandler.cs` - Update profile handler
15. `UserRegistration.UserManagement/UpdateProfile/UpdateProfileRequest.cs` - Update profile request model
16. `UserRegistration.UserManagement/GetUserState/GetUserStateHandler.cs` - Get user state handler
17. `UserRegistration.UserManagement/GetUserEvents/GetUserEventsHandler.cs` - Get user events handler
18. `UserRegistration.UserManagement/UserManagementExtensions.cs` - Minimal API extension methods
19. `UserRegistration.Hosting/Program.cs` - Hosting startup and composition root
20. `UserRegistration.Hosting/appsettings.json` - Configuration (PostgreSQL connection, CORS)
21. `UserRegistration.Hosting/.http` - HTTP test files
22. `UserRegistration.Hosting/` - Swagger configuration files
23. `user-registration-app/src/RegistrationForm.jsx` - React form component
24. `user-registration-app/src/db/indexedDb.js` - IndexedDB setup (events and aggregates stores)
25. `user-registration-app/src/services/eventReplay.js` - Client-side event replay logic
26. `UserRegistration.ComponentTests/EventReplayTests.cs` - Replay demonstration test

## Demo Flow

1. User fills form → each field creates event and stores in IndexedDB (events store)
2. Submit form → retrieves events from IndexedDB, sends event stream to API
3. UserManagement stores events in PostgreSQL event store, creates User aggregate
4. Frontend builds aggregate by replaying events, stores in IndexedDB (aggregates store)
5. Perform operations (verify email, activate) → generate more events in PostgreSQL
6. Show component test that replays events from PostgreSQL to demonstrate how event stream reveals errors
7. Demonstrate how IndexedDB serves as client-side event store and aggregate projection
