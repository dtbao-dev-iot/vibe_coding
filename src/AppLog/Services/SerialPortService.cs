using System.IO.Ports;
using AppLog.Models;

namespace AppLog.Services;

/// <summary>
/// Service for managing serial port communication.
/// Handles opening, closing, reading, and writing to UART serial ports.
/// </summary>
public class SerialPortService : IDisposable
{
    /// <summary>
    /// Default baud rate for serial communication.
    /// </summary>
    private const int DefaultBaudRate = 115200;

    /// <summary>
    /// Read timeout in milliseconds.
    /// </summary>
    private const int ReadTimeoutMs = 500;

    /// <summary>
    /// Write timeout in milliseconds.
    /// </summary>
    private const int WriteTimeoutMs = 500;

    /// <summary>
    /// The underlying SerialPort instance.
    /// </summary>
    private SerialPort? _serialPort;

    /// <summary>
    /// Gets a value indicating whether the serial port is currently open.
    /// </summary>
    public bool IsOpen => _serialPort?.IsOpen ?? false;

    /// <summary>
    /// Gets the name of the currently selected port.
    /// </summary>
    public string PortName => _serialPort?.PortName ?? string.Empty;

    /// <summary>
    /// Event raised when data is received from the serial port.
    /// </summary>
    public event EventHandler<LogEntry>? DataReceived;

    /// <summary>
    /// Event raised when a serial port error occurs.
    /// </summary>
    public event EventHandler<string>? ErrorOccurred;

    /// <summary>
    /// Gets the list of available COM port names in the system.
    /// </summary>
    /// <returns>Array of available port names.</returns>
    public static string[] GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }

    /// <summary>
    /// Opens the specified serial port with default configuration.
    /// Configuration: 115200 baud, 8 data bits, no parity, 1 stop bit.
    /// </summary>
    /// <param name="portName">The name of the COM port to open (e.g., "COM3").</param>
    /// <exception cref="InvalidOperationException">Thrown when a port is already open.</exception>
    /// <exception cref="ArgumentException">Thrown when portName is null or empty.</exception>
    public void OpenPort(string portName)
    {
        if (string.IsNullOrWhiteSpace(portName))
        {
            throw new ArgumentException("Port name cannot be null or empty.", nameof(portName));
        }

        if (_serialPort?.IsOpen == true)
        {
            throw new InvalidOperationException($"Port {_serialPort.PortName} is already open. Close it first.");
        }

        _serialPort = new SerialPort(portName)
        {
            BaudRate = DefaultBaudRate,
            DataBits = 8,
            Parity = Parity.None,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            ReadTimeout = ReadTimeoutMs,
            WriteTimeout = WriteTimeoutMs
        };

        _serialPort.DataReceived += OnDataReceived;
        _serialPort.ErrorReceived += OnErrorReceived;

        _serialPort.Open();
    }

    /// <summary>
    /// Closes the currently open serial port.
    /// </summary>
    public void ClosePort()
    {
        if (_serialPort == null)
        {
            return;
        }

        if (_serialPort.IsOpen)
        {
            _serialPort.DataReceived -= OnDataReceived;
            _serialPort.ErrorReceived -= OnErrorReceived;
            _serialPort.Close();
        }

        _serialPort.Dispose();
        _serialPort = null;
    }

    /// <summary>
    /// Sends ASCII data through the serial port.
    /// </summary>
    /// <param name="data">The ASCII string to send.</param>
    /// <exception cref="InvalidOperationException">Thrown when the port is not open.</exception>
    /// <exception cref="ArgumentException">Thrown when data is null or empty.</exception>
    public void SendData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentException("Data cannot be null or empty.", nameof(data));
        }

        if (_serialPort?.IsOpen != true)
        {
            throw new InvalidOperationException("Serial port is not open.");
        }

        _serialPort.WriteLine(data);
    }

    /// <summary>
    /// Handles the DataReceived event from the serial port.
    /// Reads available data and raises the DataReceived event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (_serialPort == null || !_serialPort.IsOpen)
        {
            return;
        }

        try
        {
            string data = _serialPort.ReadExisting();
            if (!string.IsNullOrEmpty(data))
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Direction = LogDirection.RX,
                    Data = data
                };
                DataReceived?.Invoke(this, logEntry);
            }
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, $"Read error: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the ErrorReceived event from the serial port.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
        ErrorOccurred?.Invoke(this, $"Serial port error: {e.EventType}");
    }

    /// <summary>
    /// Disposes the serial port service and closes any open connection.
    /// </summary>
    public void Dispose()
    {
        ClosePort();
        GC.SuppressFinalize(this);
    }
}