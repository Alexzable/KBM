@echo off
echo ===============================================
echo Starting Docker SQL Server Environment
echo ===============================================

REM 1. Check if Docker is installed
where docker >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Docker is not installed or not in PATH.
    echo Please install Docker before continuing.
    echo See setup instructions here: docs\commands\docker-setup.txt
    exit /b 1
)

REM 2. Check if Docker Desktop is running
echo Checking if Docker Desktop is running...
tasklist /FI "IMAGENAME eq Docker Desktop.exe" | find /I "Docker Desktop.exe" >nul
if errorlevel 1 (
    echo Starting Docker Desktop...
    start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    timeout /t 15 >nul
) else (
    echo Docker Desktop is already running.
)

REM 3. Start docker-compose
echo ---------------------------------------
echo Launching SQL Server container...
docker-compose up -d
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to start Docker Compose.
    exit /b 2
)

echo [SUCCESS] Docker SQL Server environment is running.

REM Done - ready for next step in run_app.bat
exit /b 0