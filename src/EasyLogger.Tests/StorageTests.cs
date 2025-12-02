// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        StorageTests.cs                                              ║
// ║   Created:     December 2, 2025                                             ║
// ║   Description: Unit tests for the Storage class log message management      ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger.Tests;

/// <summary>Provides unit tests for the Storage class functionality.</summary>
[TestClass]
public sealed class StorageTests {
    private Storage _storage = null!;

    /// <summary>Test setup method that runs before each test to create a fresh Storage instance.</summary>
    [TestInitialize]
    public void Setup() {
        _storage = new Storage();
    }

    #region Add Tests

    /// <summary>Tests that Add stores a message correctly.</summary>
    [TestMethod]
    public void Add_StoresMessage() {
        // Arrange
        var message = CreateLogMessage("Test message");

        // Act
        _storage.Add(message);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(1, messages);
        Assert.AreEqual("Test message", messages[0].Message);
    }

    /// <summary>Tests that Add stores multiple messages in order.</summary>
    [TestMethod]
    public void Add_StoresMultipleMessagesInOrder() {
        // Arrange
        var message1 = CreateLogMessage("First");
        var message2 = CreateLogMessage("Second");
        var message3 = CreateLogMessage("Third");

        // Act
        _storage.Add(message1);
        _storage.Add(message2);
        _storage.Add(message3);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(3, messages);
        Assert.AreEqual("First", messages[0].Message);
        Assert.AreEqual("Second", messages[1].Message);
        Assert.AreEqual("Third", messages[2].Message);
    }

    #endregion

    #region Clear Tests

    /// <summary>Tests that Clear removes all messages.</summary>
    [TestMethod]
    public void Clear_RemovesAllMessages() {
        // Arrange
        _storage.Add(CreateLogMessage("Message 1"));
        _storage.Add(CreateLogMessage("Message 2"));
        _storage.Add(CreateLogMessage("Message 3"));
        Assert.HasCount(3, _storage.GetAll());

        // Act
        _storage.Clear();

        // Assert
        Assert.IsEmpty(_storage.GetAll());
    }

    /// <summary>Tests that Clear on empty storage does not throw.</summary>
    [TestMethod]
    public void Clear_OnEmptyStorage_DoesNotThrow() {
        // Arrange - storage is already empty

        // Act & Assert - should not throw
        _storage.Clear();
        Assert.IsEmpty(_storage.GetAll());
    }

    #endregion

    #region RemoveOlderThan Tests

    /// <summary>Tests that RemoveOlderThan removes messages older than the cutoff.</summary>
    [TestMethod]
    public void RemoveOlderThan_RemovesOlderMessages() {
        // Arrange
        var oldTime = DateTime.UtcNow.AddHours(-2);
        var newTime = DateTime.UtcNow;
        var cutoff = DateTime.UtcNow.AddHours(-1);

        var oldMessage = CreateLogMessageWithTimestamp("Old message", oldTime);
        var newMessage = CreateLogMessageWithTimestamp("New message", newTime);

        _storage.Add(oldMessage);
        _storage.Add(newMessage);

        // Act
        _storage.RemoveOlderThan(cutoff);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(1, messages);
        Assert.AreEqual("New message", messages[0].Message);
    }

    /// <summary>Tests that RemoveOlderThan retains messages newer than the cutoff.</summary>
    [TestMethod]
    public void RemoveOlderThan_RetainsNewerMessages() {
        // Arrange
        var cutoff = DateTime.UtcNow.AddHours(-1);
        var newerTime = DateTime.UtcNow;

        var newerMessage1 = CreateLogMessageWithTimestamp("Newer 1", newerTime);
        var newerMessage2 = CreateLogMessageWithTimestamp("Newer 2", newerTime.AddMinutes(1));

        _storage.Add(newerMessage1);
        _storage.Add(newerMessage2);

        // Act
        _storage.RemoveOlderThan(cutoff);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(2, messages);
    }

    /// <summary>Tests that RemoveOlderThan removes all messages when cutoff is in the future.</summary>
    [TestMethod]
    public void RemoveOlderThan_WithFutureCutoff_RemovesAllMessages() {
        // Arrange
        var now = DateTime.UtcNow;
        var futureCutoff = now.AddHours(1);

        _storage.Add(CreateLogMessageWithTimestamp("Message 1", now));
        _storage.Add(CreateLogMessageWithTimestamp("Message 2", now.AddMinutes(5)));
        _storage.Add(CreateLogMessageWithTimestamp("Message 3", now.AddMinutes(10)));

        // Act
        _storage.RemoveOlderThan(futureCutoff);

        // Assert
        Assert.IsEmpty(_storage.GetAll());
    }

    /// <summary>Tests that RemoveOlderThan retains all messages when cutoff is in the past.</summary>
    [TestMethod]
    public void RemoveOlderThan_WithDistantPastCutoff_RetainsAllMessages() {
        // Arrange
        var now = DateTime.UtcNow;
        var distantPastCutoff = now.AddYears(-1);

        _storage.Add(CreateLogMessageWithTimestamp("Message 1", now));
        _storage.Add(CreateLogMessageWithTimestamp("Message 2", now.AddMinutes(5)));

        // Act
        _storage.RemoveOlderThan(distantPastCutoff);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(2, messages);
    }

    /// <summary>Tests that RemoveOlderThan handles exact boundary correctly (message at cutoff is retained).</summary>
    [TestMethod]
    public void RemoveOlderThan_MessageAtExactCutoff_IsRetained() {
        // Arrange
        var cutoff = DateTime.UtcNow;
        var messageAtCutoff = CreateLogMessageWithTimestamp("At cutoff", cutoff);

        _storage.Add(messageAtCutoff);

        // Act
        _storage.RemoveOlderThan(cutoff);

        // Assert - message at exact cutoff time should be retained (not older than cutoff)
        var messages = _storage.GetAll();
        Assert.HasCount(1, messages);
        Assert.AreEqual("At cutoff", messages[0].Message);
    }

    /// <summary>Tests that RemoveOlderThan removes message just before cutoff.</summary>
    [TestMethod]
    public void RemoveOlderThan_MessageJustBeforeCutoff_IsRemoved() {
        // Arrange
        var cutoff = DateTime.UtcNow;
        var justBeforeCutoff = cutoff.AddMilliseconds(-1);
        var messageBeforeCutoff = CreateLogMessageWithTimestamp("Just before cutoff", justBeforeCutoff);

        _storage.Add(messageBeforeCutoff);

        // Act
        _storage.RemoveOlderThan(cutoff);

        // Assert - message just before cutoff should be removed
        Assert.IsEmpty(_storage.GetAll());
    }

    /// <summary>Tests that RemoveOlderThan on empty storage does not throw.</summary>
    [TestMethod]
    public void RemoveOlderThan_OnEmptyStorage_DoesNotThrow() {
        // Arrange - storage is already empty

        // Act & Assert - should not throw
        _storage.RemoveOlderThan(DateTime.UtcNow);
        Assert.IsEmpty(_storage.GetAll());
    }

    /// <summary>Tests that RemoveOlderThan correctly handles mixed old and new messages.</summary>
    [TestMethod]
    public void RemoveOlderThan_WithMixedMessages_RemovesOnlyOlderOnes() {
        // Arrange
        var now = DateTime.UtcNow;
        var cutoff = now.AddMinutes(-30);

        // Add messages with varying timestamps
        _storage.Add(CreateLogMessageWithTimestamp("Very old", now.AddHours(-2)));
        _storage.Add(CreateLogMessageWithTimestamp("Old", now.AddMinutes(-45)));
        _storage.Add(CreateLogMessageWithTimestamp("New 1", now.AddMinutes(-15)));
        _storage.Add(CreateLogMessageWithTimestamp("New 2", now.AddMinutes(-5)));
        _storage.Add(CreateLogMessageWithTimestamp("Current", now));

        // Act
        _storage.RemoveOlderThan(cutoff);

        // Assert
        var messages = _storage.GetAll();
        Assert.HasCount(3, messages);
        Assert.AreEqual("New 1", messages[0].Message);
        Assert.AreEqual("New 2", messages[1].Message);
        Assert.AreEqual("Current", messages[2].Message);
    }

    /// <summary>Tests that RemoveOlderThan can be called multiple times with different cutoffs.</summary>
    [TestMethod]
    public void RemoveOlderThan_CalledMultipleTimes_WorksCorrectly() {
        // Arrange
        var now = DateTime.UtcNow;
        _storage.Add(CreateLogMessageWithTimestamp("Message 1", now.AddHours(-3)));
        _storage.Add(CreateLogMessageWithTimestamp("Message 2", now.AddHours(-2)));
        _storage.Add(CreateLogMessageWithTimestamp("Message 3", now.AddHours(-1)));
        _storage.Add(CreateLogMessageWithTimestamp("Message 4", now));

        // Act - first call removes messages older than 2.5 hours ago
        _storage.RemoveOlderThan(now.AddHours(-2.5));
        var afterFirstCall = _storage.GetAll();

        // Assert after first call
        Assert.HasCount(3, afterFirstCall);

        // Act - second call removes messages older than 1.5 hours ago
        _storage.RemoveOlderThan(now.AddHours(-1.5));
        var afterSecondCall = _storage.GetAll();

        // Assert after second call
        Assert.HasCount(2, afterSecondCall);
        Assert.AreEqual("Message 3", afterSecondCall[0].Message);
        Assert.AreEqual("Message 4", afterSecondCall[1].Message);
    }

    #endregion

    #region GetAll Tests

    /// <summary>Tests that GetAll returns empty list for empty storage.</summary>
    [TestMethod]
    public void GetAll_EmptyStorage_ReturnsEmptyList() {
        // Act
        var messages = _storage.GetAll();

        // Assert
        Assert.IsEmpty(messages);
    }

    /// <summary>Tests that GetAll returns read-only list.</summary>
    [TestMethod]
    public void GetAll_ReturnsReadOnlyList() {
        // Arrange
        _storage.Add(CreateLogMessage("Test"));

        // Act
        var messages = _storage.GetAll();

        // Assert
        Assert.IsInstanceOfType<IReadOnlyList<LogMessage>>(messages);
    }

    #endregion

    #region Helper Methods

    /// <summary>Creates a LogMessage with the specified message text.</summary>
    private static LogMessage CreateLogMessage(string message) {
        return new LogMessage(LogLevel.Info, message, null, "TestCaller", 0);
    }

    /// <summary>Creates a LogMessage with the specified message text and timestamp.</summary>
    private static LogMessage CreateLogMessageWithTimestamp(string message, DateTime timestamp) {
        return new LogMessage(LogLevel.Info, message, null, "TestCaller", 0) { Timestamp = timestamp };
    }

    #endregion
}
