# Changelog Template - AppLog

> **Project:** AppLog - UART Serial Log Reader  
> **Last Updated:** 2026-04-15

---

## How to Use

1. Copy the template below
2. Replace `[x.y.z]` with the new version
3. Replace `YYYY-MM-DD` with the release date
4. Fill in changes under the correct sections
5. Remove empty sections
6. Save the file

---

## Template

```markdown
## [x.y.z] - YYYY-MM-DD

### Breaking Changes
- **(scope):** Description of breaking change and migration guide

### Added
- **(scope):** Description of new feature
- **(scope):** Description of new feature

### Changed
- **(scope):** Description of behavior change

### Deprecated
- **(scope):** Description of feature to be removed, including target version

### Removed
- **(scope):** Description of removed feature

### Fixed
- **(scope):** Description of bug fix
- **(scope):** Description of bug fix

### Security
- **(scope):** Description of security fix
```

---

## Filled Example

```markdown
## [0.6.0] - 2026-04-20

### Added
- **(serial):** Baud rate selection dropdown in port config area
- **(ui):** Auto-scroll toggle button for log display

### Changed
- **(log):** Optimized RichTextBox append performance for high-speed logging

### Fixed
- **(log):** 1MB buffer now correctly trims old entries when limit is reached
- **(serial):** Port disconnect no longer crashes the application
```

---

## Version Reference

| Version | Date | Status |
|---------|------|--------|
| 0.1.0 | - | Planned |