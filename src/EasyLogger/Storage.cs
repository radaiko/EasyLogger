// ╔═════════════════════════════════════════════════════════════════════════════╗
// ║                                                                             ║
// ║   File:        Storage.cs                                                   ║
// ║   Created:     December 1, 2025                                             ║
// ║   Description: Manages an in-memory collection of log messages              ║
// ║                                                                             ║
// ╚═════════════════════════════════════════════════════════════════════════════╝

namespace EasyLogger;

/// <summary>Manages an in-memory collection of log messages with filtering and retrieval capabilities.</summary>
internal sealed class Storage {

    private List<LogMessage> _messages = [];

    /// <summary>Adds a log message to the storage collection.</summary>
    /// <param name="message">The log message to add.</param>
    public void Add(LogMessage message) => _messages.Add(message);

    /// <summary>Removes all log messages from the storage collection.</summary>
    public void Clear() => _messages.Clear();

    /// <summary>Removes all log messages with a timestamp older than the specified cutoff date.</summary>
    /// <param name="cutoff">The cutoff date; messages older than this are removed.</param>
    public void RemoveOlderThan(DateTime cutoff) => _messages = _messages.Where(m => m.Timestamp >= cutoff).ToList();

    /// <summary>Gets a read-only view of all log messages in storage.</summary>
    /// <returns>A read-only list of all log messages.</returns>
    public IReadOnlyList<LogMessage> GetAll() => _messages.AsReadOnly();
}
