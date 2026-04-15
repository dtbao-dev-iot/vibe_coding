namespace AppLog.Models;

/// <summary>
/// Represents the direction of a log entry (Transmit or Receive).
/// </summary>
public enum LogDirection
{
    /// <summary>
    /// Data sent to UART (Transmit).
    /// </summary>
    TX,

    /// <summary>
    /// Data received from UART (Receive).
    /// </summary>
    RX
}

/// <summary>
/// Represents a single log entry with timestamp, direction, and data.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets or sets the timestamp of the log entry.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the direction of the log entry (TX or RX).
    /// </summary>
    public LogDirection Direction { get; set; }

    /// <summary>
    /// Gets or sets the data content of the log entry.
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Returns the formatted string representation of the log entry.
    /// Format: [HH:MM:SS.SSS] [TX/RX] data
    /// </summary>
    /// <returns>Formatted log string.</returns>
    public override string ToString()
    {
        return $"[{Timestamp:HH:mm:ss.fff}] [{Direction}] {Data}";
    }
}