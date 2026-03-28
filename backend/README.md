# backend

ASP.NET Core Web API for notes CRUD.

## Features

- Notes CRUD endpoints
- Per-user note ownership via X-User-Name request header
- Configurable storage provider: InMemory or MySql

## Run Locally

Default mode is InMemory so you can run without SQL setup.

1. Start API:

```sh
cd backend
dotnet run --launch-profile http
```

2. API base URL:

- http://localhost:5187/api

3. Health check URL:

- http://localhost:5187/health

## Railway Deploy (InMemory)

Railway deployment files are included in this repository.

Set environment variables:

- StorageProvider=InMemory
- Cors__AllowedOrigins__0=http://localhost:5173
- Cors__AllowedOrigins__1=http://127.0.0.1:5173
- Cors__AllowedOrigins__2=https://praseth-002.github.io

Optional health check path:

- /health

## Railway Deploy (MySQL)

1. In Railway, add a MySQL service to the project.
2. Set API environment variables:

- StorageProvider=MySql
- ConnectionStrings__DefaultConnection=${{MySQL.MYSQL_URL}}
- Cors__AllowedOrigins__0=http://localhost:5173
- Cors__AllowedOrigins__1=http://127.0.0.1:5173
- Cors__AllowedOrigins__2=https://praseth-002.github.io

3. Redeploy the backend service.

The backend creates the `notes` table automatically on startup.

## API Contract

All /api/notes endpoints require this header:

- X-User-Name: your_username

Without the header, API returns 400.
