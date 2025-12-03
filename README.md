# EasyLogger

EasyLogger is a simple, lightweight logging library for .NET applications. It provides a static API for writing log messages to various outputs with minimal setup.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/EasyLogger.svg)](https://www.nuget.org/packages/EasyLogger)

## Features

- **Simple Static API**: Easy-to-use static methods for logging (`Info`, `Warning`, `Error`, `Debug`).
- **Multiple Outputs**: Support for Console and File logging.
- **Context Awareness**: Automatically captures caller member name and line number.
- **Configurable**: Toggle outputs and debug logging at runtime.
- **Test-Friendly**: In-memory storage and reset capabilities for unit testing.
- **Thread-Safe**: Designed for concurrent use.

## Installation

Install the package via NuGet:

```bash
dotnet add package EasyLogger
```

## Usage

### Basic Logging

Simply call the static methods on the `Logger` class:

```csharp
using EasyLogger;

Logger.Info("Application started");
Logger.Warning("Disk space is low");
```

### Error Logging

Log exceptions with the `Error` method:

```csharp
try
{
    // ... risky code ...
}
catch (Exception ex)
{
    Logger.Error("Failed to process request", ex);
}
```

### Configuration

Configure the logger behavior by setting static properties:

```csharp
// Enable file logging (disabled by default)
Logger.UseFile = true;

// Disable console logging (enabled by default)
Logger.UseConsole = false;

// Enable debug messages (disabled by default)
Logger.EnableDebugLogging = true;

Logger.Debug("This message will now be logged");
```

### Testing Support

EasyLogger includes features to help with unit testing:

```csharp
// Clear state between tests
Logger.Reset();

// Verify logged messages
Logger.Info("Test message");
var messages = Logger.GetMessages();
Assert.Single(messages);
Assert.Equal("Test message", messages[0].Text);
```

## Requirements

- .NET 10.0 or later

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
