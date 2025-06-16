# Solution Architecture

## Technical Stack
- Backend API: ASP.NET Core
- Frontend: ASP.NET Core MVC
- ORM: Entity Framework Core
- Database: SQL Server
- Testing: xUnit with AAA Pattern

## Architecture Decisions

### Layer Separation
The solution follows a strict layered architecture:
1. Presentation Layer (MVC)
2. Application Layer (API Services)
3. Domain Layer (Entities)
4. Infrastructure Layer (Repositories, DbContext)

### Repository Pattern Implementation
- Specific repositories for complex queries
- Unit of Work pattern for transaction management

### Testing Strategy
- AAA Pattern Usage:
  * Arrange: Setup test data and mocks
  * Act: Execute the operation being tested
  * Assert: Verify expected behavior
- Coverage includes:
  * Unit Tests
  * Performance Tests (via StopwatchLatencyTester)
## Trade-offs
- **Monolithic vs Microservices**: Chose monolithic for simplicity
- **EF Core vs Dapper**: EF for rapid development, trade-off in performance
