# CLAUDE.md - AppLog Project Guide

> **Project:** AppLog - UART Serial Log Reader  
> **Tech Stack:** C# 12 / .NET 8 / WinForms  
> **Last Updated:** 2026-04-15

---

## Project Overview

**AppLog** is a Windows desktop application (C# WinForms .NET 8) for reading and displaying log data from UART serial ports. The application supports sending ASCII data to devices, displaying real-time logs with TX/RX color distinction, and saving logs to files.

### Key Facts
- **Solution:** `src/AppLog.sln` (2 projects)
- **Main Project:** `src/AppLog/AppLog.csproj` (WinForms app)
- **Test Project:** `tests/AppLog.Tests/AppLog.Tests.csproj` (xUnit)
- **Current Version:** 0.1.0
- **Target:** .NET 8.0 Windows (x64)
- **Serial Library:** `System.IO.Ports` v8.0.0 (NuGet)

---

## Docs Folder - File Mapping

| File | Content | Purpose |
|------|---------|---------|
| `docs/PRD.md` | **Product Requirement Document** - Detailed 5 features (F1-F5), UI wireframe, data flow, non-functional requirements, serial config, constraints | Read when you need to understand **feature requirements**, **acceptance criteria**, or **UI layout** |
| `docs/project-structure.md` | **Project Structure** - Directory tree diagram, description of each file/folder, 3-layer architecture (UI/Services/Models) | Read when you need to understand **code organization**, find files, or add new modules |
| `docs/style-coding.md` | **Coding Standards** - Naming convention, file naming, code formatting, Doxygen documentation rules, struct definitions | Read when **writing new code** or **reviewing code** to ensure standards compliance |
| `docs/commit-convention.md` | **Commit Convention** - Conventional Commits format, commit rules, scope, .pre-commit-config.yaml | Read when **committing code** or configuring **pre-commit hooks** |
| `docs/version-convention.md` | **Version Convention** - SemVer rules, version bump rules, pre-release, version in .csproj, git tag, branching strategy | Read when you need to **bump version** or decide MAJOR/MINOR/PATCH |
| `docs/changelog-convention.md` | **Changelog Convention** - Changelog creation rules, change assessment to version bump, sections, real examples | Read when you need to **create changelog** for a new version or **assess commits** |
| `docs/changelog-template.md` | **Changelog Template** - Template for each version changelog | Use when **creating a new CHANGELOG file** |
| `docs/IMPLEMENTATION_PLAN.md` | **Implementation Plan** - Development roadmap by phases, milestone timeline | Read when you need to know **implementation order** or **development roadmap** |

---

## Architecture

```
┌─────────────────────────────────────────────────────┐
│                    UI Layer                          │
│  Forms/MainForm.cs + MainForm.Designer.cs           │
│  (Event handlers, UI state management)              │
├─────────────────────────────────────────────────────┤
│                  Helpers Layer                       │
│  Helpers/LogDisplayHelper.cs                        │
│  (RichTextBox color-coded display, buffer mgmt)     │
├─────────────────────────────────────────────────────┤
│                 Services Layer                       │
│  Services/SerialPortService.cs  (UART communication)│
│  Services/LogFileService.cs     (File logging)      │
├─────────────────────────────────────────────────────┤
│                 Models Layer                         │
│  Models/LogEntry.cs  (LogDirection enum + LogEntry)  │
└─────────────────────────────────────────────────────┘
```

### Data Flow
```
UART Device ←→ SerialPortService ←→ MainForm ←→ LogDisplayHelper → RichTextBox
                                    ↕
                              LogFileService → logs/Log_DDMMYYYY_HHMM.log
```

---

## Source Code Structure

```
src/AppLog/
├── AppLog.csproj              # Project file (.NET 8 WinForms, System.IO.Ports)
├── Program.cs                 # Entry point → Application.Run(MainForm)
├── Forms/
│   ├── MainForm.cs            # Main form logic (event handlers, services wiring)
│   └── MainForm.Designer.cs   # UI controls layout (ComboBox, RichTextBox, Buttons)
├── Models/
│   └── LogEntry.cs            # LogEntry class + LogDirection enum
├── Services/
│   ├── SerialPortService.cs   # Serial port open/close/read/write
│   └── LogFileService.cs      # File logging start/stop/write
├── Helpers/
│   └── LogDisplayHelper.cs    # RichTextBox color append, buffer trim, auto-scroll
└── Properties/                # Assembly properties

tests/AppLog.Tests/
├── AppLog.Tests.csproj        # xUnit test project (references AppLog)
├── Models/
│   └── LogEntryTests.cs       # LogEntry.ToString() tests
└── Services/
    └── LogFileServiceTests.cs # LogFileService state management tests
```

---

## Build & Run Commands

```bash
# Build
dotnet build src/AppLog.sln --configuration Release

# Run
dotnet run --project src/AppLog/AppLog.csproj

# Test
dotnet test src/AppLog.sln --verbosity normal

# Format
dotnet format src/AppLog.sln

# Build script (full pipeline)
.\scripts\build.ps1 -Configuration Release
```

---

## Key Implementation Details

### Serial Port Configuration (Default)
- Baud Rate: 115200
- Data Bits: 8, Parity: None, Stop Bits: One
- Read/Write Timeout: 500ms

### Log Format
```
[HH:MM:SS.SSS] [TX] data_sent_to_uart     (Blue)
[HH:MM:SS.SSS] [RX] data_received_from_uart (Green)
```

### Log File
- Format: `Log_DDMMYYYY_HHMM.log`
- Location: `logs/` folder (relative to executable)
- Thread-safe writing with `lock()` and `Flush()` after each write

### UI Controls
| Control | Type | Function |
|---------|------|----------|
| `cmbPort` | ComboBox | Select COM port |
| `btnUpdatePort` | Button | Refresh port list |
| `btnOpenClose` | Button | Toggle Open/Close port |
| `rtbLogDisplay` | RichTextBox | Display color-coded log (max 1MB buffer) |
| `btnClearLog` | Button | Clear log display |
| `btnCopyLog` | Button | Copy log to clipboard |
| `txtSendData` | TextBox | Input ASCII data (Enter to send) |
| `btnSend` | Button | Send data through UART |
| `btnStartStopLog` | Button | Toggle Start/Stop file logging |
| `lblStatus` | Label | Status bar (Connected/Disconnected, Logging) |

---

## CI/CD

| Workflow | Trigger | Action |
|----------|---------|--------|
| `.github/workflows/ci.yml` | Push to main/develop, PR to main | Build + Test + Format check |
| `.github/workflows/release.yml` | Push tag `v*` | Build + Publish + GitHub Release + ZIP artifact |

---

## Scripts

| Script | Usage | Description |
|--------|-------|-------------|
| `scripts/build.ps1` | `.\scripts\build.ps1 -Configuration Release` | Full build pipeline (restore → build → test → format) |
| `scripts/version-bump.ps1` | `.\scripts\version-bump.ps1 -BumpType "minor"` | Bump version in .csproj + optional git tag |
| `scripts/commit-msg-check.py` | `python scripts/commit-msg-check.py .git/COMMIT_EDITMSG` | Validate commit message format |

---

## Development Workflow

1. **Create branch** from `develop`: `git checkout -b feature/xxx`
2. **Code** following `docs/style-coding.md`
3. **Commit** following `docs/commit-convention.md`: `feat(scope): description`
4. **Build & Test**: `.\scripts\build.ps1`
5. **Push & PR** → CI runs automatically
6. **Review & Merge** into develop
7. **Release** following `docs/version-convention.md` and `docs/changelog-convention.md`

---

## Maintenance Notes

### Adding a New Feature
1. Read `docs/PRD.md` for requirements
2. Create files following `docs/project-structure.md` structure
3. Code following `docs/style-coding.md`
4. Commit following `docs/commit-convention.md`
5. Version bump following `docs/version-convention.md`

### Changing Version
1. Assess commits: `git log <last-tag>..HEAD --oneline`
2. Decide bump type: see `docs/changelog-convention.md`
3. Run: `.\scripts\version-bump.ps1 -BumpType "minor" -CreateTag`
4. Create changelog from `docs/changelog-template.md`

### Troubleshooting
- **Build failed**: Check `dotnet restore` → `dotnet build`
- **Test failed**: Check test output → fix → re-run
- **Format check failed**: Run `dotnet format src/AppLog.sln`
- **Pre-commit hook failed**: Check commit message format

---

## Project Status

| Milestone | Status | Version |
|-----------|--------|---------|
| M1: Project Skeleton | ✅ Complete | v0.1.0 |
| M2: Serial Port Open/Close | 🔲 Planned | v0.2.0 |
| M3: Log Display | 🔲 Planned | v0.3.0 |
| M4: Send Data | 🔲 Planned | v0.4.0 |
| M5: Save Log to File | 🔲 Planned | v0.5.0 |
| M6: Polish & Error Handling | 🔲 Planned | v0.6.0 |
| M7: First Stable Release | 🔲 Planned | v1.0.0 |