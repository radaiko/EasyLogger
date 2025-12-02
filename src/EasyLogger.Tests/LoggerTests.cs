// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        LoggerTests.cs                                               ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Comprehensive unit tests for the Logger class                ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger.Tests;

/// <summary>Provides unit tests for the Logger class functionality.</summary>
/// <remarks>
/// Tests are marked as [DoNotParallelize] because the Logger class uses static state
/// that is shared across all tests. Without this attribute, tests would interfere with
/// each other when run in parallel, even with Reset() in Setup/Cleanup.
/// </remarks>
[TestClass]
[DoNotParallelize]
public sealed class LoggerTests {
    /// <summary>Test setup method that runs before each test to reset Logger state.</summary>
    [TestInitialize]
    public void Setup() {
        // Clear all logger state before running each test to ensure test isolation
        Logger.Reset();
        Logger.UseConsole = false;
    }

    /// <summary>Test cleanup method that runs after each test to reset Logger state.</summary>
    [TestCleanup]
    public void Cleanup() {
        // Clear all logger state after each test for clean isolation
        Logger.Reset();
    }

    #region Info Logging Tests

    /// <summary>Tests that the Info method stores a message correctly.</summary>
    [TestMethod]
    public void Info_StoresInformationalMessage() {
        // Arrange
        var message = "This is an info message";

        // Act
        Logger.Info(message);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
        Assert.AreEqual(message, messages[0].Message);
    }

    /// <summary>Tests that Info method preserves message content exactly.</summary>
    [TestMethod]
    public void Info_PreservesMessageContent() {
        // Arrange
        var testMessage = "Test information message";

        // Act
        Logger.Info(testMessage);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(testMessage, messages[0].Message);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
        Assert.IsNull(messages[0].Exception);
    }

    #endregion

    #region Warning Logging Tests

    /// <summary>Tests that the Warning method stores a warning message correctly.</summary>
    [TestMethod]
    public void Warning_StoresWarningMessage() {
        // Arrange
        var message = "This is a warning message";

        // Act
        Logger.Warning(message);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(LogLevel.Warning, messages[0].Level);
        Assert.AreEqual(message, messages[0].Message);
    }

    /// <summary>Tests that Warning method can be called multiple times and all messages are stored.</summary>
    [TestMethod]
    public void Warning_StoresMultipleWarnings() {
        // Arrange
        var warnings = new[] { "First warning", "Second warning", "Third warning" };

        // Act
        foreach (var warning in warnings) {
            Logger.Warning(warning);
        }

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(3, messages);
        for (int i = 0; i < warnings.Length; i++) {
            Assert.AreEqual(LogLevel.Warning, messages[i].Level);
            Assert.AreEqual(warnings[i], messages[i].Message);
        }
    }

    #endregion

    #region Error Logging Tests

    /// <summary>Tests that the Error method stores an error message with exception.</summary>
    [TestMethod]
    public void Error_StoresErrorWithException() {
        // Arrange
        var message = "An error occurred";
        var exception = new InvalidOperationException("Test exception");

        // Act
        Logger.Error(message, exception);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(LogLevel.Error, messages[0].Level);
        Assert.AreEqual(message, messages[0].Message);
        Assert.IsNotNull(messages[0].Exception);
        Assert.IsInstanceOfType(messages[0].Exception, typeof(InvalidOperationException));
        Assert.AreEqual("Test exception", messages[0].Exception?.Message);
    }

    /// <summary>Tests that the Error method preserves exception information.</summary>
    [TestMethod]
    public void Error_PreservesExceptionData() {
        // Arrange
        var message = "An error occurred";
        var exceptionMessage = "Important error details";
        var testException = new InvalidOperationException(exceptionMessage);

        // Act
        Logger.Error(message, testException);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(LogLevel.Error, messages[0].Level);
        Assert.AreEqual(message, messages[0].Message);
        Assert.AreEqual(exceptionMessage, messages[0].Exception?.Message);
    }

    /// <summary>Tests that the Error method can handle various exception types.</summary>
    [TestMethod]
    public void Error_HandlesVariousExceptionTypes() {
        // Arrange
        var exceptionTypes = new Exception[] {
            new InvalidOperationException("Invalid operation"),
            new ArgumentException("Invalid argument"),
            new NullReferenceException("Null reference"),
            new IOException("IO error")
        };

        // Act
        foreach (var ex in exceptionTypes) {
            Logger.Error("Error occurred", ex);
        }

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(4, messages);
        Assert.IsInstanceOfType(messages[0].Exception, typeof(InvalidOperationException));
        Assert.IsInstanceOfType(messages[1].Exception, typeof(ArgumentException));
        Assert.IsInstanceOfType(messages[2].Exception, typeof(NullReferenceException));
        Assert.IsInstanceOfType(messages[3].Exception, typeof(IOException));
    }

    #endregion

    #region Debug Logging Tests

    /// <summary>Tests that Debug messages are NOT stored when debug logging is disabled.</summary>
    [TestMethod]
    public void Debug_NotStoredWhenDisabled() {
        // Arrange
        Logger.EnableDebugLogging = false;

        // Act
        Logger.Debug("This debug message should be ignored");

        // Assert
        var messages = Logger.GetMessages();
        Assert.IsEmpty(messages, "Debug message should not be stored when EnableDebugLogging is false");
    }

    /// <summary>Tests that Debug messages ARE stored when debug logging is enabled.</summary>
    [TestMethod]
    public void Debug_StoredWhenEnabled() {
        // Arrange
        Logger.EnableDebugLogging = true;
        var message = "Debug message";

        // Act
        Logger.Debug(message);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(LogLevel.Debug, messages[0].Level);
        Assert.AreEqual(message, messages[0].Message);
    }

    /// <summary>Tests that non-debug messages are stored even when debug logging is disabled.</summary>
    [TestMethod]
    public void NonDebugMessages_StoredWhenDebugDisabled() {
        // Arrange
        Logger.EnableDebugLogging = false;

        // Act
        Logger.Info("Info message");
        Logger.Warning("Warning message");
        Logger.Error("Error message", new Exception());
        Logger.Debug("Debug message");

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(3, messages);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
        Assert.AreEqual(LogLevel.Warning, messages[1].Level);
        Assert.AreEqual(LogLevel.Error, messages[2].Level);
    }

    #endregion

    #region Configuration Tests

    /// <summary>Tests that UseConsole setting can be toggled on.</summary>
    [TestMethod]
    public void UseConsole_CanBeToggledOn() {
        // Arrange
        Logger.UseConsole = false;

        // Act
        Logger.UseConsole = true;

        // Assert
        Assert.IsTrue(Logger.UseConsole);
    }

    /// <summary>Tests that UseConsole can be toggled off.</summary>
    [TestMethod]
    public void UseConsole_CanBeToggledOff() {
        // Arrange
        Logger.UseConsole = true;

        // Act
        Logger.UseConsole = false;

        // Assert
        Assert.IsFalse(Logger.UseConsole);
    }

    /// <summary>Tests that UseFile setting can be toggled.</summary>
    [TestMethod]
    public void UseFile_CanBeToggled() {
        // Arrange
        Logger.UseFile = false;

        // Act
        Logger.UseFile = true;

        // Assert
        Assert.IsTrue(Logger.UseFile);
    }

    /// <summary>Tests that EnableDebugLogging can be toggled.</summary>
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

    #region Console Output Tests

    /// <summary>Tests that messages are stored regardless of console output setting.</summary>
    [TestMethod]
    public void Console_MessagesStoredWhenEnabled() {
        // Arrange
        Logger.UseConsole = true;
        var message = "Test console output";

        // Act
        Logger.Info(message);

        // Assert - verify the message is stored
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(message, messages[0].Message);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
    }

    /// <summary>Tests that messages are stored when UseConsole is disabled.</summary>
    [TestMethod]
    public void Console_MessagesStoredWhenDisabled() {
        // Arrange
        var message = "Test console output disabled";

        // Act
        Logger.Info(message);

        // Assert - verify message is stored even though console output is disabled
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(message, messages[0].Message);
    }

    /// <summary>Tests that all log levels are stored correctly.</summary>
    [TestMethod]
    public void Console_AllLevelsStored() {
        // Arrange
        Logger.UseConsole = true;

        // Act
        Logger.Info("Info test");
        Logger.Warning("Warning test");
        Logger.Error("Error test", new Exception());

        // Assert - verify all messages are stored
        var messages = Logger.GetMessages();
        Assert.HasCount(3, messages);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
        Assert.AreEqual(LogLevel.Warning, messages[1].Level);
        Assert.AreEqual(LogLevel.Error, messages[2].Level);
    }

    /// <summary>Tests that exceptions are stored along with messages.</summary>
    [TestMethod]
    public void Console_ExceptionsAreStored() {
        // Arrange
        Logger.UseConsole = true;
        var exceptionMessage = "Test exception message";

        // Act
        Logger.Error("Error message", new InvalidOperationException(exceptionMessage));

        // Assert - verify exception is stored with the message
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.IsNotNull(messages[0].Exception);
        Assert.AreEqual(exceptionMessage, messages[0].Exception?.Message);
    }

    #endregion

    #region Edge Cases Tests

    /// <summary>Tests that empty message strings are stored correctly.</summary>
    [TestMethod]
    public void EmptyMessage_IsStoredCorrectly() {
        // Arrange & Act
        Logger.Info("");

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(string.Empty, messages[0].Message);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
    }

    /// <summary>Tests that very long messages are stored and preserved exactly.</summary>
    [TestMethod]
    public void VeryLongMessage_IsPreservedExactly() {
        // Arrange
        var longMessage = new string('A', 10000);

        // Act
        Logger.Info(longMessage);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(longMessage, messages[0].Message);
        Assert.AreEqual(10000, messages[0].Message.Length);
    }

    /// <summary>Tests that special characters in messages are preserved exactly.</summary>
    [TestMethod]
    public void SpecialCharacters_ArePreservedExactly() {
        // Arrange
        var specialMessage = "Message with special chars: !@#$%^&*()_+-=[]{}|;:',.<>?/";

        // Act
        Logger.Info(specialMessage);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(specialMessage, messages[0].Message);
    }

    /// <summary>Tests that unicode characters in messages are preserved exactly.</summary>
    [TestMethod]
    public void UnicodeCharacters_ArePreservedExactly() {
        // Arrange
        var unicodeMessage = "Unicode test: 你好 мир 🌍";

        // Act
        Logger.Info(unicodeMessage);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(unicodeMessage, messages[0].Message);
    }

    /// <summary>Tests that newline characters in messages are preserved.</summary>
    [TestMethod]
    public void NewlineCharacters_ArePreserved() {
        // Arrange
        var multilineMessage = "Line 1\nLine 2\nLine 3";

        // Act
        Logger.Info(multilineMessage);

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(1, messages);
        Assert.AreEqual(multilineMessage, messages[0].Message);
    }

    #endregion

    #region Integration Tests

    /// <summary>Tests that all logging methods work together and store all messages.</summary>
    [TestMethod]
    public void AllMethods_StoreCorrectly() {
        // Arrange & Act
        Logger.Info("Information message");
        Logger.Warning("Warning message");
        Logger.Error("Error message", new Exception("Test error"));
        Logger.EnableDebugLogging = true;
        Logger.Debug("Debug message");

        // Assert
        var messages = Logger.GetMessages();
        Assert.HasCount(4, messages);
        Assert.AreEqual(LogLevel.Info, messages[0].Level);
        Assert.AreEqual(LogLevel.Warning, messages[1].Level);
        Assert.AreEqual(LogLevel.Error, messages[2].Level);
        Assert.AreEqual(LogLevel.Debug, messages[3].Level);
    }

    /// <summary>Tests that Reset clears all stored messages.</summary>
    [TestMethod]
    public void Reset_ClearsAllStoredMessages() {
        // Arrange
        Logger.Info("Message 1");
        Logger.Warning("Message 2");
        Logger.Error("Message 3", new Exception());

        // Verify messages were stored
        Assert.HasCount(3, Logger.GetMessages());

        // Act
        Logger.Reset();

        // Assert
        Assert.IsEmpty(Logger.GetMessages());
        Assert.IsTrue(Logger.UseConsole);
        Assert.IsFalse(Logger.UseFile);
        Assert.IsFalse(Logger.EnableDebugLogging);
    }

    /// <summary>Tests that Reset restores default configuration.</summary>
    [TestMethod]
    public void Reset_RestoresDefaultConfiguration() {
        // Arrange
        Logger.UseConsole = false;
        Logger.UseFile = true;
        Logger.EnableDebugLogging = true;

        // Act
        Logger.Reset();

        // Assert
        Assert.IsTrue(Logger.UseConsole, "UseConsole should be true after reset");
        Assert.IsFalse(Logger.UseFile, "UseFile should be false after reset");
        Assert.IsFalse(Logger.EnableDebugLogging, "EnableDebugLogging should be false after reset");
    }

    #endregion

    #region RemoveOlderThan Tests

    /// <summary>Tests that RemoveOlderThan removes older messages from Logger storage.</summary>
    [TestMethod]
    public void RemoveOlderThan_RemovesOlderMessages() {
        // Arrange - log some messages
        Logger.Info("Message 1");
        Logger.Info("Message 2");
        Assert.HasCount(2, Logger.GetMessages());

        // Act - remove with a future cutoff (should remove all)
        Logger.RemoveOlderThan(DateTime.UtcNow.AddSeconds(1));

        // Assert
        Assert.IsEmpty(Logger.GetMessages());
    }

    /// <summary>Tests that RemoveOlderThan retains messages newer than cutoff.</summary>
    [TestMethod]
    public void RemoveOlderThan_RetainsNewerMessages() {
        // Arrange - log some messages
        Logger.Info("Message 1");
        Logger.Info("Message 2");
        Assert.HasCount(2, Logger.GetMessages());

        // Act - remove with a distant past cutoff (should retain all)
        Logger.RemoveOlderThan(DateTime.UtcNow.AddYears(-1));

        // Assert - all messages should be retained
        Assert.HasCount(2, Logger.GetMessages());
    }

    /// <summary>Tests that RemoveOlderThan on empty storage does not throw.</summary>
    [TestMethod]
    public void RemoveOlderThan_OnEmptyStorage_DoesNotThrow() {
        // Arrange - storage is empty after Reset

        // Act & Assert - should not throw
        Logger.RemoveOlderThan(DateTime.UtcNow);
        Assert.IsEmpty(Logger.GetMessages());
    }

    #endregion
}
