@echo off
setlocal

echo ===============================================
echo Bootstrapping Full Development Environment
echo ===============================================

REM 1. Check if SQL Server container is already running
docker ps --filter "name=sqlserver" --filter "status=running" --format "{{.Names}}" | findstr /I "sqlserver" >nul
if %ERRORLEVEL% EQU 0 (
    echo [INFO] SQL Server container already running. Skipping Docker startup.
    call run_services.bat
) else (
    REM 2. Start Docker SQL Server environment
    call run_sqlserver_docker.bat
)

exit /b 0
