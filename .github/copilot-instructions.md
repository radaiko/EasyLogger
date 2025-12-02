# Copilot Instructions for EasyLogger

## Project Overview

EasyLogger is a C# logging library project. This repository contains .NET solutions and projects.

## Technology Stack

- **Language**: C#
- **Framework**: .NET
- **Build System**: MSBuild / dotnet CLI
- **Solution Format**: `.slnx` (XML-based solution format)

## Coding Conventions

This project follows the coding style defined in `.editorconfig`:

- Use `var` for variable declarations (for built-in types, when type is apparent, and elsewhere)
- Prefer expression-bodied members for single-line implementations
- Use file-scoped namespaces
- Avoid `this.` qualification
- Use pattern matching over type checks
- Prefer braces only when multiline
- No new line before opening braces
- 4-space indentation
- UTF-8 encoding with LF line endings

## Build and Test Commands

```bash
# Restore dependencies
dotnet restore src/EasyLogger.slnx

# Build the solution
dotnet build src/EasyLogger.slnx

# Run tests (when available)
dotnet test src/EasyLogger.slnx
```

## Project Structure

- `src/` - Source code and solution files
- `.editorconfig` - Coding style configuration
