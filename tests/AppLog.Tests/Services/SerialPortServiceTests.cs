using AppLog.Services;

namespace AppLog.Tests.Services;

/// <summary>
/// Unit tests for the <see cref="SerialPortService"/> class.
/// Tests focus on validation, state management, and error handling
/// without requiring actual hardware (COM ports).
/// </summary>
public class SerialPortServiceTests : IDisposable
{
    /// <summary>
    /// The service instance under test.
    /// </summary>
    private readonly SerialPortService _service;

    /// <summary>
    /// Initializes a new test instance with a fresh SerialPortService.
    /// </summary>
    public SerialPortServiceTests()
    {
        _service = new SerialPortService();
    }

    // =========================================================================
    // Initial State Tests
    // =========================================================================

    [Fact]
    public void InitialState_IsNotOpen()
    {
        Assert.False(_service.IsOpen);
    }

    [Fact]
    public void InitialState_PortNameIsEmpty()
    {
        Assert.Equal(string.Empty, _service.PortName);
    }

    [Fact]
    public void InitialState_BaudRateIsZero()
    {
        Assert.Equal(0, _service.BaudRate);
    }

    // =========================================================================
    // OpenPort Validation Tests
    // =========================================================================

    [Fact]
    public void OpenPort_NullPortName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.OpenPort(null!));
    }

    [Fact]
    public void OpenPort_EmptyPortName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.OpenPort(string.Empty));
    }

    [Fact]
    public void OpenPort_WhitespacePortName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.OpenPort("   "));
    }

    [Fact]
    public void OpenPort_NonExistentPort_ThrowsException()
    {
        Assert.ThrowsAny<Exception>(() => _service.OpenPort("COM_NONEXISTENT_99999"));
    }

    // =========================================================================
    // ClosePort Tests
    // =========================================================================

    [Fact]
    public void ClosePort_WhenNotOpen_DoesNotThrow()
    {
        // Should not throw when closing a port that was never opened
        var exception = Record.Exception(() => _service.ClosePort());
        Assert.Null(exception);
    }

    // =========================================================================
    // SendData Validation Tests
    // =========================================================================

    [Fact]
    public void SendData_WhenPortNotOpen_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _service.SendData("test"));
    }

    [Fact]
    public void SendData_NullData_ThrowsArgumentException()
    {
        // Even if we could open a port, null data should be rejected
        Assert.Throws<ArgumentException>(() => _service.SendData(null!));
    }

    [Fact]
    public void SendData_EmptyData_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.SendData(string.Empty));
    }

    [Fact]
    public void SendData_WhitespaceData_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.SendData("   "));
    }

    // =========================================================================
    // GetAvailablePorts Tests
    // =========================================================================

    [Fact]
    public void GetAvailablePorts_ReturnsNonNullArray()
    {
        string[] ports = SerialPortService.GetAvailablePorts();
        Assert.NotNull(ports);
    }

    [Fact]
    public void GetAvailablePorts_StaticMethod_DoesNotRequireInstance()
    {
        // Verify it's callable without an instance
        string[] ports = SerialPortService.GetAvailablePorts();
        Assert.NotNull(ports);
    }

    // =========================================================================
    // Dispose Tests
    // =========================================================================

    [Fact]
    public void Dispose_WhenNotOpen_DoesNotThrow()
    {
        var exception = Record.Exception(() => _service.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        _service.Dispose();
        _service.Dispose();
        // No exception = pass
    }

    // =========================================================================
    // Event Tests
    // =========================================================================

    [Fact]
    public void DataReceivedEvent_CanSubscribeAndUnsubscribe()
    {
        EventHandler<AppLog.Models.LogEntry>? handler = null;
        handler = (s, e) => { };

        _service.DataReceived += handler;
        _service.DataReceived -= handler;

        // No exception = pass
    }

    [Fact]
    public void ErrorOccurredEvent_CanSubscribeAndUnsubscribe()
    {
        EventHandler<string>? handler = null;
        handler = (s, e) => { };

        _service.ErrorOccurred += handler;
        _service.ErrorOccurred -= handler;

        // No exception = pass
    }

    /// <summary>
    /// Disposes the service after each test.
    /// </summary>
    public void Dispose()
    {
        _service.Dispose();
        GC.SuppressFinalize(this);
    }
}