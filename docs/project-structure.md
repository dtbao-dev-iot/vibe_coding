# Project Structure - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Tech Stack:** C# .NET 8 WinForms  
> **Last Updated:** 2026-04-15

---

## Directory Tree

```
vibe_coding/
│
├── .github/                              # GitHub Actions CI/CD
│   └── workflows/
│       ├── ci.yml                        # Continuous Integration pipeline
│       └── release.yml                   # Release pipeline (build + publish)
│
├── .editorconfig                         # Editor formatting rules
├── .gitignore                            # Git ignore rules
├── .pre-commit-config.yaml               # Pre-commit hooks config
├── CLAUDE.md                             # AI assistant mapping & project analysis
│
├── docs/                                 # Documentation
│   ├── project-structure.md              # ← This file: project structure description
│   ├── commit-convention.md              # Commit message convention
│   ├── style-coding.md                   # Coding style & naming convention
│   ├── version-convention.md             # Semantic Versioning rules
│   ├── changelog-convention.md           # Changelog creation rules
│   ├── changelog-template.md             # Changelog template file
│   ├── PRD.md                            # Product Requirement Document
│   ├── IMPLEMENTATION_PLAN.md            # Implementation plan 7 phases
│   └── CHANGELOG/                        # Per-version changelog files
│       └── CHANGELOG-0.1.0.md            # Changelog for v0.1.0
│
├── src/                                  # Source code
│   ├── AppLog.sln                        # Visual Studio Solution file
│   └── AppLog/                           # Main application project
│       ├── AppLog.csproj                 # Project file (.NET 8 WinForms)
│       ├── Program.cs                    # Entry point
│       │
│       ├── Properties/                   # Assembly & resource properties
│       │
│       ├── Forms/                        # WinForms UI layer
│       │   ├── MainForm.cs               # Main form logic (event handlers, UI updates)
│       │   └── MainForm.Designer.cs      # Auto-generated UI designer code
│       │
│       ├── Models/                       # Data models
│       │   └── LogEntry.cs               # Log entry model (Timestamp, Direction, Data)
│       │
│       ├── Services/                     # Business logic layer
│       │   ├── SerialPortService.cs      # Serial port management (open/close/read/write)
│       │   └── LogFileService.cs         # Log file management (start/stop/write)
│       │
│       └── Helpers/                      # Utility classes
│           └── LogDisplayHelper.cs       # RichTextBox display utilities
│
├── tests/                                # Unit tests
│   └── AppLog.Tests/                     # Test project
│       ├── AppLog.Tests.csproj           # Test project file (xUnit)
│       ├── Services/
│       │   └── LogFileServiceTests.cs    # Tests for LogFileService
│       └── Models/
│           └── LogEntryTests.cs          # Tests for LogEntry model
│
├── scripts/                              # Build & utility scripts
│   ├── build.ps1                         # Build script (PowerShell)
│   ├── version-bump.ps1                  # Version bump script
│   └── commit-msg-check.py              # Commit message validator (pre-commit hook)
│
└── assets/                               # Static assets
    └── icons/                            # Application icons
```

---

## File & Folder Descriptions

### Root Config Files

| File | Purpose |
|------|---------|
| `.editorconfig` | Defines formatting rules for editors (indent, charset, naming conventions for C#) |
| `.gitignore` | Ignore build outputs, user files, IDE cache, log files |
| `.pre-commit-config.yaml` | Configure pre-commit hooks: trailing whitespace, end-of-file, dotnet format, commit message check |
| `CLAUDE.md` | Mapping between docs and project, comprehensive analysis for AI assistant maintenance |

### `.github/workflows/`

| File | Purpose |
|------|---------|
| `ci.yml` | Runs on every push/PR: `dotnet restore` → `build` → `test` → `dotnet format --verify-no-changes` |
| `release.yml` | Runs on push tag `v*`: build → publish (self-contained .exe) → create GitHub Release → upload artifact |

### `docs/`

| File | Purpose |
|------|---------|
| `project-structure.md` | Directory structure, file descriptions, component functionality |
| `commit-convention.md` | Conventional Commits standard: `feat:`, `fix:`, `docs:`, `refactor:` + scope + breaking changes |
| `style-coding.md` | Naming convention, formatting rules, Doxygen documentation, struct definitions |
| `version-convention.md` | Semantic Versioning: MAJOR.MINOR.PATCH, when to bump, branching strategy |
| `changelog-convention.md` | How to create changelog from git log, assess changes → version bump |
| `changelog-template.md` | Template format for CHANGELOG files |
| `PRD.md` | Product Requirement Document: features, user stories, UI wireframe, tech stack |
| `IMPLEMENTATION_PLAN.md` | Detailed 7-phase implementation plan with tasks and Definition of Done |

### `src/AppLog/`

| File/Folder | Purpose |
|-------------|---------|
| `AppLog.csproj` | .NET 8 WinForms project config, package references, version info |
| `Program.cs` | Application entry point: `ApplicationConfiguration.Initialize()`, `Application.Run(new MainForm())` |
| `Properties/` | Assembly metadata, resources, user settings |
| `Forms/` | WinForms UI layer - MainForm with port config, log display, send area, log file controls |
| `Models/` | Data models - `LogEntry` contains Timestamp, Direction (TX/RX), Data |
| `Services/` | Business logic - `SerialPortService` manages UART, `LogFileService` manages log files |
| `Helpers/` | Utility classes - `LogDisplayHelper` for RichTextBox color-coded display |

### `tests/AppLog.Tests/`

| File | Purpose |
|------|---------|
| `AppLog.Tests.csproj` | xUnit test project, references AppLog project |
| `Services/LogFileServiceTests.cs` | Test start/stop logging, file format, thread-safe writes |
| `Models/LogEntryTests.cs` | Test LogEntry model, ToString() format |

### `scripts/`

| File | Purpose |
|------|---------|
| `build.ps1` | PowerShell script: restore → build → test, supports Release/Debug config |
| `version-bump.ps1` | Script to bump version in .csproj, create git tag |
| `commit-msg-check.py` | Python script to validate commit message following Conventional Commits format |

### `assets/`

| Folder | Purpose |
|--------|---------|
| `icons/` | Application icons (.ico, .png) for form title bar and executable |

---

## Architecture

```
┌──────────────────────────────────────────────────┐
│                    UI Layer                      │
│              Forms / MainForm.cs                 │
│  (Event handlers, UI updates, user interaction)  │
└──────────────┬──────────────────┬────────────────┘
               │                  │
               ▼                  ▼
┌──────────────────────┐  ┌──────────────────────┐
│    Services Layer    │  │    Helpers Layer     │
│  SerialPortService   │  │  LogDisplayHelper    │
│  LogFileService      │  │                      │
└──────────┬───────────┘  └──────────────────────┘
           │
           ▼
┌──────────────────────┐
│     Models Layer     │
│     LogEntry         │
│  (Timestamp, Dir,    │
│       Data)          │
└──────────────────────┘
```

**Data Flow:**
1. UART Serial Data → `SerialPortService.DataReceived` → `LogEntry` (RX)
2. User Send ASCII → `SerialPortService.SendData()` → UART + `LogEntry` (TX)
3. `LogEntry` → Event `OnLogEntryReceived` → MainForm displays + LogFileService writes to file