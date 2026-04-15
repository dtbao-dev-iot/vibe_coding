# Changelog Convention - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Last Updated:** 2026-04-15

---

## Changelog Standard

This project follows **Keep a Changelog** (https://keepachangelog.com/).

### Purpose

- Record all notable changes between versions
- Help users and developers understand what has changed
- Support version bump decisions (MAJOR/MINOR/PATCH)

---

## Version Bump Rules & Changelog Creation

### Workflow

```
1. Develop features/fixes on a branch
2. Commit following Conventional Commits
3. Merge into main/develop
4. Assess all commits since the latest version
5. Decide version bump (MAJOR/MINOR/PATCH)
6. Create CHANGELOG file from template
7. Bump version in .csproj
8. Commit + Tag
9. Push tag → trigger Release CI
```

### Assess Changes → Version Bump

**Step 1:** Get commit list from the latest tag:

```bash
# Get commits from the latest tag to current HEAD
git log <last-tag>..HEAD --oneline

# Example:
git log v0.5.0..HEAD --oneline
```

**Step 2:** Classify commits by change type:

| Commit Type | Changelog Section | Version Impact |
|-------------|-------------------|---------------|
| `feat(...)!:` or `BREAKING CHANGE` | **Breaking Changes** | MAJOR |
| `feat(...)` | **Added** | MINOR |
| `fix(...)` | **Fixed** | PATCH |
| `perf(...)` | **Changed** (performance) | PATCH |
| `refactor(...)` | **Changed** | PATCH (if visible) |
| `docs(...)` | (Do not add to changelog) | None |
| `style(...)` | (Do not add to changelog) | None |
| `test(...)` | (Do not add to changelog) | None |

**Step 3:** Decide version bump:

```
If there are Breaking Changes  → MAJOR bump (x.0.0)
If there are Added features     → MINOR bump (0.x.0)
If only Fixed/Changed           → PATCH bump (0.0.x)
If no src changes               → No bump
```

**Step 4:** Create changelog file:

- Create file `docs/CHANGELOG/CHANGELOG-<version>.md` using the template
- Or add a new section to the root `CHANGELOG.md` file

---

## Changelog Sections

### Section Order

```markdown
## [x.y.z] - YYYY-MM-DD

### Breaking Changes
- ...

### Added
- ...

### Changed
- ...

### Deprecated
- ...

### Removed
- ...

### Fixed
- ...

### Security
- ...
```

### Section Descriptions

| Section | Description | Version Impact |
|---------|-------------|---------------|
| **Breaking Changes** | Incompatible changes, users must update code/config | MAJOR |
| **Added** | New features, new functionality | MINOR |
| **Changed** | Behavior changes in existing features, improvements | PATCH |
| **Deprecated** | Features that will be removed in a future version | MINOR (advance notice) |
| **Removed** | Features that have been removed (previously marked Deprecated) | MAJOR |
| **Fixed** | Bug fixes | PATCH |
| **Security** | Security vulnerability fixes | PATCH or MAJOR |

---

## Practical Examples for AppLog

### Example: Assessing Commits

```bash
$ git log v0.5.0..HEAD --oneline

a1b2c3d feat(serial): add baud rate selection dropdown
e4f5g6h feat(ui): add auto-scroll toggle button
i7j8k9l fix(log): fix 1MB buffer not trimming correctly
m0n1o2p fix(serial): handle port disconnect gracefully
q3r4s5t perf(log): optimize RichTextBox append performance
u6v7w8x docs(readme): update installation instructions
```

**Assessment:**
- 2x `feat` → MINOR bump
- 2x `fix` → PATCH bump
- 1x `perf` → PATCH bump
- 1x `docs` → no impact

**Result:** MINOR bump → `v0.6.0`

### Example: Changelog Output

```markdown
## [0.6.0] - 2026-04-20

### Added
- Serial port baud rate selection dropdown (feat serial)
- Auto-scroll toggle button for log display (feat ui)

### Changed
- Optimized RichTextBox append performance for high-speed logging (perf log)

### Fixed
- 1MB buffer now correctly trims old entries when limit is reached (fix log)
- Port disconnect no longer crashes the application (fix serial)
```

---

## Automated Changelog Generation (Optional)

### Using git log + script

```bash
# Generate changelog from Conventional Commits
git log v0.5.0..HEAD --pretty=format:"%s" | \
  grep -E "^(feat|fix|perf|refactor)" | \
  sort
```

### Recommended Tools

| Tool | Description |
|------|-------------|
| `git-cliff` | Rust-based changelog generator from Conventional Commits |
| `standard-changelog` | Node.js changelog generator |
| `conventional-changelog` | CLI tool for Conventional Commits |

---

## Changelog File Naming

| Approach | File | Description |
|----------|------|-------------|
| **Centralized** | `docs/CHANGELOG.md` | Single file containing all versions, new sections added at the top |
| **Distributed** | `docs/CHANGELOG/CHANGELOG-<version>.md` | One file per version |

**Recommendation:** Use **centralized** (`CHANGELOG.md`) for small projects, **distributed** for large projects.

### Using Distributed Approach

```
docs/
├── changelog-convention.md      # Convention rules (this file)
├── changelog-template.md         # Template
├── CHANGELOG/
│   ├── CHANGELOG-0.1.0.md       # Changelog for v0.1.0
│   ├── CHANGELOG-0.2.0.md       # Changelog for v0.2.0
│   └── CHANGELOG-1.0.0.md       # Changelog for v1.0.0