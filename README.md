# EasyLogger

A simple and easy-to-use logging library for .NET 10 applications.

## Installation

```bash
dotnet add package EasyLogger
```

## Usage

### Basic Usage

```csharp
using EasyLogger;

// Create a console logger
var logger = new ConsoleLogger();

// Log messages at different levels
logger.Debug("This is a debug message");
logger.Info("This is an informational message");
logger.Warning("This is a warning message");
logger.Error("This is an error message");
```

### Setting Minimum Log Level

```csharp
var logger = new ConsoleLogger
{
    MinimumLevel = LogLevel.Warning
};

// This will NOT be logged (below minimum level)
logger.Info("This message will be ignored");

// This WILL be logged
logger.Warning("This message will be displayed");
```

## Log Levels

| Level   | Description                                       |
|---------|---------------------------------------------------|
| Debug   | Detailed diagnostic information                   |
| Info    | General operational messages                      |
| Warning | Potentially harmful situations                    |
| Error   | Error events that might still allow continuation  |

## Features

- Simple and intuitive API
- Color-coded console output
- Configurable minimum log level
- Thread-safe logging
- UTC timestamps in log messages

## Building from Source

```bash
# Clone the repository
git clone https://github.com/radaiko/EasyLogger.git
cd EasyLogger

# Build the solution
dotnet build

# Run tests
dotnet test
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
