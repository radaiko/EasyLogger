// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        Logger.cs                                                    ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Provides static logging methods for writing messages at      ║
// ║                various severity levels                                      ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

using System.Runtime.CompilerServices;

namespace EasyLogger;

/// <summary>Provides static logging methods with support for multiple output targets and configurable behavior.</summary>
public static class Logger {
    /// <summary>Storage for maintaining logged messages in memory.</summary>
    private static readonly Storage Storage = new();

    /// <summary>Gets or sets a value indicating whether log messages should be written to the console.</summary>
    public static bool UseConsole { get; set; } = true;

    /// <summary>Gets or sets a value indicating whether log messages should be written to files.</summary>
    public static bool UseFile { get; set; }

    /// <summary>Gets or sets a value indicating whether debug log messages are enabled.</summary>
    public static bool EnableDebugLogging { get; set; }

    /// <summary>Logs an informational message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The name of the calling member (automatically populated).</param>
    /// <param name="lineNumber">The line number in the source code (automatically populated).</param>
    public static void Info(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        => Write(new LogMessage(LogLevel.Info, message, null, caller, lineNumber));

    /// <summary>Logs a warning message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The name of the calling member (automatically populated).</param>
    /// <param name="lineNumber">The line number in the source code (automatically populated).</param>
    public static void Warning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        => Write(new LogMessage(LogLevel.Warning, message, null, caller, lineNumber));

    /// <summary>Logs an error message with associated exception details.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="exception">The exception associated with the error.</param>
    /// <param name="caller">The name of the calling member (automatically populated).</param>
    /// <param name="lineNumber">The line number in the source code (automatically populated).</param>
    public static void Error(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        => Write(new LogMessage(LogLevel.Error, message, exception, caller, lineNumber));

    /// <summary>Logs a debug message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The name of the calling member (automatically populated).</param>
    /// <param name="lineNumber">The line number in the source code (automatically populated).</param>
    public static void Debug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        => Write(new LogMessage(LogLevel.Debug, message, null, caller, lineNumber));

    /// <summary>Resets all static state (flags and internal storage) to their default values.</summary>
    /// <remarks>This method is primarily intended for test cleanup to ensure isolated test execution.</remarks>
    public static void Reset() {
        UseConsole = true;
        UseFile = false;
        EnableDebugLogging = false;
        Storage.Clear();
    }

    /// <summary>Gets a read-only view of all logged messages in the internal storage.</summary>
    /// <remarks>This method is primarily intended for testing to verify logged content.</remarks>
    /// <returns>A read-only list of all log messages that have been stored.</returns>
    public static IReadOnlyList<LogMessage> GetMessages() {
        return Storage.GetAll();
    }

    /// <summary>Writes a log message to configured output targets.</summary>
    /// <param name="logMessage">The log message to write.</param>
    private static void Write(LogMessage logMessage) {
        if (logMessage.Level == LogLevel.Debug && !EnableDebugLogging) {
            return;
        }
        Storage.Add(logMessage);
        if (UseConsole)
            ConsoleWriter.Write(logMessage);
        if (UseFile)
            FileWriter.Write(logMessage);
    }
}
