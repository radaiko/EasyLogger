namespace EasyLogger;

/// <summary>
/// A simple console logger that writes log messages to the console output.
/// </summary>
public class ConsoleLogger : ILogger
{
    private readonly object _lock = new();

    /// <inheritdoc />
    public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;

    /// <inheritdoc />
    public void Log(LogLevel level, string message)
    {
        if (level < MinimumLevel)
        {
            return;
        }

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logEntry = $"[{timestamp}] [{level}] {message}";

        lock (_lock)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            Console.WriteLine(logEntry);
            Console.ForegroundColor = originalColor;
        }
    }

    /// <inheritdoc />
    public void Debug(string message) => Log(LogLevel.Debug, message);

    /// <inheritdoc />
    public void Info(string message) => Log(LogLevel.Info, message);

    /// <inheritdoc />
    public void Warning(string message) => Log(LogLevel.Warning, message);

    /// <inheritdoc />
    public void Error(string message) => Log(LogLevel.Error, message);

    private static ConsoleColor GetColorForLevel(LogLevel level) => level switch
    {
        LogLevel.Debug => ConsoleColor.Gray,
        LogLevel.Info => ConsoleColor.White,
        LogLevel.Warning => ConsoleColor.Yellow,
        LogLevel.Error => ConsoleColor.Red,
        _ => ConsoleColor.White
    };
}
