param (
    [int]$Delay = 0
)

if ($Delay -gt 0) {
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    $response = ""

    Write-Host "Do you want to skip building FormBuilder? (y/n) (Auto-building $($Delay) seconds...)" -ForegroundColor Yellow

    while ($stopwatch.Elapsed.TotalSeconds -lt $Delay) {
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
        Write-Host "Build skipped." -ForegroundColor Red
    } else {
        Write-Host "Starting build..." -ForegroundColor Green
        dotnet build -c Debug
    }
} else {
    dotnet build -c Debug
}