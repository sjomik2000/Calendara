# Calendara

## 1. Brief description

Calendara is a robust calendar application built on modern .NET technologies that enables users to manage and track events efficiently. The application features an ASP.NET Core Web API backend adhering to RESTful principles, providing a scalable foundation for event management operations. Developed using Test-Driven Development (TDD) methodology, the project ensures high code quality and maintainability. The solution implements proper separation of concerns through a layered architecture and integrates with PostgreSQL for persistent data storage. Version control follows Git workflows with Azure DevOps integration, including CI/CD pipelines for automated testing and deployment.

## 2. Features

### A) CRUD operations
1) Create: Users can add new events to specific dates with required details (title, description, time), which are persisted in the database.
2) Read: Users can retrieve events by date range or specific criteria, with pagination support for large result sets ***.
3) Update: Existing events can be modified with new information while maintaining data integrity through proper validation.
4) Delete: Events can be permanently removed from the system with appropriate confirmation mechanisms.

### B) RESTful API Standards
The API follows REST architectural constraints:

* Resource-based URLs (/api/events/{id})
* Proper HTTP methods (GET, POST, PUT, DELETE)
* Stateless operations
* Standard HTTP status codes (200, 201, 400, 404, etc.)
* HATEOAS principles for discoverability
* JSON payloads for request/response bodies
* Consistent error handling with problem details (RFC 7807)

### C) Modular design
The application implements a clean, layered architecture:

1) API Layer:
  + Controllers handle HTTP requests/responses
  + Input validation and model mapping
  + Authentication/authorization***
  + API versioning support ***
2) Application Layer:
  + Business logic implementation
  + Domain services and DTOs ***
  + Transaction management ***
  + Exception handling
3) Data Access Layer:
  + Entity Framework Core data context
  + Repository pattern implementation
  + Database migrations ***
  + Query optimization ***
4) Contracts Layer:
  + Shared contracts defining API request and response structures

### D) Test-Driven Design (TDD) 
The project follows TDD practices using the xUnit framework:
  + Unit tests for all business logic (95%+ coverage)
  + Integration tests for API endpoints
  + Database tests with an in-memory provider
  + Mocking dependencies with Moq library ***
  + Test fixtures for complex scenarios
  + Continuous testing via Azure DevOps pipelines

Development workflow:
  1) Write a failing test
  2) Implement minimal code to pass the test
  3) Refactor while maintaining green tests
  4) Repeat for each feature

### E) Data Persistence 
The application uses PostgreSQL with Entity Framework Core for robust data management:
  + Code-first database design
  + Optimized data model for calendar operations
  + Proper indexing for frequent queries
  + Eager loading
  + Transaction support for data integrity ***
  + Connection pooling for performance ***
  + Migration history tracking ***
  + Configurable retry policies for transient failures ***

### F) CI/CD Pipelines
Azure DevOps pipelines ensure automated quality control:
  1) Build Pipeline:
      + Code compilation
      + NuGet package restoration
      + Static code analysis
      + Security scanning
  2) Test Pipeline:
      + Unit test execution
      + Integration test runs
      + Code coverage reporting
      + Test result publishing
  3) Release Pipeline:
      + Database migration application
      + Environment-specific configuration
      + Blue-green deployment strategy
      + Health verification
      + Rollback capabilities
     
## 3. Tasklist
- [x] Create a GitHub repository.
- [x] Create controller
- [x] Write response and request contracts
- [x] Mapping
- [ ] Api Endpoints
- [x] Service Layer 
- [ ] Database Layer 
- [ ] PostgreSQL config
- [ ] EF core Code first init
- [x] Validation

## 4. ChangeLog
+ 2025.04.14 - Created GitHub repository, added .gitignore and modeled project structure and features,
created README.MD with project breakdown.
+ 2025.04.15 - Created API, Application, Contracts and UnitTests base layers, started working 
on Controller class and Services class, registered Service DI, created Events model and 5 UnitTests.  
+ 2025.04.16 - Upgraded Project Framework from .NET Core 3.1 to .NET 8
+ 2025.04.16 - Finished writing Calendara.Contracts layer, finished writing all Controller actions and
mappings for requests and responses. Fully tested Create, GetById and GetAll methods.
+ 2025.04.17 - Created GitHub CI/CD Pipeline on dev.azure.com with self hosted pool. Fully tested Update 
method.
+ 2025.04.18 - Fully Tested Update, GetByDate, GetByDateRange and Delete controller methods. Finished 
writing Service layer and fully tested it. Finished writing Validation layer with mapping middleware and 
validation response failure and fully tested it. Finished writing repository layer for a temporary dictionary
database. Configured launchSettings.json and Startup.cs to launch API for specified port.
