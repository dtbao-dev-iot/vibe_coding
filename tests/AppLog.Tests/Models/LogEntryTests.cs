using AppLog.Models;

namespace AppLog.Tests.Models;

/// <summary>
/// Unit tests for the <see cref="LogEntry"/> class.
/// </summary>
public class LogEntryTests
{
    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = new DateTime(2026, 4, 15, 14, 30, 25, 123),
            Direction = LogDirection.TX,
            Data = "AT+RESET"
        };

        // Act
        string result = entry.ToString();

        // Assert
        Assert.Equal("[14:30:25.123] [TX] AT+RESET", result);
    }

    [Fact]
    public void ToString_RXDirection_ReturnsCorrectFormat()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = new DateTime(2026, 4, 15, 14, 30, 25, 456),
            Direction = LogDirection.RX,
            Data = "OK"
        };

        // Act
        string result = entry.ToString();

        // Assert
        Assert.Equal("[14:30:25.456] [RX] OK", result);
    }

    [Fact]
    public void LogDirection_HasCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)LogDirection.TX);
        Assert.Equal(1, (int)LogDirection.RX);
    }
}