# Version Convention - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Last Updated:** 2026-04-15

---

## Semantic Versioning (SemVer)

This project follows **Semantic Versioning 2.0.0** (https://semver.org/).

### Format

```
MAJOR.MINOR.PATCH
```

| Component | Description | When to bump |
|-----------|-------------|--------------|
| **MAJOR** | Breaking changes, not backward compatible | API changes, important behavior changes |
| **MINOR** | New features, backward compatible | Adding new functionality, extending capabilities |
| **PATCH** | Bug fixes, backward compatible | Fixing bugs, performance improvements, docs |

### Examples

```
v0.1.0  → Initial skeleton, no functionality
v0.2.0  → Serial port open/close working
v0.3.0  → Log display UI
v0.4.0  → Send data via UART
v0.5.0  → Save log to file
v0.6.0  → Polish & error handling
v1.0.0  → First stable release
v1.1.0  → New feature (e.g.: baud rate config)
v1.0.1  → Bug fix (e.g.: buffer overflow)
v2.0.0  → Breaking change (e.g.: complete API overhaul)
```

---

## Version Bump Rules

### Change Assessment Rules

| Change Type | Version Bump | Example |
|-------------|-------------|---------|
| Fix crash, display bug | PATCH | 1.0.0 → 1.0.1 |
| Add button, new UI feature | MINOR | 1.0.0 → 1.1.0 |
| Add new serial feature | MINOR | 1.0.0 → 1.1.0 |
| Add new export format | MINOR | 1.0.0 → 1.1.0 |
| Change log format (cannot read old files) | MAJOR | 1.0.0 → 2.0.0 |
| Change public method signature | MAJOR | 1.0.0 → 2.0.0 |
| Change config file format | MAJOR | 1.0.0 → 2.0.0 |

### Assessment from Git Commits

```
feat(...)     → MINOR bump
fix(...)      → PATCH bump
feat(...)!    → MAJOR bump (breaking change)
perf(...)     → PATCH bump
docs(...)     → No bump
style(...)    → No bump
refactor(...) → PATCH bump (if not breaking)
test(...)     → No bump
```

---

## Pre-release Versions

### Format

```
MAJOR.MINOR.PATCH-<label>.<number>
```

| Label | Description | Example |
|-------|-------------|---------|
| `alpha` | Feature in development, may be unstable | `1.1.0-alpha.1` |
| `beta` | Feature complete, testing in progress | `1.1.0-beta.1` |
| `rc` | Release candidate, ready to release if no bugs found | `1.1.0-rc.1` |

### Precedence

```
1.0.0-alpha.1 < 1.0.0-alpha.2 < 1.0.0-beta.1 < 1.0.0-rc.1 < 1.0.0
```

---

## Version in Project

### 1. In `.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Version>0.1.0</Version>
    <AssemblyVersion>0.1.0.0</AssemblyVersion>
    <FileVersion>0.1.0.0</FileVersion>
    <Product>AppLog</Product>
    <Description>UART Serial Log Reader</Description>
  </PropertyGroup>
</Project>
```

### 2. In Form Title

```csharp
this.Text = $"AppLog v{Assembly.GetExecutingAssembly().GetName().Version}";
```

### 3. Git Tag

```bash
# Create annotated tag
git tag -a v1.0.0 -m "Release v1.0.0: First stable release"

# Push tag
git push origin v1.0.0

# Push all tags
git push origin --tags
```

---

## Branching & Version Strategy

```
                    v0.1.0    v0.2.0    v1.0.0     v1.0.1
main          ────●────────●─────────●──────────●──
                    \        /          ↑
develop        ─────●──●──●──●──●─────
                      \     /
feature/xxx     ───────●──●───
```

### Rules

| Branch | Version | Note |
|--------|---------|------|
| `develop` | `0.x.y` or pre-release | Development version |
| `feature/*` | N/A | Merge into develop |
| `release/x.y.z` | Release candidate | Prepare for release |
| `main` | Stable release | Only accepts merge from release/hotfix |
| `hotfix/*` | PATCH bump | Fix urgent issues on main |

---

## Version Bump Workflow

### Manual bump

```bash
# 1. Update version in .csproj
# Change <Version>0.5.0</Version> → <Version>1.0.0</Version>

# 2. Commit
git add src/AppLog/AppLog.csproj
git commit -m "chore(release): bump version to 1.0.0"

# 3. Create tag
git tag -a v1.0.0 -m "Release v1.0.0"

# 4. Push
git push origin main --tags
```

### Bump using script

```powershell
# Using version-bump.ps1
.\scripts\version-bump.ps1 -Version "1.0.0"

# Or auto bump
.\scripts\version-bump.ps1 -BumpType "minor"  # 1.0.0 → 1.1.0
.\scripts\version-bump.ps1 -BumpType "patch"  # 1.0.0 → 1.0.1
.\scripts\version-bump.ps1 -BumpType "major"  # 1.0.0 → 2.0.0
```

---

## Version History

| Version | Date | Description |
|---------|------|-------------|
| 0.1.0 | - | Initial project skeleton |