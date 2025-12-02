# GitHub Copilot Instructions for EasyLogger

## Project Overview

EasyLogger is a simple, lightweight logging library for .NET applications. It provides static logging methods for writing messages at various severity levels (Info, Warning, Error, Debug) with support for multiple output targets (console and file).

## Technology Stack

- **Language:** C# (latest)
- **Framework:** .NET 10.0
- **Testing Framework:** MSTest 4.0.1
- **Solution Format:** slnx (modern solution format)
- **Nullable Reference Types:** Enabled
- **Implicit Usings:** Enabled

## Build, Test, and Lint Commands

All commands should be run from the `src/` directory:

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal
```

## Project Structure

```
EasyLogger/
├── .github/
│   ├── copilot-instructions.md    # This file - Copilot instructions
│   └── workflows/                 # GitHub Actions workflows
├── src/
│   ├── EasyLogger/                # Main library project
│   │   ├── EasyLogger.csproj
│   │   ├── Logger.cs              # Main static logger class
│   │   ├── LogLevel.cs            # Enum for log severity levels
│   │   ├── LogMessage.cs          # Log message data structure
│   │   ├── Storage.cs             # In-memory log storage
│   │   ├── ConsoleWriter.cs       # Console output writer
│   │   └── FileWriter.cs          # File output writer
│   ├── EasyLogger.Tests/          # Unit test project
│   │   ├── EasyLogger.Tests.csproj
│   │   ├── LoggerTests.cs         # Tests for Logger class
│   │   ├── StorageTests.cs        # Tests for Storage class
│   │   └── MSTestSettings.cs      # MSTest configuration
│   └── EasyLogger.slnx            # Solution file
├── .editorconfig                  # Code style configuration
├── .gitignore
├── LICENSE
└── README.md
```

## Coding Conventions

### General Guidelines

1. Use XML documentation comments (`///`) for all public types and members
2. Follow C# naming conventions (PascalCase for public members, _camelCase for private fields)
3. Use expression-bodied members where appropriate for single-line implementations
4. Prefer `readonly` for fields that don't change after construction
5. Use `volatile` for fields accessed from multiple threads without locking

### Test Guidelines

1. Use the Arrange-Act-Assert pattern for all tests
2. Use `#region` blocks to organize tests by category
3. Add `[DoNotParallelize]` attribute when tests share static state
4. Include `[TestInitialize]` and `[TestCleanup]` methods for proper test isolation
5. Use descriptive test method names following the pattern: `MethodName_Scenario_ExpectedBehavior`

### Code Examples

**Do:**
```csharp
/// <summary>Logs an informational message.</summary>
/// <param name="message">The message to log.</param>
public static void Info(string message)
    => Write(new LogMessage(LogLevel.Info, message, null));
```

**Don't:**
```csharp
// Missing XML documentation
// Inconsistent formatting
public static void Info(string message) { Write(new LogMessage(LogLevel.Info, message, null)); }
```

## Boundaries and Restrictions

### Do NOT:

- Modify files in `bin/`, `obj/`, or `.nuget/` directories
- Commit generated files or build artifacts
- Add dependencies without checking for security vulnerabilities
- Modify the LICENSE file
- Remove or weaken existing test coverage
- Use `Thread.Sleep()` in tests without a compelling reason

### Always:

- Run `dotnet test` before submitting changes
- Ensure all new public APIs have XML documentation
- Follow the existing code style as defined in `.editorconfig`
- Add tests for new functionality

## File Header Format

Every C# file in this project should start with the following header format:

```csharp
// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        {FileName}.cs                                                ║
// ║   Created:     {Date in format: Month Day, Year}                             ║
// ║   Description: {Brief description of the file's purpose}                     ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝
```

### Example

```csharp
// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        LogLevel.cs                                                  ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Defines the severity levels for logging messages             ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;
```

### Rules

1. **File:** Always include the exact filename with `.cs` extension
2. **Created:** Use the full date format (Month Day, Year) when the file is created
3. **Description:** Keep it brief (1-2 lines max) and clearly state the file's purpose
4. **Alignment:** Each line (including the leading `// ` prefix) should be **82 characters total**. The inner content between the border characters (`║`) is 75 characters wide.
   - **Example count:** `// ║                                                                             ║` = 3 (prefix) + 79 (border + content + border) = 82 chars
5. **Borders:** Use the box-drawing characters as shown (`╔`, `═`, `╗`, `║`, `╚`, `╝`)

### When to Apply

**New Files:**
- Apply this header to **all new C# files** (.cs) created in the EasyLogger project

**Scope:**
- Apply headers only to files in the `src/` directory and its subdirectories
- Include files in both the `EasyLogger/` and `EasyLogger.Tests/` projects
- Do NOT apply headers to files outside the `src/` directory or other repo locations

**Retroactive Updates:**
- **NO** retroactive updates to existing files (existing headers should remain unchanged)
- Only apply this header when creating new files or substantially refactoring existing files as part of planned work

**Exceptions (Do NOT apply header):**
- Generated or auto-generated files (e.g., designer files like `*.Designer.cs`)
- Files in `bin/`, `obj/`, or `.nuget/` directories
- Third-party or vendor code (e.g., imported utility files)
- Automatically generated files from code generators, ORM tools, or other automated processes

**Edge Cases:**
- **Refactored files:** If you significantly refactor or restructure an existing file, you may add the header as part of that change
- **Partial updates:** If a file lacks a header but is only receiving a minor fix, do NOT add the header; only add it with intentional refactoring
- **Test helpers:** Utility files in `EasyLogger.Tests/` receive headers just like regular project files

### Additional Guidelines

- Always follow the header with the `namespace` declaration
- Add XML documentation comments (`///`) for public types and members
- Keep the file header as the first element in each file, before the namespace

---

**Last Updated:** December 1, 2025

