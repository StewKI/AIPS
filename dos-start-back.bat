<# : batch portion
@echo off
set "SCRIPT_DIR=%~dp0"
powershell -ExecutionPolicy Bypass "iex((Get-Content '%~f0' -Raw))"
exit /b
#>

Set-Location (Join-Path $env:SCRIPT_DIR "dotnet")

$jobs = @()
$jobs += Start-Job -ScriptBlock { Set-Location $using:PWD; dotnet run --project AipsWebApi 2>&1 | ForEach-Object { "[WebApi]  $_" } }
$jobs += Start-Job -ScriptBlock { Set-Location $using:PWD; dotnet run --project AipsRT 2>&1 | ForEach-Object { "[RT]     $_" } }
$jobs += Start-Job -ScriptBlock { Set-Location $using:PWD; dotnet run --project AipsWorker 2>&1 | ForEach-Object { "[Worker] $_" } }

try {
    while ($jobs | Where-Object { $_.State -eq 'Running' }) {
        foreach ($job in $jobs) {
            Receive-Job -Job $job
        }
        Start-Sleep -Milliseconds 200
    }
    foreach ($job in $jobs) {
        Receive-Job -Job $job
    }
} finally {
    $jobs | Stop-Job -PassThru | Remove-Job
}
