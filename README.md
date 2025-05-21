# ShiftTrack (BE)

This is REST API following the principles of Clean Architecture.

## Project Description
ShiftTrack is a web application designed for managing employee work schedules.

## Technologies
- .NET 8.0
- ASP.NET Core
- Docker
- Entity Framework Core
- PostgreSQL
- Npgsql (PostgreSQL provider for EF Core)

## Overview
Following Clean Architecture principles, the project is structured into these layers:

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer but has no dependencies on any other
layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application
needs to access a notification service, a new interface would be added to the application and an implementation would be
created within the infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These
classes should be based on interfaces defined within the application layer.

### API Layer (Presentation)

This layer serves as the interface for external interactions through REST API controllers. It handles HTTP requests,
manages request filtering and includes various middleware components for request processing. The layer also contains API
configuration settings and manages dependency injection setup to ensure proper service resolution throughout the
application.