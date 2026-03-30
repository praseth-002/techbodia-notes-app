# backend

ASP.NET Core Web API for a notes CRUD app.

This backend uses Dapper and supports two storage providers:

- `InMemory` (default for quick local runs)
- `MySql` (persistent, used on Railway)

## Current Auth Model

No password/JWT auth is implemented.

User identity is provided by request header:

- `X-User-Name`

All note operations are scoped to this username.

## Features

- Create, read, update, delete notes
- Per-user note isolation by username header
- Auto-managed `created_at` and `updated_at`
- DB table auto-creation in MySQL mode

## Local Run

```sh
cd backend
dotnet run --launch-profile http
```

Endpoints:

- API base: http://localhost:5187/api
- Health: http://localhost:5187/health

## Railway Setup

Required variables:

- `StorageProvider=MySql` (or `InMemory`)
- `ConnectionStrings__DefaultConnection=${{MySQL.MYSQL_URL}}` (for MySQL mode)
- `Cors__AllowedOrigins__0=http://localhost:5173`
- `Cors__AllowedOrigins__1=http://127.0.0.1:5173`
- `Cors__AllowedOrigins__2=https://praseth-002.github.io`

## Requirement Note

Assignment asked for SQL Server.

Current implementation uses MySQL.
