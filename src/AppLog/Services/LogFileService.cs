using System.Text;
using AppLog.Models;

namespace AppLog.Services;

/// <summary>
/// Service for logging UART data to file.
/// Supports Start/Stop logging with thread-safe file writes.
/// </summary>
public class LogFileService : IDisposable
{
    /// <summary>
    /// Maximum log file name length.
    /// </summary>
    private const int MaxFileNameLength = 50;

    /// <summary>
    /// Lock object for thread-safe file writing.
    /// </summary>
    private readonly object _lockObj = new();

    /// <summary>
    /// The current StreamWriter for the log file.
    /// </summary>
    private StreamWriter? _writer;

    /// <summary>
    /// Gets a value indicating whether logging is currently active.
    /// </summary>
    public bool IsLogging => _writer != null;

    /// <summary>
    /// Gets the current log file path.
    /// </summary>
    public string CurrentLogFilePath { get; private set; } = string.Empty;

    /// <summary>
    /// Starts logging to a new file in the logs/ directory.
    /// File name format: Log_DDMMYYYY_HHMM.log
    /// </summary>
    public void StartLogging()
    {
        lock (_lockObj)
        {
            StopLoggingInternal();

            string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDirectory);

            DateTime now = DateTime.Now;
            string fileName = $"Log_{now:ddMMyyyy_HHmm}.log";
            string filePath = Path.Combine(logsDirectory, fileName);

            _writer = new StreamWriter(filePath, true, Encoding.UTF8)
            {
                AutoFlush = true
            };

            CurrentLogFilePath = filePath;
        }
    }

    /// <summary>
    /// Stops logging and closes the current log file.
    /// </summary>
    public void StopLogging()
    {
        lock (_lockObj)
        {
            StopLoggingInternal();
        }
    }

    /// <summary>
    /// Writes a log entry to the current log file.
    /// Thread-safe. Flushes after each write.
    /// </summary>
    /// <param name="logEntry">The log entry to write.</param>
    public void WriteEntry(LogEntry logEntry)
    {
        lock (_lockObj)
        {
            if (_writer == null)
            {
                return;
            }

            _writer.WriteLine(logEntry.ToString());
            _writer.Flush();
        }
    }

    /// <summary>
    /// Stops logging internally without lock (caller must hold lock).
    /// </summary>
    private void StopLoggingInternal()
    {
        if (_writer != null)
        {
            _writer.Flush();
            _writer.Close();
            _writer.Dispose();
            _writer = null;
            CurrentLogFilePath = string.Empty;
        }
    }

    /// <summary>
    /// Disposes the log file service and stops logging.
    /// </summary>
    public void Dispose()
    {
        StopLogging();
        GC.SuppressFinalize(this);
    }
}