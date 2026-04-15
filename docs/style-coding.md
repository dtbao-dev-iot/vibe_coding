# Style & Coding Convention - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Tech Stack:** C# .NET 8 WinForms  
> **Last Updated:** 2026-04-15

---

## 1. Naming Convention

### 1.1 General Rules

| Element | Convention | Example |
|---------|-----------|---------|
| Namespace | PascalCase, follow folder structure | `AppLog.Services`, `AppLog.Models` |
| Class | PascalCase, noun | `SerialPortService`, `LogEntry` |
| Interface | PascalCase, prefix `I` | `ISerialPortService`, `ILogFileService` |
| Struct | PascalCase, noun | `PortConfig` |
| Enum | PascalCase, singular noun | `LogDirection`, `LogLevel` |
| Enum value | PascalCase | `TX`, `RX` |
| Method | PascalCase, verb | `OpenPort()`, `GetAvailablePorts()` |
| Property | PascalCase | `IsLogging`, `PortName` |
| Public field | PascalCase (prefer property) | `MaxBufferSize` |
| Private field | camelCase, prefix `_` | `_serialPort`, `_logWriter` |
| Parameter | camelCase | `portName`, `asciiData` |
| Local variable | camelCase | `availablePorts`, `logEntry` |
| Constant | PascalCase | `MaxBufferSize`, `DefaultBaudRate` |
| Event | PascalCase, past tense or before-after | `LogEntryReceived`, `PortDisconnected` |
| Generic type param | Prefix `T` | `TResult`, `TEntity` |

### 1.2 WinForms Controls Naming

| Control Type | Prefix | Example |
|-------------|--------|---------|
| Button | `btn` | `btnOpenClose`, `btnSend`, `btnClearLog` |
| ComboBox | `cmb` | `cmbPort` |
| TextBox | `txt` | `txtSendData` |
| RichTextBox | `rtb` | `rtbLogDisplay` |
| Label | `lbl` | `lblStatus` |
| StatusStrip | `ssMain` | `ssMain` |
| ToolStripStatusLabel | `tssl` | `tsslConnection`, `tsslLogFile` |
| Panel | `pnl` | `pnlPortConfig` |
| TableLayoutPanel | `tlp` | `tlpMain` |

### 1.3 Forbidden

- ❌ Hungarian notation outside control prefix (`strName`, `intCount`)
- ❌ Unclear abbreviations (`svc`, `mgr`, `ctx`) — except common ones (`Id`, `Xml`, `IO`)
- ❌ Single letter variables (except loop counters `i`, `j`, `k`)
- ❌ Numbers in names (`Button1`, `TextBox2`)

---

## 2. File and Folder Naming

### 2.1 Source Files

| Rule | Description | Example |
|------|-------------|---------|
| File name = Class name | Each file contains 1 main class | `SerialPortService.cs` contains class `SerialPortService` |
| PascalCase | Capitalize first letter of each word | `LogFileService.cs` |
| Partial class | `ClassName.Designer.cs` for auto-generated | `MainForm.Designer.cs` |
| Resources | `ClassName.resx` | `MainForm.resx` |

### 2.2 Folder Naming

| Rule | Description | Example |
|------|-------------|---------|
| PascalCase | Capitalize first letter of each word | `Services/`, `Models/`, `Helpers/` |
| Namespace match | Folder name = Namespace segment | `Forms/` → `AppLog.Forms` |
| Singular for category | Singular noun | `Model` not `Models` (optional) |

### 2.3 Documentation Files

| Rule | Description | Example |
|------|-------------|---------|
| lowercase-kebab-case | Lowercase, hyphen-separated | `commit-convention.md`, `style-coding.md` |
| .md extension | Markdown format | All docs |

### 2.4 Script Files

| Rule | Description | Example |
|------|-------------|---------|
| kebab-case | Lowercase, hyphen-separated | `build.ps1`, `version-bump.ps1` |
| snake_case for Python | `commit_msg_check.py` | Python scripts |

---

## 3. Code Formatting

### 3.1 Indentation & Spacing

```csharp
// 4 spaces indentation, NO tabs
// 1 space between type cast and value
var portName = (string)comboBox.SelectedItem;

// Space after keyword in control flow
if (condition)
{
    // ...
}

for (int i = 0; i < count; i++)
{
    // ...
}
```

### 3.2 Braces

```csharp
// Opening brace on new line (Allman style)
public void OpenPort(string portName)
{
    if (string.IsNullOrEmpty(portName))
    {
        throw new ArgumentNullException(nameof(portName));
    }

    try
    {
        _serialPort.PortName = portName;
        _serialPort.Open();
    }
    catch (UnauthorizedAccessException ex)
    {
        throw new InvalidOperationException(
            $"Port {portName} is in use.", ex);
    }
}
```

### 3.3 Line Length & Wrapping

```csharp
// Limit 120 characters per line
// Wrap parameter list if too long
public LogEntry(
    DateTime timestamp,
    LogDirection direction,
    string data)
{
    Timestamp = timestamp;
    Direction = direction;
    Data = data;
}

// Wrap method call if too long
var logEntry = new LogEntry(
    DateTime.Now,
    LogDirection.TX,
    asciiData);
```

### 3.4 Blank Lines

```csharp
// 1 blank line between methods
public void OpenPort(string portName)
{
    // ...
}

public void ClosePort()
{
    // ...

}

// 1 blank line between logical blocks in method
public void ProcessData(string data)
{
    // Validate input
    if (string.IsNullOrEmpty(data))
    {
        return;
    }

    // Parse data
    var entries = ParseData(data);

    // Notify subscribers
    foreach (var entry in entries)
    {
        OnLogEntryReceived(entry);
    }
}
```

### 3.5 Using Directives

```csharp
// System namespaces first
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

// Blank line
// Third-party packages

// Blank line
// Project namespaces
using AppLog.Models;

namespace AppLog.Services
{
    // ...
}
```

### 3.6 File Template

```csharp
/// <file>
/// <summary>
/// Brief description of the file's purpose.
/// </summary>
/// <project>AppLog - UART Serial Log Reader</project>
/// <author>Author Name</author>
/// <date>2026-04-15</date>
/// </file>

using System;
using System.IO.Ports;

using AppLog.Models;

namespace AppLog.Services
{
    /// <summary>
    /// Manages serial port communication for UART data transfer.
    /// </summary>
    public class SerialPortService : IDisposable
    {
        // Implementation
    }
}
```

---

## 4. Doxygen / XML Documentation Rules

This project uses **C# XML Documentation Comments** (compatible with Doxygen and IDE tooltips).

### 4.1 Class Documentation

```csharp
/// <summary>
/// Manages serial port communication for UART data transfer.
/// Provides methods to open, close, read, and write serial port data.
/// </summary>
/// <remarks>
/// This class is thread-safe for concurrent read/write operations.
/// Implements <see cref="IDisposable"/> for proper resource cleanup.
/// </remarks>
/// <example>
/// <code>
/// var service = new SerialPortService();
/// service.OnLogEntryReceived += (sender, entry) => Console.WriteLine(entry);
/// service.OpenPort("COM3");
/// service.SendData("AT+RESET");
/// service.ClosePort();
/// </code>
/// </example>
public class SerialPortService : IDisposable
{
    // ...
}
```

### 4.2 Method Documentation

```csharp
/// <summary>
/// Opens the specified serial port with default configuration.
/// </summary>
/// <param name="portName">Name of the serial port (e.g., "COM3").</param>
/// <exception cref="ArgumentNullException">
/// Thrown when <paramref name="portName"/> is null or empty.
/// </exception>
/// <exception cref="InvalidOperationException">
/// Thrown when the port is already open or cannot be accessed.
/// </exception>
/// <remarks>
/// Default configuration: 115200 baud, 8 data bits, no parity, 1 stop bit.
/// </remarks>
public void OpenPort(string portName)
{
    // ...
}
```

### 4.3 Property Documentation

```csharp
/// <summary>
/// Gets a value indicating whether the serial port is currently open.
/// </summary>
/// <value>
/// <c>true</c> if the serial port is open; otherwise, <c>false</c>.
/// </value>
public bool IsPortOpen => _serialPort?.IsOpen ?? false;
```

### 4.4 Event Documentation

```csharp
/// <summary>
/// Occurs when a new log entry is received (either TX or RX).
/// </summary>
/// <remarks>
/// This event is raised on the serial port's reader thread.
/// UI updates must be marshaled to the UI thread using
/// <see cref="Control.Invoke(Delegate)"/> or <see cref="Control.BeginInvoke(Delegate)"/>.
/// </remarks>
public event EventHandler<LogEntry>? OnLogEntryReceived;
```

### 4.5 Enum Documentation

```csharp
/// <summary>
/// Specifies the direction of serial data transmission.
/// </summary>
public enum LogDirection
{
    /// <summary>
    /// Data transmitted from host to UART device.
    /// </summary>
    TX,

    /// <summary>
    /// Data received from UART device to host.
    /// </summary>
    RX
}
```

### 4.6 XML Tags Reference

| Tag | Description | Required |
|-----|-------------|----------|
| `<summary>` | Brief description | ✅ Class, method, property, enum |
| `<remarks>` | Additional detailed description | When needed |
| `<param>` | Parameter description | ✅ Each parameter |
| `<returns>` | Return value description | Method with return != void |
| `<exception>` | Exception type + description | Method that can throw |
| `<example>` | Usage example | Main public API |
| `<value>` | Property value description | Properties |
| `<seealso>` | Cross-reference | When needed |
| `<code>` | Code block in example | Inside `<example>` |
| `<c>` | Inline code | Inside text |
| `<paramref>` | Reference to parameter | Inside text |

---

## 5. Struct & Class Definitions

### 5.1 Class Layout Order

```csharp
public class ClassName
{
    // 1. Constants
    public const int MaxBufferSize = 1048576; // 1MB

    // 2. Static fields
    private static readonly object _lock = new();

    // 3. Instance fields
    private readonly SerialPort _serialPort;
    private bool _isDisposed;

    // 4. Constructors
    public ClassName()
    {
        _serialPort = new SerialPort();
    }

    // 5. Events
    public event EventHandler<LogEntry>? OnLogEntryReceived;

    // 6. Properties
    public bool IsPortOpen => _serialPort?.IsOpen ?? false;

    // 7. Public methods
    public void OpenPort(string portName)
    {
        // ...
    }

    // 8. Protected / Private methods
    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        // ...
    }

    // 9. IDisposable implementation
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _serialPort?.Dispose();
            }
            _isDisposed = true;
        }
    }
}
```

### 5.2 Struct vs Class

| Criteria | Struct | Class |
|----------|--------|-------|
| Size | < 16 bytes | All other cases |
| Semantics | Value type (copy) | Reference type |
| Lifetime | Short-lived | Long-lived |
| Mutable | Prefer immutable | Can be mutable |
| Inheritance | Not supported | Supported |

```csharp
/// <summary>
/// Immutable struct for serial port configuration.
/// </summary>
public readonly struct PortConfig
{
    public string PortName { get; init; }
    public int BaudRate { get; init; }
    public Parity Parity { get; init; }
    public int DataBits { get; init; }
    public StopBits StopBits { get; init; }
}
```

### 5.3 IDisposable Pattern

```csharp
/// <summary>
/// Base class for services that manage disposable resources.
/// </summary>
public abstract class DisposableBase : IDisposable
{
    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            DisposeManagedResources();
        }

        DisposeUnmanagedResources();
        _isDisposed = true;
    }

    protected virtual void DisposeManagedResources() { }
    protected virtual void DisposeUnmanagedResources() { }

    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }
}
```

---

## 6. Error Handling

### 6.1 Exception Handling

```csharp
// ✅ Good: Specific exception types, meaningful message
try
{
    _serialPort.Open();
}
catch (UnauthorizedAccessException ex)
{
    throw new InvalidOperationException(
        $"Serial port {_serialPort.PortName} is already in use by another application.", ex);
}
catch (IOException ex)
{
    throw new InvalidOperationException(
        $"Failed to open serial port {_serialPort.PortName}.", ex);
}

// ❌ Bad: Catch all exceptions
try
{
    _serialPort.Open();
}
catch (Exception)
{
    // Silently ignore
}
```

### 6.2 Argument Validation

```csharp
public void SendData(string asciiData)
{
    ArgumentNullException.ThrowIfNull(asciiData);

    if (asciiData.Length == 0)
    {
        throw new ArgumentException("Data cannot be empty.", nameof(asciiData));
    }

    ThrowIfDisposed();

    // Implementation
}
```

---

## 7. Thread Safety

### 7.1 Lock Pattern

```csharp
private readonly object _lock = new();

public void WriteEntry(LogEntry entry)
{
    lock (_lock)
    {
        _writer?.WriteLine(entry.ToString());
        _writer?.Flush();
    }
}
```

### 7.2 Invoke Pattern for UI

```csharp
private void OnLogEntryReceived(object? sender, LogEntry entry)
{
    if (rtbLogDisplay.InvokeRequired)
    {
        rtbLogDisplay.BeginInvoke(new Action(() => OnLogEntryReceived(sender, entry)));
        return;
    }

    // Safe to update UI
    AppendLogEntry(entry);
}
```

---

## 8. String Formatting

### 8.1 Log Entry Format

```csharp
// Format: [HH:MM:SS.SSS] [TX] data
public override string ToString()
{
    var direction = Direction == LogDirection.TX ? "TX" : "RX";
    return $"[{Timestamp:HH:mm:ss.fff}] [{direction}] {Data}";
}
```

### 8.2 Log File Name Format

```csharp
// Format: Log_DDMMYYYY_HHMM.log
var fileName = $"Log_{DateTime.Now:ddMMyyyy_HHmm}.log";
```

### 8.3 String Interpolation

```csharp
// ✅ Use string interpolation (C# 6+)
var message = $"Port {portName} opened successfully at {baudRate} baud.";

// ❌ Don't use string.Format
var message = string.Format("Port {0} opened successfully at {1} baud.", portName, baudRate);
```
