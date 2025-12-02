// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        ConsoleWriter.cs                                             ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Writes colored log messages to the console output            ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;

/// <summary>Writes colored log messages to the console with appropriate formatting.</summary>
internal static class ConsoleWriter {
    /// <summary>Lock object to synchronize console color operations across threads.</summary>
    private static readonly object _consoleLock = new();

    /// <summary>Writes a log message to the console with color coding based on its severity level.</summary>
    /// <param name="logMessage">The log message to write to the console.</param>
    /// <remarks>
    /// This method is thread-safe. Console color operations are synchronized to prevent
    /// color interleaving when multiple threads log messages concurrently.
    /// </remarks>
    internal static void Write(LogMessage logMessage) {
        lock (_consoleLock) {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = logMessage.Level switch {
                LogLevel.Info => ConsoleColor.Green,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Debug => ConsoleColor.Cyan,
                _ => originalColor
            };

            try {
                Console.WriteLine(logMessage);
                if (logMessage.Exception != null)
                    Console.WriteLine($"Exception: {logMessage.Exception}");
            }
            finally {
                Console.ForegroundColor = originalColor;
            }
        }
    }
}
