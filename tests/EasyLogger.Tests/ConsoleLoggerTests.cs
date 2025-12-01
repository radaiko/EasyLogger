namespace EasyLogger.Tests;

public class ConsoleLoggerTests
{
    [Fact]
    public void Log_WithDefaultMinimumLevel_ShouldLogDebugMessages()
    {
        // Arrange
        var logger = new ConsoleLogger();
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Debug("Test debug message");

        // Assert
        var logOutput = output.ToString();
        Assert.Contains("Debug", logOutput);
        Assert.Contains("Test debug message", logOutput);
    }

    [Fact]
    public void Log_WithMinimumLevelSetToWarning_ShouldNotLogInfoMessages()
    {
        // Arrange
        var logger = new ConsoleLogger { MinimumLevel = LogLevel.Warning };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Info("This should not be logged");

        // Assert
        var logOutput = output.ToString();
        Assert.Empty(logOutput);
    }

    [Fact]
    public void Log_WithMinimumLevelSetToWarning_ShouldLogWarningMessages()
    {
        // Arrange
        var logger = new ConsoleLogger { MinimumLevel = LogLevel.Warning };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Warning("This should be logged");

        // Assert
        var logOutput = output.ToString();
        Assert.Contains("Warning", logOutput);
        Assert.Contains("This should be logged", logOutput);
    }

    [Fact]
    public void Log_ShouldIncludeTimestamp()
    {
        // Arrange
        var logger = new ConsoleLogger();
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Info("Test message");

        // Assert
        var logOutput = output.ToString();
        // Check for timestamp format [YYYY-MM-DD HH:mm:ss.fff]
        Assert.Matches(@"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]", logOutput);
    }

    [Fact]
    public void Error_ShouldLogErrorLevel()
    {
        // Arrange
        var logger = new ConsoleLogger();
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Error("Error occurred");

        // Assert
        var logOutput = output.ToString();
        Assert.Contains("Error", logOutput);
        Assert.Contains("Error occurred", logOutput);
    }
}

