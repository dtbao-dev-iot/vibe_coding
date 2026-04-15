using AppLog.Models;
using AppLog.Services;

namespace AppLog.Tests.Services;

/// <summary>
/// Unit tests for the <see cref="LogFileService"/> class.
/// </summary>
public class LogFileServiceTests : IDisposable
{
    private readonly LogFileService _logFileService;

    public LogFileServiceTests()
    {
        _logFileService = new LogFileService();
    }

    [Fact]
    public void IsLogging_BeforeStart_ReturnsFalse()
    {
        // Assert
        Assert.False(_logFileService.IsLogging);
    }

    [Fact]
    public void IsLogging_AfterStart_ReturnsTrue()
    {
        // Act
        _logFileService.StartLogging();

        // Assert
        Assert.True(_logFileService.IsLogging);
    }

    [Fact]
    public void IsLogging_AfterStartThenStop_ReturnsFalse()
    {
        // Act
        _logFileService.StartLogging();
        _logFileService.StopLogging();

        // Assert
        Assert.False(_logFileService.IsLogging);
    }

    [Fact]
    public void StartLogging_SetsCurrentLogFilePath()
    {
        // Act
        _logFileService.StartLogging();

        // Assert
        Assert.NotEmpty(_logFileService.CurrentLogFilePath);
        Assert.Contains("Log_", _logFileService.CurrentLogFilePath);
        Assert.EndsWith(".log", _logFileService.CurrentLogFilePath);
    }

    [Fact]
    public void StopLogging_ClearsCurrentLogFilePath()
    {
        // Arrange
        _logFileService.StartLogging();

        // Act
        _logFileService.StopLogging();

        // Assert
        Assert.Empty(_logFileService.CurrentLogFilePath);
    }

    [Fact]
    public void WriteEntry_WhenNotLogging_DoesNotThrow()
    {
        // Arrange
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Direction = LogDirection.TX,
            Data = "test data"
        };

        // Act & Assert - should not throw
        _logFileService.WriteEntry(entry);
    }

    [Fact]
    public void Dispose_StopsLogging()
    {
        // Arrange
        _logFileService.StartLogging();

        // Act
        _logFileService.Dispose();

        // Assert
        Assert.False(_logFileService.IsLogging);
    }

    public void Dispose()
    {
        _logFileService.Dispose();
    }
}