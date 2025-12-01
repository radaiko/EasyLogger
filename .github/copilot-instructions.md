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

