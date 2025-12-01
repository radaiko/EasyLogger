// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        LogMessage.cs                                                ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Represents a single log message with timestamp and metadata  ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;

/// <summary>Represents a single log message with timestamp and metadata about its origin.</summary>
internal sealed class LogMessage {
    /// <summary>Gets the UTC timestamp when the log message was created.</summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>Gets the severity level of the log message.</summary>
    public LogLevel Level { get; init; }

    /// <summary>Gets the content of the log message.</summary>
    public string Message { get; init; }

    /// <summary>Gets the exception associated with the log message, if any.</summary>
    public Exception? Exception { get; init; }

    /// <summary>Gets the name of the caller that generated the log message.</summary>
    public string Caller { get; init; }

    /// <summary>Gets the source code line number where the log message was generated.</summary>
    public int LineNumber { get; init; }

    /// <summary>Initializes a new instance of the <see cref="LogMessage"/> class.</summary>
    /// <param name="level">The severity level of the log message.</param>
    /// <param name="message">The content of the log message.</param>
    /// <param name="exception">The exception associated with the log message, if any.</param>
    /// <param name="caller">The name of the caller that generated the log message.</param>
    /// <param name="lineNumber">The source code line number where the log message was generated.</param>
    public LogMessage(LogLevel level, string message, Exception? exception, string caller, int lineNumber) {
        Level = level;
        Message = message;
        Exception = exception;
        Caller = caller;
        LineNumber = lineNumber;

    }
}
