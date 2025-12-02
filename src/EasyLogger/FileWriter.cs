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

    /// <summary>Persistent StreamWriter for efficient file writes.</summary>
    private static StreamWriter? _writer;

    /// <summary>Writes a log message to a file in a thread-safe manner.</summary>
    /// <param name="logMessage">The log message to write to file.</param>
    internal static void Write(LogMessage logMessage) {
        try {
            lock (WriteLock) {
                EnsureWriterInitialized();
                _writer!.WriteLine(logMessage);
            }
        }
        catch (Exception ex) {
            // Swallow IO exceptions to prevent crashing callers.
            // In production, you might want to log this to a fallback mechanism.
            System.Diagnostics.Debug.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }

    /// <summary>Flushes the internal buffer to the underlying file.</summary>
    internal static void Flush() {
        try {
            lock (WriteLock) {
                _writer?.Flush();
            }
        }
        catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"Failed to flush log file: {ex.Message}");
        }
    }

    /// <summary>Closes the file writer and releases all resources.</summary>
    internal static void Close() {
        lock (WriteLock) {
            if (_writer != null) {
                try {
                    _writer.Dispose();
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine($"Failed to close log file: {ex.Message}");
                }
                finally {
                    _writer = null;
                }
            }
        }
    }

    /// <summary>Ensures the StreamWriter is initialized for writing.</summary>
    private static void EnsureWriterInitialized() {
        if (_writer == null) {
            var fileStream = new FileStream(
                LogFilePath,
                FileMode.Append,
                FileAccess.Write,
                FileShare.Read);
            _writer = new StreamWriter(fileStream) { AutoFlush = true };
        }
    }

    // TODO: Add methods for managing log files, such as rotation and cleanup
}
