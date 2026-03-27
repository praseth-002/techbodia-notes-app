# techbodia-notes-app

Full-stack notes app built with:

- Frontend: Vue 3 + TypeScript + Tailwind
- Backend: ASP.NET Core Web API
- ORM: Dapper
- Database mode: InMemory (default), SqlServer (optional)

## Current Recommended Mode

For fastest completion and submission, run with InMemory storage.
No database setup required.

## Local Run (InMemory)

1) Start backend

- cd backend
- dotnet run --launch-profile http

2) Start frontend

- cd frontend
- npm install
- npm run dev

3) Open app

- http://127.0.0.1:5173

## Deployment Prep Included

This repository is already prepared for:

- Railway backend deploy (Docker)
- GitHub Pages frontend deploy (GitHub Actions)

### Railway Files

- railway.json
- backend/Dockerfile
- backend/.dockerignore

### GitHub Pages Files

- .github/workflows/frontend-pages.yml
- frontend/vite.config.ts

## Railway Backend Setup

1) Create a new Railway project and connect this repository.
2) Deploy using the included Docker config.
3) Set backend environment variables:

- StorageProvider=InMemory
- Cors__AllowedOrigins__0=http://localhost:5173
- Cors__AllowedOrigins__1=http://127.0.0.1:5173
- Cors__AllowedOrigins__2=https://YOUR_GITHUB_USERNAME.github.io

4) Copy backend URL after deploy. Example:

- https://your-service.up.railway.app

Health check endpoint is available at:

- /health

## GitHub Pages Frontend Setup

1) In GitHub repository settings, enable Pages with GitHub Actions source.
2) Create repository variable:

- VITE_API_BASE_URL=https://your-service.up.railway.app/api

3) Push to main branch to trigger workflow.
4) Frontend will be published to:

- https://YOUR_GITHUB_USERNAME.github.io/REPO_NAME/

## Optional SQL Server Mode

If needed later, switch backend to SQL mode:

- StorageProvider=SqlServer
- ConnectionStrings__DefaultConnection=your_sql_server_connection_string

The Dapper SQL repository and schema initialization are already implemented.
