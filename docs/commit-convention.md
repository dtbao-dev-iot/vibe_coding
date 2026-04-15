# Commit Convention - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Last Updated:** 2026-04-15

---

## Commit Message Standard

This project uses **Conventional Commits** specification (https://www.conventionalcommits.org/).

### Format

```
<type>(<scope>): <subject>

[optional body]

[optional footer(s)]
```

### Examples

```
feat(serial): add serial port open/close functionality

- Implement OpenPort() and ClosePort() methods
- Add DataReceived event handler
- Add unit tests

Closes #12
```

```
fix(log): fix 1MB buffer not trimming old entries correctly

The buffer check was comparing bytes instead of characters.
```

```
docs(prd): update PRD with send data feature requirements
```

---

## Type (Required)

| Type | Description | Version Impact |
|------|-------------|---------------|
| `feat` | New feature, new functionality | MINOR |
| `fix` | Bug fix | PATCH |
| `docs` | Documentation changes only | None |
| `style` | Formatting, missing semicolons, no logic change | None |
| `refactor` | Code refactoring, no new feature or bug fix | None |
| `perf` | Performance improvement | PATCH |
| `test` | Add or modify unit tests | None |
| `build` | Build system changes, dependencies (csproj, CI) | None |
| `ci` | CI/CD configuration changes | None |
| `chore` | Maintenance tasks, not related to src or tests | None |
| `revert` | Revert a previous commit | Varies |

## Scope (Optional but Recommended)

| Scope | Description |
|-------|-------------|
| `serial` | SerialPortService, UART communication |
| `log` | LogFileService, log display, buffer |
| `ui` | MainForm, UI controls, layout |
| `model` | LogEntry, data models |
| `helper` | LogDisplayHelper, utility classes |
| `docs` | Documentation files |
| `ci` | CI/CD pipeline |
| `build` | Build scripts, project config |

## Subject (Required)

- Write in **English**
- Use **imperative mood** ("add", not "added" or "adds")
- Do not capitalize first letter
- Do not end with a period
- Limit to **72 characters**

## Body (Optional)

- Describe **why** the change was made (not **what** - see diff)
- Separate from subject with a blank line
- Limit each line to **100 characters**

## Footer (Optional)

- **Breaking Changes:** `BREAKING CHANGE: <description>` → MAJOR bump
- **Issue references:** `Closes #123`, `Fixes #456`
- **Co-authors:** `Co-authored-by: Name <email>`

---

## Breaking Changes

There are 2 ways to mark a breaking change:

### Method 1: `!` after type

```
feat(serial)!: change SerialPortService API to use async methods

BREAKING CHANGE: OpenPort() now returns Task<bool> instead of void.
All callers must be updated to await the result.

Closes #20
```

### Method 2: Footer `BREAKING CHANGE`

```
feat(serial): add baud rate configuration support

BREAKING CHANGE: OpenPort() now requires BaudRate parameter.
Previously used default 9600.
```

---

## Rules

### ✅ Must

1. Each commit should do **one thing** (atomic commits)
2. Commit message must follow format `<type>(<scope>): <subject>`
3. Subject must be in imperative mood
4. Subject ≤ 72 characters
5. Body should describe the reason for change (if complex)
6. Add `BREAKING CHANGE:` footer if API changes are incompatible

### ❌ Must Not

1. Do not commit directly to `main` branch
2. Do not use `git push --force` on `main` or `develop`
3. Do not commit large binary files (>500KB)
4. Do not commit secrets, passwords, API keys
5. Do not use generic messages: "update", "fix bug", "changes"

---

## Git Branching Strategy

```
main          ──●──────────────────●──────────●──  (stable, tagged releases)
                   \                /            ↑
develop        ────●──●──●──●──●──●─────        v1.0.0
                      \     /
feature/xxx     ──────●──●──
```

### Branch Naming

| Pattern | Description | Example |
|---------|-------------|---------|
| `feature/<name>` | New feature | `feature/serial-port-open-close` |
| `fix/<name>` | Bug fix | `fix/buffer-overflow-log` |
| `docs/<name>` | Documentation | `docs/add-prd` |
| `refactor/<name>` | Refactoring | `refactor/service-layer` |
| `release/<version>` | Release preparation | `release/v1.0.0` |
| `hotfix/<name>` | Urgent fix on production | `hotfix/fix-crash-on-disconnect` |

---

## Pre-commit Hook

File `.pre-commit-config.yaml` configures automated checks:

| Hook | Description |
|------|-------------|
| `trailing-whitespace` | Remove trailing whitespace |
| `end-of-file-fixer` | Ensure newline at end of file |
| `check-yaml` | Validate YAML syntax |
| `check-json` | Validate JSON syntax |
| `check-merge-conflict` | Detect merge conflict markers |
| `check-added-large-files` | Block files > 500KB |
| `no-commit-to-branch` | Block direct commits to main/master |
| `dotnet-format` | Run `dotnet format` on C# files |
| `commit-msg-check` | Validate commit message format |

### Installation

```bash
# Install pre-commit
pip install pre-commit

# Install hooks into git
pre-commit install

# Install commit-msg hook
pre-commit install --hook-type commit-msg

# Run all hooks on all files (first time)
pre-commit run --all-files
```

---

## Example Commit Messages for AppLog

```bash
# Phase 1: Skeleton
git commit -m "feat(build): initialize AppLog solution with .NET 8 WinForms project"
git commit -m "feat(model): add LogEntry model with Timestamp, Direction, Data properties"
git commit -m "feat(service): add SerialPortService skeleton with interface methods"
git commit -m "feat(service): add LogFileService skeleton with interface methods"
git commit -m "docs(all): add project documentation and conventions"

# Phase 2: Serial Core
git commit -m "feat(serial): implement GetAvailablePorts using SerialPort.GetPortNames"
git commit -m "feat(serial): implement OpenPort and ClosePort with event handling"
git commit -m "feat(serial): implement SendData method with ASCII to byte conversion"
git commit -m "feat(serial): add OnLogEntryReceived event for TX/RX notifications"
git commit -m "test(serial): add unit tests for SerialPortService"

# Phase 3: UI
git commit -m "feat(ui): create MainForm layout with TableLayoutPanel"
git commit -m "feat(ui): implement port selection and open/close toggle"
git commit -m "feat(ui): add log display with color-coded TX/RX entries"
git commit -m "feat(ui): implement 1MB buffer limit for log display"
git commit -m "feat(ui): add Clear and Copy log buttons"

# Phase 4: Send Data
git commit -m "feat(ui): add ASCII input text box and Send button"
git commit -m "feat(ui): implement Send with Enter key support"

# Phase 5: Log File
git commit -m "feat(log): implement LogFileService with StartLogging/StopLogging"
git commit -m "feat(log): add thread-safe file writing with priority flush"
git commit -m "feat(ui): add Start/Stop Log button with status display"
git commit -m "test(log): add unit tests for LogFileService"

# Phase 6: Polish
git commit -m "fix(serial): handle port disconnected unexpectedly"
git commit -m "feat(ui): add status bar with connection and logging info"
git commit -m "feat(ui): add responsive layout and minimum window size"

# Phase 7: Release
git commit -m "chore(release): bump version to 1.0.0"
git commit -m "docs(changelog): add CHANGELOG for v1.0.0"