# techbodia-notes-app

Full-stack notes application built for internship assessment.

## Stack

- Frontend: Vue 3, TypeScript, Tailwind CSS
- Backend: ASP.NET Core Web API (.NET 9)
- Data access: Dapper
- Storage mode: InMemory by default, SqlServer optional

## Live URLs

- Frontend: https://praseth-002.github.io/techbodia-notes-app/
- Backend health: https://techbodia-notes-app-production.up.railway.app/health
- Backend API base: https://techbodia-notes-app-production.up.railway.app/api

## Current Deployment Mode

The deployed version currently uses InMemory storage for quick proof-of-concept.
Data can reset when the backend restarts.

## Run Locally

1. Start backend

```sh
cd backend
dotnet run --launch-profile http
```

2. Start frontend

```sh
cd frontend
npm install
npm run dev
```

3. Open app

- http://127.0.0.1:5173

## Deployment

- Railway backend deployment is configured with Docker.
- GitHub Pages frontend deployment is configured with GitHub Actions.

Key files:

- railway.json
- backend/Dockerfile
- .github/workflows/frontend-pages.yml

## Notes API contract

All /api/notes routes require this header:

- X-User-Name: your_username

## Optional SQL Server mode

If required later, switch backend to SqlServer by setting:

- StorageProvider=SqlServer
- ConnectionStrings__DefaultConnection=your_sql_server_connection_string

The Dapper SQL repository and schema bootstrap are already implemented.
