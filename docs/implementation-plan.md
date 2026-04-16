# Implementation Plan - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Tech Stack:** C# .NET 8 WinForms  
> **Last Updated:** 2026-04-16

---

## Overview

The Implementation Plan is divided into **7 phases**, each with clear objectives, detailed task lists, and corresponding outputs.

---

## Phase 1: Project Skeleton & Documentation

**Objective:** Set up the entire project structure, documentation, and CI/CD pipeline.

| # | Task | Details | Output |
|---|------|---------|--------|
| 1.1 | Create solution structure | `AppLog.sln`, `AppLog.csproj` (.NET 8 WinForms) | `src/AppLog.sln`, `src/AppLog/AppLog.csproj` |
| 1.2 | Create entry point | `Program.cs` with `Application.Run(new MainForm())` | `src/AppLog/Program.cs` |
| 1.3 | Create Models | `LogEntry.cs` (Timestamp, Direction TX/RX, Data) | `src/AppLog/Models/LogEntry.cs` |
| 1.4 | Create Services skeleton | `SerialPortService.cs`, `LogFileService.cs` (interface + empty methods) | `src/AppLog/Services/` |
| 1.5 | Create Helpers | `LogDisplayHelper.cs` (color-coded display, buffer management) | `src/AppLog/Helpers/` |
| 1.6 | Create test project | `AppLog.Tests.csproj` + skeleton test classes | `tests/AppLog.Tests/` |
| 1.7 | Create all docs | 8 markdown files in `docs/` | `docs/*.md` |
| 1.8 | Create CLAUDE.md | Docs mapping + project analysis | `CLAUDE.md` |
| 1.9 | Create CI/CD | GitHub Actions: ci.yml + release.yml | `.github/workflows/` |
| 1.10 | Create scripts | build.ps1, version-bump.ps1, commit-msg-check.py | `scripts/` |
| 1.11 | Create config files | .gitignore, .editorconfig, .pre-commit-config.yaml | root |
| 1.12 | Verify build | `dotnet build` succeeds | - |

**Definition of Done:**
- [x] `dotnet build` runs successfully
- [x] `dotnet test` runs successfully (28 tests pass)
- [x] CI pipeline runs green on GitHub Actions

---

## Phase 2: Serial Port Core

**Objective:** Implement port open/close, read/write UART serial data.

| # | Task | Details | Output |
|---|------|---------|--------|
| 2.1 | Implement `SerialPortService` | Initialize `System.IO.Ports.SerialPort`, configure baud rate, parity, data bits, stop bits | `SerialPortService.cs` |
| 2.2 | Method `GetAvailablePorts()` | Use `SerialPort.GetPortNames()` to return list of available COM ports | `SerialPortService.cs` |
| 2.3 | Method `OpenPort(portName)` | Open port with standard config, start listening to `DataReceived` event | `SerialPortService.cs` |
| 2.4 | Method `ClosePort()` | Close port safely, unsubscribe events, dispose resources | `SerialPortService.cs` |
| 2.5 | Event `DataReceived` handler | Read data from UART buffer, create `LogEntry` with Direction=RX, current Timestamp | `SerialPortService.cs` |
| 2.6 | Method `SendData(string asciiData)` | Convert ASCII string → byte array, write to SerialPort, create `LogEntry` Direction=TX | `SerialPortService.cs` |
| 2.7 | Event `OnLogEntryReceived` | Custom event `EventHandler<LogEntry>` raised when a new log entry is created (TX or RX) | `SerialPortService.cs` |
| 2.8 | Implement `LogEntry` model | Class: `DateTime Timestamp`, `enum Direction { TX, RX }`, `string Data`, method `ToString()` format `[HH:MM:SS.SSS] [TX/RX] data` | `LogEntry.cs` |
| 2.9 | Unit test SerialPortService | Mock SerialPort, test open/close/send, verify events fired | `AppLog.Tests/Services/SerialPortServiceTests.cs` |

**Definition of Done:**
- [x] Open/Close port works
- [x] Send data via UART succeeds
- [x] Receive data from UART fires correct event
- [x] Unit tests pass (17 SerialPortService tests)

---

## Phase 3: UI - MainForm Layout & Log Display

**Objective:** Display complete UI with real-time log from UART.

### UI Layout:

```
┌─────────────────────────────────────────────────────────┐
│  Port Config Area                                        │
│  [ComboBox: COM Port ▼]  [Update Port]  [Open/Close]    │
├─────────────────────────────────────────────────────────┤
│  Log Display (RichTextBox, max 1MB)                      │
│  [14:30:25.123] [TX] AT+RESET                           │
│  [14:30:25.456] [RX] OK                                  │
│  [14:30:26.001] [RX] System ready                        │
│  ...                                                     │
│                                          [Clear] [Copy]  │
├─────────────────────────────────────────────────────────┤
│  Send Area                                               │
│  [TextBox: ASCII Input___________]  [Send]              │
├─────────────────────────────────────────────────────────┤
│  Log File                                                │
│  [Start Log / Stop Log]                                  │
├─────────────────────────────────────────────────────────┤
│  Status Bar: Connected | COM3 | Logging to: Log_15042026_1430.log │
└─────────────────────────────────────────────────────────┘
```

### Tasks:

| # | Task | Details | Output |
|---|------|---------|--------|
| 3.1 | Create MainForm layout | TableLayoutPanel with 5 areas: Port Config, Log Display, Send Area, Log File, Status Bar | `MainForm.Designer.cs` |
| 3.2 | Port Config area | ComboBox `cmbPort`, Button `btnUpdatePort`, Button `btnOpenClose` (text: "Open") | `MainForm.Designer.cs` |
| 3.3 | Update Port logic | `btnUpdatePort_Click` → call `GetAvailablePorts()` → update ComboBox items | `MainForm.cs` |
| 3.4 | Load ports on startup | `MainForm_Load` → automatically call Update Port to populate list | `MainForm.cs` |
| 3.5 | Open/Close logic | `btnOpenClose_Click` → toggle text "Open" ↔ "Close", call `OpenPort()`/`ClosePort()`, set `cmbPort.Enabled` and `btnUpdatePort.Enabled` | `MainForm.cs` |
| 3.6 | Log Display area | RichTextBox `rtbLogDisplay` for log display, readonly = true | `MainForm.Designer.cs` |
| 3.7 | Log display format | Format: `[HH:MM:SS.SSS] [TX] data\r\n` or `[HH:MM:SS.SSS] [RX] data\r\n` | `MainForm.cs` |
| 3.8 | Log color coding | TX = Blue, RX = Green, Timestamp = Gray | `MainForm.cs` |
| 3.9 | Subscribe `OnLogEntryReceived` | When SerialPortService fires event → append log entry to RichTextBox, auto-scroll to bottom | `MainForm.cs` |
| 3.10 | Clear Log button | `btnClearLog_Click` → `rtbLogDisplay.Clear()` | `MainForm.cs` |
| 3.11 | Copy Log button | `btnCopyLog_Click` → `Clipboard.SetText(rtbLogDisplay.Text)` | `MainForm.cs` |
| 3.12 | 1MB buffer limit | Check `rtbLogDisplay.TextLength`, if > 1,048,576 chars → remove oldest lines (find first `\n`, remove up to that point) | `MainForm.cs` |

**Definition of Done:**
- [x] UI displays correct layout
- [x] Open/Close port toggles UI state correctly
- [x] Log displays real-time with correct colors
- [x] Auto-scroll works
- [x] 1MB buffer limit works

---

## Phase 4: Send Data

**Objective:** Enter ASCII data and send via UART port.

| # | Task | Details | Output |
|---|------|---------|--------|
| 4.1 | Send area UI | TextBox `txtSendData` (multiline=false) + Button `btnSend` | `MainForm.Designer.cs` |
| 4.2 | Send logic | `btnSend_Click` → call `_serialPortService.SendData(txtSendData.Text)` → keep text in input | `MainForm.cs` |
| 4.3 | Send on Enter key | `txtSendData_KeyDown` → if Enter → trigger `btnSend_Click` | `MainForm.cs` |
| 4.4 | Enable/Disable Send | Button `btnSend` only enabled when port is Open, disabled when Closed | `MainForm.cs` |
| 4.5 | TX log display | After send, TX entry automatically displays in log display via `OnLogEntryReceived` event | `MainForm.cs` |
| 4.6 | Empty data check | Do not send if `txtSendData.Text` is empty | `MainForm.cs` |

**Definition of Done:**
- [x] Send ASCII data via UART successfully
- [x] TX entry displays in log display
- [x] Enter key triggers send
- [x] Text is preserved after send
- [x] Disabled when port is closed

---

## Phase 5: Save Log to File

**Objective:** Write log to file with standard format, prioritize no data loss.

### Log File Format:
```
File: logs/Log_DDMMYYYY_HHMM.log
Example: logs/Log_15042026_1430.log

Content:
[14:30:25.123] [TX] AT+RESET
[14:30:25.456] [RX] OK
[14:30:26.001] [RX] System ready
```

### Tasks:

| # | Task | Details | Output |
|---|------|---------|--------|
| 5.1 | Implement `LogFileService` | Class managing file stream, write log entries, thread-safe | `LogFileService.cs` |
| 5.2 | Method `StartLogging()` | Create `logs/` folder if not exists, create file `Log_DDMMYYYY_HHMM.log`, open StreamWriter | `LogFileService.cs` |
| 5.3 | Method `StopLogging()` | Flush + Close StreamWriter, dispose resources safely | `LogFileService.cs` |
| 5.4 | Method `WriteEntry(LogEntry)` | Write entry to file: `[HH:MM:SS.SSS] [TX/RX] data\r\n`, use lock for thread-safety | `LogFileService.cs` |
| 5.5 | Property `IsLogging` | bool flag indicating whether logging is active | `LogFileService.cs` |
| 5.6 | Start/Stop Log UI | Button `btnStartStopLog` toggle text "Start Log" ↔ "Stop Log" | `MainForm.Designer.cs` |
| 5.7 | Integrate log subscription | When `IsLogging = true` → subscribe `OnLogEntryReceived` → call `WriteEntry()`. When Stop → unsubscribe | `MainForm.cs` |
| 5.8 | Priority write | Use `lock(object)` + `StreamWriter.Flush()` after each write to ensure data is written immediately | `LogFileService.cs` |
| 5.9 | Status bar update | Display current log file name in status bar | `MainForm.cs` |
| 5.10 | Unit test LogFileService | Test start/stop/write, verify file content matches format, test concurrent writes | `AppLog.Tests/Services/LogFileServiceTests.cs` |

**Definition of Done:**
- [x] Start/Stop logging toggle works correctly
- [x] Log file created with correct name format
- [x] File content has correct format
- [x] No data loss during continuous logging
- [x] Thread-safe write

---

## Phase 6: Polish & UX

**Objective:** Refine UX, error handling, and edge cases.

| # | Task | Details | Output |
|---|------|---------|--------|
| 6.1 | Error handling - Port | Handle: port not found, port in use, unexpected disconnect → show MessageBox | `SerialPortService.cs`, `MainForm.cs` |
| 6.2 | Error handling - File | Handle: folder creation failed, disk full, permission denied → show MessageBox | `LogFileService.cs`, `MainForm.cs` |
| 6.3 | Status bar | Display: Connected/Disconnected state, port name, Logging state, log file name | `MainForm.Designer.cs` |
| 6.4 | Form resize | Anchor/Dock controls for responsive resize, minimum size = 600x400 | `MainForm.Designer.cs` |
| 6.5 | App icon | Add icon for application and form title bar (pending: no icon file) | `Properties/Resources.resx` |
| 6.6 | Form title | `this.Text = "AppLog v{version}"` | `MainForm.cs` |
| 6.7 | Port disconnected handling | When USB unplugged → auto close port, enable UI, notify user. Timer-based polling (1s) checks port existence + `ErrorOccurred` event. Auto-switches button to "Open" | `MainForm.cs` |
| 6.8 | Config persistence | Save last used port name to `Properties.Settings.Default` (optional) | `Properties/Settings.settings` |

**Definition of Done:**
- [x] No crash in any edge case
- [x] User-friendly error messages
- [x] UI responsive on resize
- [x] Status bar displays correct state

---

## Phase 7: Release v1.0.0

**Objective:** Official first release.

| # | Task | Details | Output |
|---|------|---------|--------|
| 7.1 | Final testing | Test complete end-to-end flow: open port → send → receive → display → save log → close | Test report |
| 7.2 | Update version | Set `<Version>1.0.0</Version>` in `AppLog.csproj` | `AppLog.csproj` |
| 7.3 | Create CHANGELOG.md | From `changelog-template.md`, list all v1.0.0 features | `docs/CHANGELOG/CHANGELOG-1.0.0.md` |
| 7.4 | Git tag | `git tag -a v1.0.0 -m "Release v1.0.0"` → `git push origin v1.0.0` | Git tag |
| 7.5 | Verify GitHub Release | Check GitHub Actions `release.yml` builds successfully, download `AppLog_v1.0.0.zip` | GitHub Release page |
| 7.6 | Smoke test release | Download artifact from GitHub Release, run on clean machine, verify functionality | - |

**Definition of Done:**
- [x] All phases 1-6 completed
- [x] Version tag created and pushed
- [ ] GitHub Release published with artifact
- [ ] Smoke test passes

---

## Estimated Timeline

| Phase | Estimated Time | Dependencies |
|-------|---------------|--------------|
| Phase 1: Skeleton | 1-2 hours | - |
| Phase 2: Serial Core | 2-3 hours | Phase 1 |
| Phase 3: UI & Log Display | 3-4 hours | Phase 2 |
| Phase 4: Send Data | 1-2 hours | Phase 3 |
| Phase 5: Save Log | 2-3 hours | Phase 3 |
| Phase 6: Polish | 2-3 hours | Phase 4, 5 |
| Phase 7: Release | 1 hour | Phase 6 |
| **Total** | **~12-18 hours** | |