# Product Requirement Document (PRD) - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Version:** 1.0  
> **Last Updated:** 2026-04-15

---

## 1. Overview

### 1.1 Product Name
**AppLog** - UART Serial Log Reader

### 1.2 Product Description
AppLog is a Windows desktop application built with C# .NET 8 WinForms, designed to read and display log data from UART serial ports. The application supports sending ASCII data to devices, displaying real-time logs with TX/RX distinction, and saving logs to files.

### 1.3 Target Users
- Embedded firmware developers who need to debug UART communication
- Hardware engineers testing serial communication with MCU/SOC
- QA engineers verifying UART output from devices

### 1.4 Tech Stack

| Component | Technology |
|-----------|-----------|
| Language | C# 12 (.NET 8) |
| UI Framework | Windows Forms (WinForms) |
| Serial Communication | `System.IO.Ports` (NuGet package) |
| Build System | MSBuild / dotnet CLI |
| CI/CD | GitHub Actions |
| Testing | xUnit |
| Version Control | Git + GitHub |

---

## 2. Features

### 2.1 F1: Serial Port Selection

| Attribute | Value |
|-----------|---------|
| **ID** | F1 |
| **Priority** | P0 (Must have) |
| **Status** | Planned |

**Description:**
- ComboBox displaying available COM ports in the system
- "Update Port" button to refresh port list
- ComboBox allows selecting a single port

**UI Elements:**
- `ComboBox cmbPort` - Dropdown to select COM port
- `Button btnUpdatePort` - "Update Port" button

**Behavior:**
- App startup → automatically load port list
- Press "Update Port" → refresh list from `SerialPort.GetPortNames()`
- When port is Open → ComboBox and btnUpdatePort are disabled (grayed out)
- When port is Closed → ComboBox and btnUpdatePort are enabled

**Acceptance Criteria:**
- [ ] ComboBox displays correct list of COM ports
- [ ] "Update Port" refreshes the list
- [ ] Disabled when port is open
- [ ] Enabled when port is closed

---

### 2.2 F2: Open/Close Serial Port

| Attribute | Value |
|-----------|---------|
| **ID** | F2 |
| **Priority** | P0 (Must have) |
| **Status** | Planned |

**Description:**
- "Open" button to open the selected serial port
- After opening, button text changes to "Close"
- When Closed → returns to "Open" state

**UI Elements:**
- `Button btnOpenClose` - Toggle button "Open" ↔ "Close"

**Behavior:**
- Press "Open":
  1. Open serial port with config: 115200 baud, 8 data bits, no parity, 1 stop bit
  2. Button text → "Close"
  3. ComboBox `cmbPort` → `Enabled = false`
  4. Button `btnUpdatePort` → `Enabled = false`
  5. Button `btnSend` → `Enabled = true`
  6. Start receiving data from UART

- Press "Close":
  1. Close serial port
  2. Button text → "Open"
  3. ComboBox `cmbPort` → `Enabled = true`
  4. Button `btnUpdatePort` → `Enabled = true`
  5. Button `btnSend` → `Enabled = false`
  6. Stop receiving data

**Acceptance Criteria:**
- [ ] Toggle Open/Close works correctly
- [ ] UI state changes correctly on Open and Close
- [ ] Successful port open does not throw exception
- [ ] Non-existent port → display error message

---

### 2.3 F3: Log Display

| Attribute | Value |
|-----------|---------|
| **ID** | F3 |
| **Priority** | P0 (Must have) |
| **Status** | Planned |

**Description:**
- Area to display real-time log text from UART
- Distinguish TX/RX with different colors
- Display timestamp in `HH:MM:SS.SSS` format
- Maximum buffer limit of 1MB

**UI Elements:**
- `RichTextBox rtbLogDisplay` - Log display area
- `Button btnClearLog` - "Clear" button
- `Button btnCopyLog` - "Copy" button

**Log Format:**
```
[HH:MM:SS.SSS] [TX] data_sent_to_uart
[HH:MM:SS.SSS] [RX] data_received_from_uart
```

**Color Coding:**
| Direction | Color | Hex |
|-----------|-------|-----|
| Timestamp | Gray | `#808080` |
| TX | Blue | `#0000FF` |
| RX | Green | `#008000` |

**Buffer Management:**
- Maximum log display size: **1MB** (1,048,576 characters)
- When exceeding 1MB → remove oldest lines (FIFO) until below threshold
- Check after each append

**Behavior:**
- Auto-scroll to bottom on new log entry
- Press "Clear" → clear all log display
- Press "Copy" → copy all log to clipboard
- Readonly log display (user cannot edit directly)

**Acceptance Criteria:**
- [ ] Log displays real-time when UART receives/sends data
- [ ] Timestamp in correct HH:MM:SS.SSS format
- [ ] TX displays in blue
- [ ] RX displays in green
- [ ] Auto-scroll works
- [ ] Buffer does not exceed 1MB
- [ ] Clear removes all log
- [ ] Copy copies all log to clipboard

---

### 2.4 F4: Send ASCII Data

| Attribute | Value |
|-----------|---------|
| **ID** | F4 |
| **Priority** | P0 (Must have) |
| **Status** | Planned |

**Description:**
- TextBox to enter ASCII data
- "Send" button sends data via UART
- After sending, keep data in input field

**UI Elements:**
- `TextBox txtSendData` - Input for ASCII data
- `Button btnSend` - "Send" button

**Behavior:**
- Enter ASCII data in TextBox
- Press "Send" or Enter → send data via serial port
- Data sent successfully → display TX entry in log display
- **Keep data** in TextBox after sending (do not clear)
- "Send" button only enabled when port is Open
- Do not send if TextBox is empty

**Acceptance Criteria:**
- [ ] Send ASCII data via UART successfully
- [ ] TX entry displays in log display after send
- [ ] Data remains in TextBox after send
- [ ] Enter key triggers Send
- [ ] Send button disabled when port is Closed
- [ ] Does not send empty data

---

### 2.5 F5: Save Log to File

| Attribute | Value |
|-----------|---------|
| **ID** | F5 |
| **Priority** | P0 (Must have) |
| **Status** | Planned |

**Description:**
- Save log (TX and RX) to file
- Toggle Start/Stop logging
- File naming in standard format
- **Prioritize logging** - do not miss data

**UI Elements:**
- `Button btnStartStopLog` - Toggle "Start Log" ↔ "Stop Log"

**File Format:**
```
File name: Log_DDMMYYYY_HHMM.log
Example:   Log_15042026_1430.log
Folder:    logs/ (relative to app executable)
```

**File Content:**
```
[14:30:25.123] [TX] AT+RESET
[14:30:25.456] [RX] OK
[14:30:26.001] [RX] System ready
```

**Behavior:**
- Press "Start Log":
  1. Create `logs/` folder if it doesn't exist
  2. Create file `Log_DDMMYYYY_HHMM.log` with current timestamp
  3. Open StreamWriter for the file
  4. Button text → "Stop Log"
  5. All log entries (TX and RX) are written to file

- Press "Stop Log":
  1. Flush and close StreamWriter
  2. Button text → "Start Log"
  3. Stop writing to file

- **Priority Write:**
  - Use `lock()` for thread safety
  - `Flush()` after each write entry
  - Ensure no data loss even at high log speed

**Acceptance Criteria:**
- [ ] Start/Stop toggle works correctly
- [ ] File created in correct name format `Log_DDMMYYYY_HHMM.log`
- [ ] Folder `logs/` created automatically
- [ ] File content in correct format
- [ ] Both TX and RX are logged
- [ ] Thread-safe, no data loss
- [ ] Flush after each write

---

## 3. Non-Functional Requirements

### 3.1 Performance

| Requirement | Target |
|-------------|--------|
| Log display latency | < 50ms from data received to display |
| Max UART baud rate support | 921600 bps |
| Log file write latency | < 10ms per entry |
| Memory usage | < 100MB during normal operation |
| App startup time | < 3 seconds |

### 3.2 Reliability

| Requirement | Description |
|-------------|--------|
| No crash on disconnect | App does not crash on unexpected USB unplug |
| No data loss | Log file does not miss entries when logging is active |
| Auto-recovery | UI recoverable after port disconnect |

### 3.3 Usability

| Requirement | Description |
|-------------|--------|
| Minimum window size | 600 x 400 pixels |
| Responsive layout | Controls resize properly when window is resized |
| Clear visual feedback | Button states, log colors, status bar |
| Error messages | User-friendly MessageBox for errors |

### 3.4 Compatibility

| Requirement | Description |
|-------------|--------|
| OS | Windows 10 / Windows 11 |
| Runtime | .NET 8 Runtime |
| Architecture | x64 |

---

## 4. UI Wireframe

```
┌─────────────────────────────────────────────────────────────┐
│  AppLog v0.1.0                                              │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Port Config                                         │    │
│  │  [ComboBox: COM Port ▼]  [Update Port]  [Open]      │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Log Display                                         │    │
│  │ [14:30:25.123] [TX] AT+RESET                        │    │
│  │ [14:30:25.456] [RX] OK                               │    │
│  │ [14:30:26.001] [RX] System ready                     │    │
│  │ [14:30:26.102] [TX] AT+STATUS                        │    │
│  │ [14:30:26.250] [RX] STATUS: RUNNING                  │    │
│  │                                                      │    │
│  │                                      [Clear] [Copy]  │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Send Data                                           │    │
│  │  [TextBox: Enter ASCII data_________]  [Send]        │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Log File                                            │    │
│  │  [Start Log]                                         │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                             │
│  ├─────────────────────────────────────────────────────────┤  │
│  │ ● Disconnected | COM - | Not logging                   │  │
│  └─────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## 5. Data Flow

```
┌──────────┐     UART RX      ┌─────────────────┐     Event     ┌───────────┐
│  UART    │ ──────────────→  │ SerialPortService│ ───────────→ │ MainForm  │
│  Device  │                   │ (DataReceived)  │              │ (Display) │
│          │ ←────────────── │                  │              │           │
└──────────┘     UART TX      └─────────────────┘              └─────┬─────┘
                   ↑                                                 │
                   │                                                 │
              SendData()                                    LogEntry (TX/RX)
                   ↑                                                 │
           ┌───────────────┐                                          │
           │  User Input   │                                          │
           │  (ASCII data) │                                          │
           └───────────────┘                                          │
                                                                          │
                   ┌──────────────────────────────────────────────────┐   │
                   │              LogFileService                      │   │
                   │  (if logging active → write to file)            │←──┘
                   └──────────────────────────────────────────────────┘
                                      │
                                      ▼
                            ┌──────────────────┐
                            │ logs/Log_DDMMYYYY │
                            │     _HHMM.log     │
                            └──────────────────┘
```

---

## 6. Serial Port Configuration

### Default Configuration

| Parameter | Default Value | Note |
|-----------|--------------|------|
| Baud Rate | 115200 | Common for embedded debugging |
| Data Bits | 8 | Standard |
| Parity | None | Standard |
| Stop Bits | One | Standard |
| Handshake | None | No flow control |
| Read Timeout | 500ms | |
| Write Timeout | 500ms | |

---

## 7. Constraints & Assumptions

### Constraints
- Windows only (WinForms dependency)
- Requires .NET 8 Runtime installed
- Serial port limited by OS (COM1, COM2, ...)
- RichTextBox has performance limitations with very large text

### Assumptions
- User has basic knowledge of serial communication
- UART device is physically connected (USB-to-UART cable)
- COM port driver is installed

---

## 8. Future Considerations (v2.0+)

| Feature | Description | Priority |
|---------|--------|----------|
| Baud rate selection | Dropdown to select baud rate | P1 |
| HEX display mode | Display data in HEX format | P1 |
| HEX send mode | Send data in HEX format | P1 |
| Auto-reconnect | Auto reconnect when port reconnects | P2 |
| Search/Filter log | Search within log display | P2 |
| Timestamp ON/OFF | Toggle timestamp display | P2 |
| Multiple port support | Open multiple ports simultaneously | P3 |
| Dark theme | Dark mode UI | P3 |
| Export filtered log | Export filtered log | P3 |
| Config persistence | Save config between sessions | P2 |