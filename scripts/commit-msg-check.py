#!/usr/bin/env python3
# =============================================================================
# AppLog - Commit Message Checker
# =============================================================================
# Validates commit messages follow Conventional Commits format.
# Used as a pre-commit hook.
#
# Format: <type>(<scope>): <subject>
#
# Usage:
#   python scripts/commit-msg-check.py <commit-msg-file>
# =============================================================================

import sys
import re

# Valid commit types
VALID_TYPES = [
    "feat", "fix", "docs", "style", "refactor",
    "perf", "test", "build", "ci", "chore", "revert"
]

# Valid scopes (optional)
VALID_SCOPES = [
    "serial", "log", "ui", "model", "helper",
    "docs", "ci", "build", "all"
]

# Conventional Commits pattern
# Format: type(scope): subject
# Or:     type!: subject (breaking change)
PATTERN = re.compile(
    r"^(?P<type>" + "|".join(VALID_TYPES) + r")"
    r"(?:\((?P<scope>[a-zA-Z0-9_-]+)\))?"
    r"(?P<breaking>!)?"
    r":\s(?P<subject>.+)$"
)

# Maximum subject length
MAX_SUBJECT_LENGTH = 72


def check_commit_message(message: str) -> list[str]:
    """Validate a commit message against Conventional Commits.

    Args:
        message: The commit message to validate.

    Returns:
        A list of error messages. Empty list means valid.
    """
    errors = []

    # Get first line (subject)
    first_line = message.split("\n")[0].strip()

    if not first_line:
        errors.append("Commit message is empty")
        return errors

    # Match pattern
    match = PATTERN.match(first_line)

    if not match:
        errors.append(
            f"Invalid format: '{first_line}'\n"
            f"Expected: <type>(<scope>): <subject>\n"
            f"Valid types: {', '.join(VALID_TYPES)}\n"
            f"Example: feat(serial): add baud rate selection"
        )
        return errors

    # Validate scope (warning only, not error)
    scope = match.group("scope")
    if scope and scope not in VALID_SCOPES:
        # This is just a warning, not blocking
        pass

    # Check subject length
    subject = match.group("subject")
    if len(subject) > MAX_SUBJECT_LENGTH:
        errors.append(
            f"Subject too long: {len(subject)} chars (max {MAX_SUBJECT_LENGTH})\n"
            f"Subject: {subject}"
        )

    # Check subject doesn't end with period
    if subject.endswith("."):
        errors.append(
            f"Subject should not end with a period: '{subject}'"
        )

    # Check subject starts with lowercase
    if subject[0].isupper() and not subject[0].isdigit():
        errors.append(
            f"Subject should start with lowercase: '{subject}'"
        )

    return errors


def main():
    """Main entry point for the commit message checker."""
    if len(sys.argv) < 2:
        print("ERROR: No commit message file provided")
        print("Usage: python commit-msg-check.py <commit-msg-file>")
        sys.exit(1)

    commit_msg_file = sys.argv[1]

    try:
        with open(commit_msg_file, "r", encoding="utf-8") as f:
            message = f.read().strip()
    except FileNotFoundError:
        print(f"ERROR: File not found: {commit_msg_file}")
        sys.exit(1)

    # Skip merge commits
    if message.startswith("Merge "):
        sys.exit(0)

    errors = check_commit_message(message)

    if errors:
        print("=" * 60)
        print("COMMIT MESSAGE VALIDATION FAILED")
        print("=" * 60)
        for error in errors:
            print(f"  ✗ {error}")
        print()
        print("Expected format: <type>(<scope>): <subject>")
        print(f"Valid types: {', '.join(VALID_TYPES)}")
        print(f"Valid scopes: {', '.join(VALID_SCOPES)}")
        print()
        print("Examples:")
        print("  feat(serial): add baud rate selection dropdown")
        print("  fix(log): fix 1MB buffer not trimming correctly")
        print("  docs(prd): update PRD with send data feature")
        print("=" * 60)
        sys.exit(1)
    else:
        # Silent success
        sys.exit(0)


if __name__ == "__main__":
    main()