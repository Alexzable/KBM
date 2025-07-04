﻿KBM Microservices Environment

Welcome to the KBM Microservices Architecture. This repository contains two .NET microservices:

KBMGrpcService – gRPC-based backend

KBMHttpService – RESTful HTTP API

The services connect to a shared SQL Server database running in Docker. The system is intended for rapid development and testing with a minimal setup effort.

🚀 How to Start the Environment

1. Prerequisites

Windows 10+

.NET 8 SDK

Docker Desktop (must be installed and available in PATH)

⚠️ If Docker is not installed or running, the environment scripts will warn and exit.

2. Recommended Setup

Run from root of the project:

run_app.bat

This will:

Start Docker and launch the SQL Server container

Set an environment variable so services use Docker DB connection

Launch both KBMGrpcService and KBMHttpService in new terminal windows

Wait for your confirmation to shut down the Docker container

To stop the environment:

Press X then Enter in the run_app.bat prompt window

📬 Using Postman to Test

1. Open Postman

Import the collection file from: docs/postman/KBM_Organization_API.postman_collection

Import the collection file from: docs/postman/KBM_Users_API.postman_collection

2. Environment (if any):

You can create a Postman environment with localhost:5001 and localhost:6001 as base URLs (gRPC & HTTP ports)

3. Ready to test!

Use available requests in the collection to:

Create users

Manage organizations

Simulate data exchange between gRPC and HTTP

📁 Project Structure

```
KBM/
├── src/
│   ├── KBMGrpcService/
│   └── KBMHttpService/
├── docker-compose.yml
├── .env
├── run_app.bat
├── run_sqlserver_docker.bat
├── run_services.bat
└── docs/
    ├── postman/
    │   ├── KBM_Organization_API.postman_collection
    │   └── KBM_Users_API.postman_collection
    ├── illustrations/
    │   ├── architecture.png
    │   └── dataflow-and-integration.png
    ├── instructions/
    │   ├── docker-setup.txt
    │   ├── entity-basic.txt
    │   ├── libraries-core.txt
    │   ├── project-creation.txt
    │   └── script-definition.txt
    └── project/
        └── project-overview.docx
```



📄 Documentation

docs/instructions/script-definition.txt

This file explains in plain text what each .bat script does:

run_app.bat – Main entry point. Checks if the SQL Server container is already running. If it is, it launches the gRPC and HTTP services. If not, it starts the SQL Server container and then proceeds to launch the services.

run_sqlserver_docker.bat – Verifies whether Docker Desktop is running and starts it if needed. Then it launches the SQL Server container using docker-compose, and once ready, it calls run_services.bat.

run_services.bat – Starts the .NET services (KBMGrpcService and KBMHttpService) in separate terminal windows and provides an option to shut down the Docker container.

docs/instructions/docker-setup.txt

Instructions to install Docker for Windows and verify setup

docs/instructions/entity-basic.txt

Explains key database entities and EF Core migration commands

docs/instructions/libraries-core.txt

Lists NuGet packages used across projects and their purposes

docs/instructions/project-creation.txt

Shows how the initial solution and project structure was generated using dotnet new

✅ Notes

Connection strings are dynamically chosen based on USE_DOCKER_DB env var.

Docker image used: mcr.microsoft.com/mssql/server:2017-latest

SQL password is configured via .env

All commands work offline once Docker image is pulled

Setup HTTPS dev cert - dotnet dev-certs https --trust

For detailed architecture or flow diagrams, see docs/illustrations/.
