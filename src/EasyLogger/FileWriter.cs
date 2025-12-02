// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        FileWriter.cs                                                ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Writes log messages to files with rotation and cleanup       ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;

/// <summary>Writes log messages to files with support for rotation and cleanup operations.</summary>
internal static class FileWriter {
    /// <summary>Lock object for thread-safe file access.</summary>
    private static readonly Lock WriteLock = new();

    /// <summary>The path to the log file in the application directory.</summary>
    private static readonly string LogFilePath = Path.Combine(
        AppContext.BaseDirectory,
        "logs.txt"
    );

    /// <summary>Writes a log message to a file in a thread-safe manner.</summary>
    /// <param name="logMessage">The log message to write to file.</param>
    internal static void Write(LogMessage logMessage) {
        try {
            lock (WriteLock) {
                var formattedMessage = $"{logMessage}\n";
                File.AppendAllText(LogFilePath, formattedMessage);
            }
        }
        catch (Exception ex) {
            // Swallow IO exceptions to prevent crashing callers.
            // In production, you might want to log this to a fallback mechanism.
            System.Diagnostics.Debug.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }

    /// <summary>Closes and performs cleanup operations on the log file.</summary>
    /// <remarks>
    /// This method ensures proper closure of any file resources and can be extended
    /// for file rotation and cleanup operations in the future.
    /// </remarks>
    internal static void Close() {
        try {
            lock (WriteLock) {
                // Since we use File.AppendAllText(), there are no open file handles to close.
                // This method serves as a placeholder for future file rotation and cleanup logic.
                // Potential enhancements:
                // - Implement log file rotation based on size or date
                // - Archive old log files
                // - Delete logs older than a certain retention period
            }
        }
        catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"Failed to close log file: {ex.Message}");
        }
    }
}
