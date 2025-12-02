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

    /// <summary>Indicates whether initialization has permanently failed.</summary>
    private static bool _initializationFailed;

    /// <summary>Writes a log message to a file in a thread-safe manner.</summary>
    /// <param name="logMessage">The log message to write to file.</param>
    internal static void Write(LogMessage logMessage) {
        try {
            lock (WriteLock) {
                EnsureWriterInitialized();
                if (_writer != null) {
                    _writer.WriteLine(logMessage);
                }
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
            _initializationFailed = false; // Reset failure flag to allow reinitialization
        }
    }

    /// <summary>Ensures the StreamWriter is initialized for writing.</summary>
    private static void EnsureWriterInitialized() {
        if (_initializationFailed) {
            return; // Skip initialization if a previous attempt failed permanently
        }
        if (_writer == null) {
            FileStream? fileStream = null;
            try {
                fileStream = new FileStream(
                    LogFilePath,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.Read);
                // StreamWriter takes ownership of fileStream (leaveOpen: false) and will dispose it.
                // Specify UTF-8 encoding for consistent behavior across environments.
                _writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8, leaveOpen: false);
            }
            catch {
                // If StreamWriter creation fails, dispose the FileStream
                fileStream?.Dispose();
                _initializationFailed = true; // Mark as permanently failed to avoid repeated attempts
                throw;
            }
        }
    }

    // TODO: Add methods for managing log files, such as rotation and cleanup
}
