PACKAGES USED IN EVERY PROJECT
| Project                  | Package                                                         |
| ------------------------ | --------------------------------------------------------------- |
| **MyApp.Api**            | Microsoft.AspNetCore.Authentication.JwtBearer                   |
|                          | Microsoft.Extensions.Configuration                              |
|                          | Microsoft.Extensions.DependencyInjection                        |
|                          | FluentValidation                                                |
|                          | FluentValidation.AspNetCore                                     |
| **MyApp.Application**    | Microsoft.Extensions.DependencyInjection                        |
|                          | FluentValidation                                                |
|                          | **System.IdentityModel.Tokens.Jwt**                             |
|                          | **Microsoft.IdentityModel.Tokens**                              |
|                          | **Microsoft.Extensions.Options.ConfigurationExtensions**        |
| **MyApp.Infrastructure** | Microsoft.EntityFrameworkCore.SqlServer                         |
|                          | Microsoft.EntityFrameworkCore.Design                            |
|                          | Microsoft.Extensions.Options                                    |
|                          | Microsoft.Extensions.DependencyInjection                        |
| **MyApp.Common**         | Microsoft.Extensions.DependencyInjection                        |
| **MyApp.Domain**         | Microsoft.Extensions.DependencyInjection                        |


**PROJECT STRUCTURE**
src/
  MyApp.Api/
    MyApp.Api.csproj        // References: MyApp.Application, MyApp.Infrastructure, MyApp.Common, MyApp.Domain
    Program.cs
    appsettings.json
    Extensions/
      ServiceCollectionExtensions.cs
    Controllers/
      Future Controllers
  MyApp.Application/
    MyApp.Application.csproj // References: MyApp.Domain, MyApp.Common
    Interfaces/
      IJwtService.cs
    Services/
      JwtService.cs
    Dtos/
      LoginRequest.cs
      LoginResponse.cs
      UserDto.cs
    Behaviors/
      ValidationBehavior.cs
  MyApp.Domain/
    MyApp.Domain.csproj     // References: (none) — core entities and rules
    Entities/
      User.cs
    Exceptions/
      NotFoundException.cs
    ValueObjects/
      Email.cs
  MyApp.Infrastructure/
    MyApp.Infrastructure.csproj // References: MyApp.Domain, MyApp.Common
    Data/
      MyAppDbContext.cs
      EntityTypeConfigurations/
        UserAccountConfiguration.cs
    Repositories/
      IUserRepository.cs
      UserRepository.cs

  MyApp.Common/
    MyApp.Common.csproj     // References: (none) — shared utilities
    Constants/
      PolicyNames.cs
    Auth/
      JwtSettings.cs


**PROJECT REFERENCES**

Project References Explained

MyApp.Api

References MyApp.Application for business logic and DTOs.

References MyApp.Infrastructure for EF Core context and repositories.

References MyApp.Common for DI extensions and shared constants.

MyApp.Application

References MyApp.Domain to work with entities, value objects, and domain rules.

References MyApp.Common for pipeline behaviors, markers, and shared helpers.

MyApp.Infrastructure

References MyApp.Domain to map and configure domain entities in the DbContext.

References MyApp.Common for DI wiring and shared settings classes.

MyApp.Domain

Core project—does not reference any other layer to maintain independence.

MyApp.Common

Shared utilities and constants; does not depend on other projects.
