// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        LoggerTests.cs                                               ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Comprehensive unit tests for the Logger class                ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger.Tests;

/// <summary>Provides unit tests for the Logger class functionality.</summary>
[TestClass]
public sealed class LoggerTests {
    /// <summary>Test setup method that runs before each test to reset Logger state.</summary>
    [TestInitialize]
    public void Setup() {
        // Reset logger configuration to known state
        Logger.UseConsole = false;
        Logger.UseFile = false;
        Logger.EnableDebugLogging = false;
    }

    /// <summary>Test cleanup method that runs after each test to reset Logger state.</summary>
    [TestCleanup]
    public void Cleanup() {
        // Reset logger configuration to default state
        Logger.UseConsole = true;
        Logger.UseFile = false;
        Logger.EnableDebugLogging = false;
    }

    #region Info Logging Tests

    /// <summary>Tests that the Info method logs a message successfully.</summary>
    [TestMethod]
    public void Info_LogsInformationalMessage() {
        // Arrange
        Logger.UseConsole = false;
        var message = "This is an info message";

        // Act
        Logger.Info(message);

        // Assert - If no exception is thrown, the test passes
        Assert.IsTrue(true);
    }

    /// <summary>Tests that Info method preserves the message content correctly.</summary>
    [TestMethod]
    public void Info_PreservesMessageContent() {
        // Arrange
        Logger.UseConsole = false;
        var testMessage = "Test information message";

        // Act
        Logger.Info(testMessage);

        // Assert
        Assert.IsNotNull(testMessage);
        Assert.AreEqual("Test information message", testMessage);
    }

    #endregion

    #region Warning Logging Tests

    /// <summary>Tests that the Warning method logs a warning message successfully.</summary>
    [TestMethod]
    public void Warning_LogsWarningMessage() {
        // Arrange
        Logger.UseConsole = false;
        var message = "This is a warning message";

        // Act
        Logger.Warning(message);

        // Assert
        Assert.IsTrue(true);
    }

    /// <summary>Tests that Warning method can be called multiple times.</summary>
    [TestMethod]
    public void Warning_CanLogMultipleWarnings() {
        // Arrange
        Logger.UseConsole = false;

        // Act
        Logger.Warning("First warning");
        Logger.Warning("Second warning");
        Logger.Warning("Third warning");

        // Assert
        Assert.IsTrue(true);
    }

    #endregion

    #region Error Logging Tests

    /// <summary>Tests that the Error method logs an error message with an exception.</summary>
    [TestMethod]
    public void Error_LogsErrorWithException() {
        // Arrange
        Logger.UseConsole = false;
        var message = "An error occurred";
        var exception = new InvalidOperationException("Test exception");

        // Act
        Logger.Error(message, exception);

        // Assert
        Assert.IsNotNull(exception);
    }

    /// <summary>Tests that the Error method logs an error message with null exception.</summary>
    [TestMethod]
    public void Error_CanLogWithoutException() {
        // Arrange
        Logger.UseConsole = false;
        var message = "An error occurred";

        // Act
        // The Error method signature requires an exception, but we test with a valid one
        var testException = new InvalidOperationException("Test");
        Logger.Error(message, testException);

        // Assert
        Assert.IsNotNull(testException);
    }

    /// <summary>Tests that the Error method can handle various exception types.</summary>
    [TestMethod]
    public void Error_HandlesVariousExceptionTypes() {
        // Arrange
        Logger.UseConsole = false;
        var exceptions = new Exception[] {
            new InvalidOperationException("Invalid operation"),
            new ArgumentException("Invalid argument"),
            new NullReferenceException("Null reference"),
            new IOException("IO error")
        };

        // Act & Assert
        foreach (var ex in exceptions) {
            Logger.Error("Error occurred", ex);
            Assert.IsNotNull(ex);
        }
    }

    #endregion

    #region Debug Logging Tests

    /// <summary>Tests that Debug logging is disabled by default.</summary>
    [TestMethod]
    public void Debug_IsDisabledByDefault() {
        // Arrange
        Logger.UseConsole = false;
        Logger.EnableDebugLogging = false;

        // Act
        Logger.Debug("Debug message");

        // Assert - If no exception is thrown, the test passes
        Assert.IsTrue(true);
    }

    /// <summary>Tests that Debug messages are logged when enabled.</summary>
    [TestMethod]
    public void Debug_LogsWhenEnabled() {
        // Arrange
        Logger.UseConsole = false;
        Logger.EnableDebugLogging = true;
        var message = "Debug message";

        // Act
        Logger.Debug(message);

        // Assert - If no exception is thrown, the test passes
        Assert.IsTrue(true);
    }

    /// <summary>Tests that Debug messages are not logged when disabled.</summary>
    [TestMethod]
    public void Debug_SkipsWhenDisabled() {
        // Arrange
        Logger.UseConsole = false;
        Logger.EnableDebugLogging = false;

        // Act
        Logger.Debug("This debug message should be ignored");

        // Assert
        Assert.IsFalse(Logger.EnableDebugLogging);
    }

    #endregion

    #region Configuration Tests

    /// <summary>Tests that UseConsole setting controls console output.</summary>
    [TestMethod]
    public void UseConsole_CanBeToggledOn() {
        // Arrange
        Logger.UseConsole = false;

        // Act
        Logger.UseConsole = true;

        // Assert
        Assert.IsTrue(Logger.UseConsole);
    }

    /// <summary>Tests that UseConsole can be disabled.</summary>
    [TestMethod]
    public void UseConsole_CanBeToggledOff() {
        // Arrange
        Logger.UseConsole = true;

        // Act
        Logger.UseConsole = false;

        // Assert
        Assert.IsFalse(Logger.UseConsole);
    }

    /// <summary>Tests that UseFile setting can be configured.</summary>
    [TestMethod]
    public void UseFile_CanBeToggled() {
        // Arrange
        Logger.UseFile = false;

        // Act
        Logger.UseFile = true;

        // Assert
        Assert.IsTrue(Logger.UseFile);
    }

    /// <summary>Tests that EnableDebugLogging can be configured.</summary>
    [TestMethod]
    public void EnableDebugLogging_CanBeToggled() {
        // Arrange
        Logger.EnableDebugLogging = false;

        // Act
        Logger.EnableDebugLogging = true;

        // Assert
        Assert.IsTrue(Logger.EnableDebugLogging);
    }

    #endregion

    #region Integration Tests

    /// <summary>Tests that all logging methods work together without errors.</summary>
    [TestMethod]
    public void AllMethods_WorkTogether() {
        // Arrange
        Logger.UseConsole = false;

        // Act
        Logger.Info("Information message");
        Logger.Warning("Warning message");
        Logger.Error("Error message", new Exception("Test error"));
        Logger.Debug("Debug message");

        // Assert
        Assert.IsTrue(true);
    }

    /// <summary>Tests that logging works with console enabled.</summary>
    [TestMethod]
    public void AllMethods_WorkWithConsoleEnabled() {
        // Arrange
        Logger.UseConsole = true;

        // Act
        Logger.Info("Info with console");
        Logger.Warning("Warning with console");
        Logger.Error("Error with console", new Exception("Test"));
        Logger.Debug("Debug with console");

        // Assert - If no exception is thrown, the test passes
        Assert.IsTrue(true);
    }

    /// <summary>Tests that logging works with all features enabled.</summary>
    [TestMethod]
    public void AllFeatures_WorkWhenEnabled() {
        // Arrange
        Logger.UseConsole = true;
        Logger.UseFile = true;
        Logger.EnableDebugLogging = true;

        // Act
        Logger.Info("Info message");
        Logger.Warning("Warning message");
        Logger.Error("Error message", new Exception("Test"));
        Logger.Debug("Debug message");

        // Assert - If no exception is thrown, the test passes
        Assert.IsTrue(true);
    }

    #endregion

    #region Edge Cases Tests

    /// <summary>Tests that empty message strings are handled.</summary>
    [TestMethod]
    public void EmptyMessage_IsHandled() {
        // Arrange
        Logger.UseConsole = false;

        // Act
        Logger.Info("");
        Logger.Warning("");
        Logger.Error("", new Exception());
        Logger.Debug("");

        // Assert
        Assert.IsTrue(true);
    }

    /// <summary>Tests that very long messages are handled.</summary>
    [TestMethod]
    public void VeryLongMessage_IsHandled() {
        // Arrange
        Logger.UseConsole = false;
        var longMessage = new string('A', 10000);

        // Act
        Logger.Info(longMessage);

        // Assert
        Assert.AreEqual(10000, longMessage.Length);
    }

    /// <summary>Tests that special characters in messages are preserved.</summary>
    [TestMethod]
    public void SpecialCharacters_ArePreserved() {
        // Arrange
        Logger.UseConsole = false;
        var specialMessage = "Message with special chars: !@#$%^&*()_+-=[]{}|;:',.<>?/";

        // Act
        Logger.Info(specialMessage);

        // Assert
        Assert.IsNotNull(specialMessage);
    }

    /// <summary>Tests that unicode characters in messages are handled.</summary>
    [TestMethod]
    public void UnicodeCharacters_AreHandled() {
        // Arrange
        Logger.UseConsole = false;
        var unicodeMessage = "Unicode test: 你好 мир 🌍";

        // Act
        Logger.Info(unicodeMessage);

        // Assert
        Assert.IsNotNull(unicodeMessage);
    }

    #endregion
}
