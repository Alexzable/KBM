@echo off
setlocal

REM 1. Set environment variable to use Docker DB
set USE_DOCKER_DB=true

REM 2. Start .NET services
echo Starting gRPC Service...
start "GRPC Service" cmd /k "dotnet run --project src\KBMGrpcService"

echo Starting HTTP Service...
start "HTTP Service" cmd /k "dotnet run --project src\KBMHttpService"

REM 3. Summary and control
echo.
echo ===============================================
echo [OK] Services launched.
echo -----------------------------------------------
echo - gRPC Service
echo - HTTP Service
echo -----------------------------------------------
echo.
echo Press X then ENTER to stop Docker container.
set /p stopInput=Press X to stop: 
if /I "%stopInput%"=="X" (
    echo Stopping Docker container...
    docker-compose down
    echo [INFO] Docker container stopped.
) else (
    echo [INFO] Invalid input. Docker and services remain running.
)

exit /b 0
