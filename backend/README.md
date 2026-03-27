# backend

ASP.NET Core Web API for notes CRUD.

## Features

- Notes CRUD endpoints
- Per-user note ownership via X-User-Name request header
- Pluggable storage mode:
  - InMemory (default)
  - SqlServer with Dapper

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

Railway deployment files are included in the repo root and backend folder.

Set environment variables:

- StorageProvider=InMemory
- Cors__AllowedOrigins__0=http://localhost:5173
- Cors__AllowedOrigins__1=http://127.0.0.1:5173
- Cors__AllowedOrigins__2=https://YOUR_GITHUB_USERNAME.github.io

## Switch To SQL Server + Dapper

1. Start SQL Server (optional Docker path from repo root):

```sh
docker compose up -d
```

2. Edit appsettings.Development.json:

- Set StorageProvider to SqlServer
- Set ConnectionStrings.DefaultConnection

3. Restart backend:

```sh
dotnet run --launch-profile http
```

The Notes table is auto-created on startup in SQL mode.
A reference SQL script is also available at sql/init.sql.

## API Contract

All /api/notes endpoints require this header:

- X-User-Name: your_username

Without the header, API returns 400.
