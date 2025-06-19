@echo off
setlocal

echo ===============================================
echo Starting Docker SQL Server Environment
echo ===============================================

REM 1. Check if Docker is installed
where docker >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Docker is not installed or not in PATH.
    exit /b 1
)

REM 2. Check if Docker Desktop is running
echo Checking if Docker Desktop is running...
tasklist /FI "IMAGENAME eq Docker Desktop.exe" | find /I "Docker Desktop.exe" >nul
if errorlevel 1 (
    echo Starting Docker Desktop...
    start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    timeout /t 8 >nul
) else (
    echo Docker Desktop is already running.
)

REM 3. Start docker-compose
echo Launching SQL Server container...
docker-compose up -d
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to start Docker Compose.
    exit /b 2
)

echo [SUCCESS] Docker SQL Server environment is running.

REM 4. Wait for SQL Server to finish initializing
echo Waiting 8 seconds for SQL Server to be ready...
timeout /t 8 >nul

REM 5. Continue with services
call run_services.bat

exit /b 0
