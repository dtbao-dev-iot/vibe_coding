# Changelog - v1.0.0

> **Version:** 1.0.0  
> **Date:** 2026-04-16  
> **Status:** First Official Release

---

## [1.0.0] - 2026-04-16

### Added

- **(serial):** SerialPortService with full UART communication support (open/close/read/write)
- **(serial):** Configurable baud rate selection (9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600)
- **(serial):** Automatic port disconnection detection via 1-second polling timer
- **(serial):** ErrorOccurred event for handling serial port errors and unexpected disconnects
- **(ui):** MainForm with 5-section layout: Port Config, Log Display, Send Area, Log File, Status Bar
- **(ui):** RichTextBox log display with color-coded entries (TX=Blue, RX=Green, Timestamp=Gray)
- **(ui):** Auto-scroll to bottom on new log entries
- **(ui):** 1MB buffer limit with automatic old entry trimming
- **(ui):** Clear Log and Copy Log buttons
- **(ui):** Responsive form layout with anchor/dock controls (minimum size 600x400)
- **(ui):** Status bar showing connection state, port name, baud rate, and logging state
- **(send):** ASCII data sending via UART with Enter key support
- **(send):** Input validation - empty data check, port open check
- **(send):** Send controls disabled when port is closed
- **(log):** LogFileService for thread-safe file logging with immediate flush
- **(log):** Log file naming format: `Log_DDMMYYYY_HHMM.log` in `logs/` folder
- **(log):** Start/Stop logging toggle with UI button
- **(model):** LogEntry class with Timestamp, Direction (TX/RX), Data
- **(model):** LogEntry.ToString() format: `[HH:MM:SS.SSS] [TX/RX] data`
- **(helper):** LogDisplayHelper for color-coded RichTextBox append and buffer management
- **(error):** Comprehensive error handling for port not found, port in use, disk full, permission denied
- **(error):** User-friendly error messages via MessageBox
- **(error):** Graceful handling of USB unplugged scenario with auto-recovery
- **(build):** Form title displays application version from assembly
- **(ci):** GitHub Actions CI pipeline (build, test, format check)
- **(ci):** GitHub Actions Release pipeline (publish, create release, upload artifact)
- **(build):** PowerShell build script with restore, build, test, format steps
- **(build):** Version bump script with SemVer support
- **(build):** Commit message checker for Conventional Commits
- **(docs):** Complete documentation set (PRD, style guide, conventions, implementation plan)

### Version Assessment

**Current version:** 1.0.0 (First official release)  
**All planned features implemented and tested.**  
**Unit tests:** 28 tests passing  
**Build:** Release configuration, 0 warnings, 0 errors