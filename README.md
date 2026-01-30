# Left4Dead Troll Control

A WPF desktop application for managing and tracking troll players in Left 4 Dead. This application helps server administrators keep a database of problematic players and generate scripts to control their access.

## 📋 Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [Project Structure](#project-structure)
- [Database](#database)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

- **Troll Player Management**: Register, update, and delete troll players with their Steam IDs and profiles
- **Player Database**: Store player information including:
  - Steam ID
  - Profile URL
  - Alias/Nickname
  - Observations/Notes
  - Creation and update timestamps
- **Search and Filter**: Find specific troll players by Steam ID or alias
- **Script Generation**: Generate control scripts for server administration
- **Modern UI**: Clean and intuitive WPF interface with multiple pages:
  - Home page
  - Troll registration
  - Troll list view
  - Script generation
  - Settings

## 🛠 Technologies

### Core Framework
- **.NET 8.0** - Latest .NET framework for Windows applications
- **WPF (Windows Presentation Foundation)** - Modern Windows desktop UI framework

### Architecture & Patterns
- **Clean Architecture** - Separation of concerns with multiple layers
- **MVVM Pattern** - Model-View-ViewModel for UI logic separation
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - IoC container for loose coupling

### Libraries & Packages
- **Entity Framework Core 8.0.23** - ORM for database operations
- **SQLite** - Lightweight embedded database
- **Mapster 7.4.0** - High-performance object mapping
- **Microsoft.Extensions.DependencyInjection** - Built-in DI container

## 🏗 Architecture

The project follows Clean Architecture principles with clear separation of concerns:

```
Left4DeadTrollControl/
│
├── Left4DeadTrollControl.Domain/
│   ├── Entities/              # Domain entities (TrollPlayer)
│   └── Interfaces/            # Repository interfaces
│
├── Left4DeadTrollControl.Application/
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Filters/               # Query filters
│   ├── Interfaces/            # Service interfaces
│   ├── Services/              # Business logic services
│   └── ProfileMappings/       # Mapster mappings
│
├── Left4DeadTrollControl.Infrastructure.Persistence/
│   ├── Contexts/              # EF Core DbContext
│   ├── Repositories/          # Repository implementations
│   ├── Mappings/              # Entity configurations
│   └── Migrations/            # Database migrations
│
├── Left4DeadTrollControl.Infrastructure.IoC/
│   └── DependencyInjection    # DI container configuration
│
└── Left4DeadTrollControl.AppClient/
    ├── Pages/                 # WPF pages/views
    ├── ViewModels/            # MVVM view models
    └── DependencyInjection    # Client DI setup
```

### Layers Description

- **Domain Layer**: Contains business entities and core interfaces. No dependencies on other layers.
- **Application Layer**: Contains business logic, DTOs, services, and application interfaces.
- **Infrastructure Layer**: 
  - **Persistence**: Database context, repositories, and migrations
  - **IoC**: Dependency injection configuration
- **Presentation Layer**: WPF application with MVVM pattern implementation

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11 (for WPF support)
- Visual Studio 2022 or JetBrains Rider (recommended)
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/mchomem/Left4DeadTrollControl.git
   cd Left4DeadTrollControl
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update --project Left4DeadTrollControl.Infrastructure.Persistence --startup-project Left4DeadTrollControl.AppClient
   ```

### Running the Application

#### Using Visual Studio
1. Open the solution in Visual Studio 2022
2. Set `Left4DeadTrollControl.AppClient` as the startup project
3. Press `F5` or click the "Start" button

#### Using Command Line
```bash
dotnet run --project Left4DeadTrollControl.AppClient
```

## 📁 Project Structure

### Domain Entities

**TrollPlayer**
- Unique identifier (GUID)
- Steam ID (required)
- Profile URL
- Alias/Nickname
- Observations
- Timestamps (Created/Updated)

### Database

The application uses **SQLite** as its database, providing:
- Zero configuration
- Cross-platform compatibility
- Easy backup and portability
- Perfect for desktop applications

The database file is created automatically on first run.

### DTOs

- `TrollPlayerDto` - Full player data
- `TrollPlayerInsertDto` - Data for creating new players
- `TrollPlayerUpdateDto` - Data for updating existing players

## 💡 Usage

### Registering a Troll Player

1. Navigate to the "Troll Registration" page
2. Fill in the player information:
   - Steam ID (required)
   - Profile URL
   - Alias
   - Observations
3. Click "Save" to add the player to the database

### Viewing Troll Players

1. Navigate to the "Troll List" page
2. View all registered troll players
3. Use filters to search by Steam ID or alias
4. Edit or delete players as needed

### Generating Scripts

1. Navigate to the "Script Generation" page
2. Click "Generate Script" button
3. The application will create a server control script with all registered troll players

## 🧪 Running Migrations

To create a new migration after modifying entities:

```bash
dotnet ef migrations add MigrationName --project Left4DeadTrollControl.Infrastructure.Persistence --startup-project Left4DeadTrollControl.AppClient
```

To update the database:

```bash
dotnet ef database update --project Left4DeadTrollControl.Infrastructure.Persistence --startup-project Left4DeadTrollControl.AppClient
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 👤 Author

**mchomem**

## 🙏 Acknowledgments

- Built for the Left 4 Dead server administration community
- Inspired by the need for better troll player management tools
