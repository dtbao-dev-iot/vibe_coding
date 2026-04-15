# =============================================================================
# AppLog - Version Bump Script
# =============================================================================
# Usage:
#   .\scripts\version-bump.ps1 -Version "1.0.0"
#   .\scripts\version-bump.ps1 -BumpType "major"
#   .\scripts\version-bump.ps1 -BumpType "minor"
#   .\scripts\version-bump.ps1 -BumpType "patch"
# =============================================================================

param(
    [Parameter(Mandatory = $false)]
    [string]$Version,

    [ValidateSet("major", "minor", "patch")]
    [Parameter(Mandatory = $false)]
    [string]$BumpType,

    [switch]$CreateTag = $false,
    [switch]$Push = $false
)

$ErrorActionPreference = "Stop"
$CsprojPath = "src/AppLog/AppLog.csproj"

# =============================================================================
# Functions
# =============================================================================

function Get-CurrentVersion {
    param([string]$Path)

    $content = Get-Content $Path -Raw
    if ($content -match '<Version>(.*?)</Version>') {
        return $matches[1]
    }
    return "0.0.0"
}

function Set-Version {
    param(
        [string]$Path,
        [string]$NewVersion
    )

    $content = Get-Content $Path -Raw

    # Update <Version>
    $content = $content -replace '<Version>.*?</Version>', "<Version>$NewVersion</Version>"

    # Update <AssemblyVersion> (format: x.y.z.0)
    $assemblyVersion = "$NewVersion.0"
    $content = $content -replace '<AssemblyVersion>.*?</AssemblyVersion>', "<AssemblyVersion>$assemblyVersion</AssemblyVersion>"

    # Update <FileVersion> (format: x.y.z.0)
    $content = $content -replace '<FileVersion>.*?</FileVersion>', "<FileVersion>$assemblyVersion</FileVersion>"

    Set-Content $Path $content -NoNewline
}

function Bump-Version {
    param(
        [string]$CurrentVersion,
        [string]$Type
    )

    $parts = $CurrentVersion -split '\.'
    $major = [int]$parts[0]
    $minor = [int]$parts[1]
    $patch = [int]$parts[2]

    switch ($Type) {
        "major" { $major++; $minor = 0; $patch = 0 }
        "minor" { $minor++; $patch = 0 }
        "patch" { $patch++ }
    }

    return "$major.$minor.$patch"
}

# =============================================================================
# Main
# =============================================================================

# Validate parameters
if (-not $Version -and -not $BumpType) {
    Write-Host "ERROR: Specify either -Version or -BumpType" -ForegroundColor Red
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor Yellow
    Write-Host "  .\scripts\version-bump.ps1 -Version `"1.0.0`""
    Write-Host "  .\scripts\version-bump.ps1 -BumpType `"minor`""
    exit 1
}

# Get current version
$currentVersion = Get-CurrentVersion -Path $CsprojPath
Write-Host "Current version: $currentVersion" -ForegroundColor Cyan

# Calculate new version
if ($Version) {
    $newVersion = $Version
} else {
    $newVersion = Bump-Version -CurrentVersion $currentVersion -Type $BumpType
}

Write-Host "New version:     $newVersion" -ForegroundColor Green
Write-Host ""

# Validate version format
if ($newVersion -notmatch '^\d+\.\d+\.\d+(-[a-zA-Z0-9.]+)?$') {
    Write-Host "ERROR: Invalid version format: $newVersion" -ForegroundColor Red
    Write-Host "Expected format: MAJOR.MINOR.PATCH or MAJOR.MINOR.PATCH-label" -ForegroundColor Yellow
    exit 1
}

# Update .csproj
Write-Host "Updating $CsprojPath..." -ForegroundColor Yellow
Set-Version -Path $CsprojPath -NewVersion $newVersion
Write-Host "OK: Version updated" -ForegroundColor Green
Write-Host ""

# Git operations
if ($CreateTag) {
    Write-Host "Creating git commit and tag..." -ForegroundColor Yellow

    git add $CsprojPath
    git commit -m "chore(release): bump version to $newVersion"

    $tagName = "v$newVersion"
    git tag -a $tagName -m "Release $tagName"

    Write-Host "OK: Commit and tag created" -ForegroundColor Green

    if ($Push) {
        Write-Host "Pushing to origin..." -ForegroundColor Yellow
        git push origin main
        git push origin $tagName
        Write-Host "OK: Pushed to origin" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "To push manually:" -ForegroundColor Yellow
        Write-Host "  git push origin main"
        Write-Host "  git push origin $tagName"
    }
} else {
    Write-Host "To create commit and tag:" -ForegroundColor Yellow
    Write-Host "  git add $CsprojPath"
    Write-Host "  git commit -m `"chore(release): bump version to $newVersion`""
    Write-Host "  git tag -a v$newVersion -m `"Release v$newVersion`""
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  VERSION BUMPED: $currentVersion → $newVersion" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan