# Changelog - v0.1.0

> **Version:** 0.1.0  
> **Date:** 2026-04-15  
> **Status:** Initial Release (Skeleton)

---

## [0.1.0] - 2026-04-15

### Added
- **(all):** Initial project structure with C# WinForms .NET 8
- **(ui):** MainForm with Port Config, Log Display, Send Data, Log File sections
- **(serial):** SerialPortService for UART communication (open/close/read/write)
- **(log):** LogFileService for thread-safe file logging
- **(model):** LogEntry class with LogDirection enum (TX/RX)
- **(helper):** LogDisplayHelper for color-coded RichTextBox display with 1MB buffer
- **(ci):** GitHub Actions CI pipeline (build, test, format check)
- **(ci):** GitHub Actions Release pipeline (publish, create release, upload artifact)
- **(build):** PowerShell build script with restore, build, test, format steps
- **(build):** Version bump script with SemVer support
- **(build):** Commit message checker for Conventional Commits
- **(docs):** Complete documentation set (PRD, style guide, conventions, implementation plan)

### Version Assessment

**Current version:** 0.1.0 (Initial skeleton)  
**Recommendation:** No version bump needed - this is a project skeleton, no complete functionality yet.

**Next planned version:** 0.2.0
- When: Serial Port Open/Close works completely (Milestone M2)
- Bump: MINOR (new feature)