# =============================================================================
# AppLog - Build Script
# =============================================================================
# Usage:
#   .\scripts\build.ps1                    # Debug build
#   .\scripts\build.ps1 -Configuration Release  # Release build
#   .\scripts\build.ps1 -SkipTests              # Skip tests
# =============================================================================

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [switch]$SkipTests = $false
)

$ErrorActionPreference = "Stop"
$SolutionPath = "src/AppLog.sln"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  AppLog Build Script" -ForegroundColor Cyan
Write-Host "  Configuration: $Configuration" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Restore
Write-Host "[1/4] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore $SolutionPath
if ($LASTEXITCODE -ne 0) {
    Write-Host "FAILED: Restore dependencies" -ForegroundColor Red
    exit 1
}
Write-Host "OK: Restore dependencies" -ForegroundColor Green
Write-Host ""

# Step 2: Build
Write-Host "[2/4] Building solution ($Configuration)..." -ForegroundColor Yellow
dotnet build $SolutionPath --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "FAILED: Build solution" -ForegroundColor Red
    exit 1
}
Write-Host "OK: Build solution" -ForegroundColor Green
Write-Host ""

# Step 3: Test
if (-not $SkipTests) {
    Write-Host "[3/4] Running tests..." -ForegroundColor Yellow
    dotnet test $SolutionPath --configuration $Configuration --no-build --verbosity normal
    if ($LASTEXITCODE -ne 0) {
        Write-Host "FAILED: Run tests" -ForegroundColor Red
        exit 1
    }
    Write-Host "OK: Run tests" -ForegroundColor Green
} else {
    Write-Host "[3/4] Skipping tests" -ForegroundColor DarkGray
}
Write-Host ""

# Step 4: Format check
Write-Host "[4/4] Checking code format..." -ForegroundColor Yellow
dotnet format $SolutionPath --verify-no-changes --verbosity minimal
if ($LASTEXITCODE -ne 0) {
    Write-Host "WARNING: Code format issues detected. Run 'dotnet format' to fix." -ForegroundColor DarkYellow
} else {
    Write-Host "OK: Code format" -ForegroundColor Green
}
Write-Host ""

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  BUILD SUCCESSFUL" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan