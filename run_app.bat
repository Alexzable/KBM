@echo off
setlocal

echo ===============================================
echo Bootstrapping Full Development Environment
echo ===============================================

REM 1. Check if SQL Server container is already running
docker ps --filter "name=sqlserver" --filter "status=running" --format "{{.Names}}" | findstr /I "sqlserver" >nul
if %ERRORLEVEL% EQU 0 (
    echo [INFO] SQL Server container already running. Skipping Docker startup.
) else (
    REM 2. Start Docker SQL Server environment
    call run_sqlserver_docker.bat no-wait
    if %ERRORLEVEL% NEQ 0 (
        echo [ERROR] Docker environment failed to start. Exiting.
        exit /b %ERRORLEVEL%
    )
)

REM 2. Set environment variable to use Docker DB
set USE_DOCKER_DB=true

REM 3. Start .NET services
echo Starting gRPC Service...
start "GRPC Service" cmd /k "dotnet run --project src\KBMGrpcService"

echo Starting HTTP Service...
start "HTTP Service" cmd /k "dotnet run --project src\KBMHttpService"

REM 4. Summary and control
echo.
echo ===============================================
echo [OK] Environment is running.
echo -----------------------------------------------
echo - gRPC Service
echo - HTTP Service
echo - SQL Server (Docker)
echo -----------------------------------------------
echo.
echo Press X then ENTER to stop everything (including Docker and services).
set /p stopInput=Press X to stop: 
if /I "%stopInput%"=="X" (
    echo Stopping Docker container...
    docker-compose down
    echo [INFO] Docker container stopped.
) else (
    echo [INFO] Invalid input. Services remain running.
)

exit /b 0
