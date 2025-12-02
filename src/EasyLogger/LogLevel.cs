// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        LogLevel.cs                                                  ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Defines the severity levels for logging messages             ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;

/// <summary>Defines the severity levels for logging messages.</summary>
public enum LogLevel {
    /// <summary>Informational message.</summary>
    Info,
    /// <summary>Warning message indicating a potential issue.</summary>
    Warning,
    /// <summary>Error message indicating a serious problem.</summary>
    Error,
    /// <summary>Debug message for development and troubleshooting.</summary>
    Debug
}
