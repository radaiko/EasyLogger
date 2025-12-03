// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        FileWriterTests.cs                                           ║
// ║   Created:     December 2, 2025                                             ║
// ║   Description: Unit tests for the FileWriter class                          ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger.Tests;

/// <summary>Provides unit tests for the FileWriter class functionality.</summary>
/// <remarks>
/// Tests are marked as [Collection("SerialCollection")] because the FileWriter class uses static state
/// that is shared across all tests. Without this attribute, tests would interfere with
/// each other when run in parallel.
/// </remarks>
[Collection("SerialCollection")]
public sealed class FileWriterTests : IDisposable {
    /// <summary>Gets the path to the log file used by FileWriter.</summary>
    private static string LogFilePath => Path.Combine(AppContext.BaseDirectory, "logs.txt");

    /// <summary>Initializes a new instance of the FileWriterTests class and resets FileWriter state.</summary>
    public FileWriterTests() {
        // Close any existing file writer and clean up
        FileWriter.Close();
        CleanupLogFile();
    }

    /// <summary>Disposes resources and resets FileWriter state after each test.</summary>
    public void Dispose() {
        // Close any existing file writer and clean up
        FileWriter.Close();
        CleanupLogFile();
    }

    /// <summary>Deletes the log file if it exists to ensure clean test state.</summary>
    private static void CleanupLogFile() {
        if (File.Exists(LogFilePath)) {
            File.Delete(LogFilePath);
        }
    }

    /// <summary>Creates a test log message with the specified message text.</summary>
    /// <param name="message">The message text for the log message.</param>
    /// <returns>A new LogMessage instance with Info level.</returns>
    private static LogMessage CreateTestLogMessage(string message)
        => new(LogLevel.Info, message, null, "TestMethod", 1);

    #region Flush Tests

    /// <summary>Tests that Flush does not throw when the writer has not been initialized.</summary>
    [Fact]
    public void Flush_WhenWriterNotInitialized_DoesNotThrow() {
        // Arrange - writer is not initialized (no writes have been performed)

        // Act & Assert - should not throw
        FileWriter.Flush();
    }

    /// <summary>Tests that Flush works correctly after writing a message.</summary>
    [Fact]
    public void Flush_AfterWrite_DoesNotThrow() {
        // Arrange - write a message to initialize the writer
        FileWriter.Write(CreateTestLogMessage("Test message"));

        // Act & Assert - should not throw
        FileWriter.Flush();
    }

    /// <summary>Tests that Flush can be called multiple times safely.</summary>
    [Fact]
    public void Flush_CalledMultipleTimes_DoesNotThrow() {
        // Arrange - write a message to initialize the writer
        FileWriter.Write(CreateTestLogMessage("Test message"));

        // Act & Assert - calling Flush multiple times should not throw
        FileWriter.Flush();
        FileWriter.Flush();
        FileWriter.Flush();
    }

    /// <summary>Tests that Flush can be called after Close without throwing.</summary>
    [Fact]
    public void Flush_AfterClose_DoesNotThrow() {
        // Arrange - write a message, then close the writer
        FileWriter.Write(CreateTestLogMessage("Test message"));
        FileWriter.Close();

        // Act & Assert - Flush after Close should not throw
        FileWriter.Flush();
    }

    /// <summary>Tests that Flush ensures data is written to disk.</summary>
    [Fact]
    public void Flush_EnsuresDataWrittenToFile() {
        // Arrange
        var message = "Message to be flushed";
        FileWriter.Write(CreateTestLogMessage(message));

        // Act
        FileWriter.Flush();

        // Assert - file should exist and contain the message
        Assert.True(File.Exists(LogFilePath), "Log file should exist after write and flush");
        var fileContent = File.ReadAllText(LogFilePath);
        Assert.Contains(message, fileContent);
    }

    /// <summary>Tests that Flush works correctly in a sequence of write-flush operations.</summary>
    [Fact]
    public void Flush_InWriteFlushSequence_WorksCorrectly() {
        // Arrange & Act - perform multiple write-flush sequences
        for (int i = 1; i <= 3; i++) {
            FileWriter.Write(CreateTestLogMessage($"Message {i}"));
            FileWriter.Flush();
        }

        // Assert - all messages should be in the file
        Assert.True(File.Exists(LogFilePath), "Log file should exist");
        var fileContent = File.ReadAllText(LogFilePath);
        Assert.Contains("Message 1", fileContent);
        Assert.Contains("Message 2", fileContent);
        Assert.Contains("Message 3", fileContent);
    }

    #endregion

    #region Close Tests

    /// <summary>Tests that Close can be called when writer is not initialized.</summary>
    [Fact]
    public void Close_WhenWriterNotInitialized_DoesNotThrow() {
        // Arrange - writer is not initialized

        // Act & Assert - should not throw
        FileWriter.Close();
    }

    /// <summary>Tests that Close can be called multiple times safely.</summary>
    [Fact]
    public void Close_CalledMultipleTimes_DoesNotThrow() {
        // Arrange - write a message to initialize the writer
        FileWriter.Write(CreateTestLogMessage("Test message"));

        // Act & Assert - calling Close multiple times should not throw
        FileWriter.Close();
        FileWriter.Close();
        FileWriter.Close();
    }

    #endregion
}
