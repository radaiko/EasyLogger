namespace EasyLogger;

/// <summary>
/// Interface for logging messages at different severity levels.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Gets or sets the minimum log level. Messages below this level will be ignored.
    /// </summary>
    LogLevel MinimumLevel { get; set; }

    /// <summary>
    /// Logs a message at the specified level.
    /// </summary>
    /// <param name="level">The severity level of the log message.</param>
    /// <param name="message">The message to log.</param>
    void Log(LogLevel level, string message);

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Debug(string message);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Info(string message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Warning(string message);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Error(string message);
}
