# GitHub Copilot Instructions for EasyLogger

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
4. **Alignment:** Ensure all content is centered and the width is 85 characters
5. **Borders:** Use the box-drawing characters as shown (`╔`, `═`, `╗`, `║`, `╚`, `╝`)

### When to Apply

- Apply this header to **all new C# files** (.cs)
- Update the header when creating new files in the EasyLogger project
- Keep the description concise and focused on the main responsibility of the file

### Additional Guidelines

- Always follow the header with the `namespace` declaration
- Add XML documentation comments (`///`) for public types and members
- Keep the file header as the first element in each file, before the namespace

---

**Last Updated:** December 1, 2025

