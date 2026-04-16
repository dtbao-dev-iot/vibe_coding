# AppLog - UART Serial Log Reader

<p align="center">
  <strong>A Windows desktop application for reading, displaying, and logging UART serial port data.</strong>
</p>

---

## Features

- **Serial Port Management** — Select COM port and baud rate (9600–921600, default 115200)
- **Real-time Log Display** — Color-coded TX (Blue) / RX (Green) with timestamp `[HH:MM:SS.SSS]`
- **Send ASCII Data** — Send commands via UART with Enter key support
- **Save Log to File** — Thread-safe file logging with `Log_DDMMYYYY_HHMM.log` format
- **USB Disconnect Detection** — Auto-recovery when device is unplugged
- **Responsive UI** — Resizable window with anchored controls (min 600×400)

## Screenshots

```
┌─────────────────────────────────────────────────────────┐
│  AppLog v0.1.0                                          │
├─────────────────────────────────────────────────────────┤
│  Port Config                                            │
│  [COM3 ▼] [115200 ▼]  [Update Port]  [Open]             │
├─────────────────────────────────────────────────────────┤
│  Log Display                                            │
│  [14:30:25.123] [TX] AT+RESET                           │
│  [14:30:25.456] [RX] OK                                 │
│  [14:30:26.001] [RX] System ready         [Clear][Copy] │
├─────────────────────────────────────────────────────────┤
│  Send Data                                              │
│  [Enter ASCII data_______________]  [Send]              │
├─────────────────────────────────────────────────────────┤
│  Log File                                               │
│  [Start Log]                                            │
├─────────────────────────────────────────────────────────┤
│  ● Connected | COM COM3 | Baud 115200 | Not logging     │
└─────────────────────────────────────────────────────────┘
```

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Language | C# 12 (.NET 8) |
| UI Framework | Windows Forms (WinForms) |
| Serial Communication | `System.IO.Ports` (NuGet) |
| Testing | xUnit |
| CI/CD | GitHub Actions |

## Prerequisites

- **.NET 8 SDK** — [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Windows 10/11** — WinForms requires Windows
- **UART Device** — USB-to-UART cable with COM port driver installed

## Build Instructions

### 1. Clone the repository

```bash
git clone https://github.com/dtbao-dev-iot/vibe_coding.git
cd vibe_coding
```

### 2. Build

```bash
# Debug build
dotnet build src/AppLog.sln

# Release build
dotnet build src/AppLog.sln --configuration Release
```

### 3. Run

```bash
dotnet run --project src/AppLog/AppLog.csproj
```

### 4. Run Tests

```bash
dotnet test src/AppLog.sln --verbosity normal
```

### 5. Publish (self-contained executable)

```bash
dotnet publish src/AppLog/AppLog.csproj \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  --output ./publish
```

### 6. Full Build Pipeline (PowerShell)

```powershell
.\scripts\build.ps1 -Configuration Release
```

## Project Structure

```
vibe_coding/
├── src/
│   ├── AppLog.sln                  # Solution file
│   └── AppLog/
│       ├── AppLog.csproj           # Project file (.NET 8 WinForms)
│       ├── Program.cs              # Entry point
│       ├── Forms/
│       │   ├── MainForm.cs         # Main form logic
│       │   └── MainForm.Designer.cs # UI controls layout
│       ├── Models/
│       │   └── LogEntry.cs         # LogEntry + LogDirection enum
│       ├── Services/
│       │   ├── SerialPortService.cs # UART communication
│       │   └── LogFileService.cs    # File logging
│       └── Helpers/
│           └── LogDisplayHelper.cs  # RichTextBox display helper
├── tests/
│   └── AppLog.Tests/               # xUnit test project
├── docs/                           # Documentation
├── scripts/                        # Build & utility scripts
├── CLAUDE.md                       # AI project guide
└── README.md                       # This file
```

## Serial Port Configuration

| Parameter | Value |
|-----------|-------|
| Baud Rate | 9600, 19200, 38400, 57600, **115200** (default), 230400, 460800, 921600 |
| Data Bits | 8 |
| Parity | None |
| Stop Bits | 1 |
| Handshake | None |

## Documentation

| File | Description |
|------|-------------|
| `docs/PRD.md` | Product Requirements Document |
| `docs/project-structure.md` | Detailed project structure |
| `docs/implementation-plan.md` | Development roadmap (Phase 1–7) |
| `docs/style-coding.md` | Coding standards |
| `docs/commit-convention.md` | Commit message conventions |
| `docs/version-convention.md` | Versioning rules (SemVer) |
| `docs/changelog-convention.md` | Changelog creation rules |
| `CLAUDE.md` | AI project guide with docs mapping |

## License

This project is proprietary software. All rights reserved.