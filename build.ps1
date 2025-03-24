$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$response = ""

Write-Host "Do you want to build FormBuilder? (y/n) (Auto-skipping in 2 seconds...)" -ForegroundColor Yellow

while ($stopwatch.Elapsed.TotalSeconds -lt 2) {
    if ($Host.UI.RawUI.KeyAvailable) {
        $key = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown").Character
        if ($key -eq 'y' -or $key -eq 'Y') {
            $response = "y"
            break
        } elseif ($key -eq 'n' -or $key -eq 'N') {
            $response = "n"
            break
        }
    }
}

$stopwatch.Stop()

if ($response -eq "y") {
    Write-Host "Starting build..." -ForegroundColor Green
    dotnet build -c Debug
} else {
    Write-Host "Build skipped." -ForegroundColor Red
}