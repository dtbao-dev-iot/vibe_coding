using System.Reflection;
using AppLog.Helpers;
using AppLog.Models;
using AppLog.Services;

namespace AppLog.Forms;

/// <summary>
/// Main application form for AppLog UART Serial Log Reader.
/// Provides serial port management, log display, data sending, and file logging.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>
    /// Serial port service for UART communication.
    /// </summary>
    private readonly SerialPortService _serialPortService;

    /// <summary>
    /// Log file service for saving log entries to file.
    /// </summary>
    private readonly LogFileService _logFileService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// Sets up services, event handlers, and initial UI state.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();

        _serialPortService = new SerialPortService();
        _logFileService = new LogFileService();

        // Set form title with version
        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
        Text = $"AppLog v{version?.Major}.{version?.Minor}.{version?.Build}";

        // Wire up event handlers
        _serialPortService.DataReceived += SerialPortService_DataReceived;
        _serialPortService.ErrorOccurred += SerialPortService_ErrorOccurred;

        // Load initial port list
        RefreshPortList();

        // Set initial UI state
        UpdateUIState(isPortOpen: false);
        UpdateStatusBar();
    }

    /// <summary>
    /// Handles the Load event of the form.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void MainForm_Load(object sender, EventArgs e)
    {
        txtSendData.Enabled = false;
    }

    // =========================================================================
    // Port Config Region
    // =========================================================================

    /// <summary>
    /// Refreshes the list of available COM ports in the ComboBox.
    /// </summary>
    private void RefreshPortList()
    {
        cmbPort.Items.Clear();
        string[] ports = SerialPortService.GetAvailablePorts();
        cmbPort.Items.AddRange(ports);

        if (cmbPort.Items.Count > 0)
        {
            cmbPort.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Handles the Click event of the Update Port button.
    /// Refreshes the COM port list.
    /// </summary>
    private void btnUpdatePort_Click(object sender, EventArgs e)
    {
        RefreshPortList();
    }

    /// <summary>
    /// Handles the Click event of the Open/Close button.
    /// Toggles the serial port open/close state.
    /// </summary>
    private void btnOpenClose_Click(object sender, EventArgs e)
    {
        if (_serialPortService.IsOpen)
        {
            CloseSerialPort();
        }
        else
        {
            OpenSerialPort();
        }
    }

    /// <summary>
    /// Opens the selected serial port and updates UI.
    /// </summary>
    private void OpenSerialPort()
    {
        if (cmbPort.SelectedItem == null)
        {
            MessageBox.Show("Please select a COM port.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string portName = cmbPort.SelectedItem.ToString()!;

        try
        {
            _serialPortService.OpenPort(portName);
            UpdateUIState(isPortOpen: true);
            UpdateStatusBar();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open {portName}:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Closes the serial port and updates UI.
    /// </summary>
    private void CloseSerialPort()
    {
        try
        {
            _serialPortService.ClosePort();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error closing port:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        UpdateUIState(isPortOpen: false);
        UpdateStatusBar();
    }

    // =========================================================================
    // Log Display Region
    // =========================================================================

    /// <summary>
    /// Handles the Click event of the Clear Log button.
    /// Clears all log entries from the display.
    /// </summary>
    private void btnClearLog_Click(object sender, EventArgs e)
    {
        LogDisplayHelper.ClearLog(rtbLogDisplay);
    }

    /// <summary>
    /// Handles the Click event of the Copy Log button.
    /// Copies all log text to the clipboard.
    /// </summary>
    private void btnCopyLog_Click(object sender, EventArgs e)
    {
        LogDisplayHelper.CopyLog(rtbLogDisplay);
        MessageBox.Show("Log copied to clipboard.", "Info",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // =========================================================================
    // Send Data Region
    // =========================================================================

    /// <summary>
    /// Handles the Click event of the Send button.
    /// Sends ASCII data through the serial port.
    /// </summary>
    private void btnSend_Click(object sender, EventArgs e)
    {
        SendData();
    }

    /// <summary>
    /// Handles the KeyDown event of the Send Data TextBox.
    /// Triggers send on Enter key.
    /// </summary>
    private void txtSendData_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            SendData();
        }
    }

    /// <summary>
    /// Sends the data from the TextBox through the serial port.
    /// Displays TX entry in log and writes to log file if active.
    /// </summary>
    private void SendData()
    {
        string data = txtSendData.Text.Trim();
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        if (!_serialPortService.IsOpen)
        {
            MessageBox.Show("Serial port is not open.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _serialPortService.SendData(data);

            // Create TX log entry
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Direction = LogDirection.TX,
                Data = data
            };

            // Display in RichTextBox
            LogDisplayHelper.AppendLogEntry(rtbLogDisplay, logEntry);

            // Write to file if logging
            if (_logFileService.IsLogging)
            {
                _logFileService.WriteEntry(logEntry);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to send data:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // =========================================================================
    // Log File Region
    // =========================================================================

    /// <summary>
    /// Handles the Click event of the Start/Stop Log button.
    /// Toggles file logging on/off.
    /// </summary>
    private void btnStartStopLog_Click(object sender, EventArgs e)
    {
        if (_logFileService.IsLogging)
        {
            _logFileService.StopLogging();
        }
        else
        {
            _logFileService.StartLogging();
        }

        UpdateLogButtonState();
        UpdateStatusBar();
    }

    // =========================================================================
    // Serial Port Events
    // =========================================================================

    /// <summary>
    /// Handles the DataReceived event from the serial port service.
    /// Invokes UI update on the main thread.
    /// </summary>
    private void SerialPortService_DataReceived(object? sender, LogEntry logEntry)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => SerialPortService_DataReceived(sender, logEntry)));
            return;
        }

        // Display in RichTextBox
        LogDisplayHelper.AppendLogEntry(rtbLogDisplay, logEntry);

        // Write to file if logging
        if (_logFileService.IsLogging)
        {
            _logFileService.WriteEntry(logEntry);
        }
    }

    /// <summary>
    /// Handles the ErrorOccurred event from the serial port service.
    /// Invokes error display on the main thread.
    /// </summary>
    private void SerialPortService_ErrorOccurred(object? sender, string errorMessage)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => SerialPortService_ErrorOccurred(sender, errorMessage)));
            return;
        }

        // Display error in log
        var errorEntry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Direction = LogDirection.RX,
            Data = $"[ERROR] {errorMessage}"
        };
        LogDisplayHelper.AppendLogEntry(rtbLogDisplay, errorEntry);
    }

    // =========================================================================
    // UI State Management
    // =========================================================================

    /// <summary>
    /// Updates UI controls based on port open/close state.
    /// </summary>
    /// <param name="isPortOpen">Whether the serial port is open.</param>
    private void UpdateUIState(bool isPortOpen)
    {
        cmbPort.Enabled = !isPortOpen;
        btnUpdatePort.Enabled = !isPortOpen;
        btnOpenClose.Text = isPortOpen ? "Close" : "Open";
        btnSend.Enabled = isPortOpen;
        txtSendData.Enabled = isPortOpen;
    }

    /// <summary>
    /// Updates the Start/Stop Log button text based on logging state.
    /// </summary>
    private void UpdateLogButtonState()
    {
        btnStartStopLog.Text = _logFileService.IsLogging ? "Stop Log" : "Start Log";
    }

    /// <summary>
    /// Updates the status bar with current connection and logging state.
    /// </summary>
    private void UpdateStatusBar()
    {
        string connectionStatus = _serialPortService.IsOpen ? "Connected" : "Disconnected";
        string portInfo = _serialPortService.IsOpen ? _serialPortService.PortName : "-";
        string loggingStatus = _logFileService.IsLogging ? "Logging" : "Not logging";

        lblStatus.Text = $"● {connectionStatus} | COM {portInfo} | {loggingStatus}";
        lblStatus.ForeColor = _serialPortService.IsOpen ? Color.Green : Color.Red;
    }

    /// <summary>
    /// Handles the FormClosing event.
    /// Cleans up services before closing.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        _serialPortService.Dispose();
        _logFileService.Dispose();
    }
}