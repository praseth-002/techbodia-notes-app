# techbodia-notes-app

Full-stack notes application built for internship assessment.

## Stack

- Frontend: Vue 3 (web framework), TypeScript (language), Tailwind CSS (styling)
- Backend: .NET 9 (web server)
- Storage: In-memory or MySQL (persistent)
- Hosting: GitHub Pages (frontend), Railway (backend)

## Live URLs

- Frontend: https://praseth-002.github.io/techbodia-notes-app/
- Backend health: https://techbodia-notes-app-production.up.railway.app/health
- Backend API base: https://techbodia-notes-app-production.up.railway.app/api

## How It Works

By default, notes are stored in backend memory for local development.
In Railway, you can switch to MySQL so notes persist across restarts.

## Feature Checklist

### Core Features
- [x] Create a Note
- [x] Read Notes
- [x] Update a Note
- [x] Delete a Note

### Frontend
- [x] Login with username
- [x] Notes list page with CRUD operations
- [x] Simple filtering and sorting functionality
- [x] Search
- [x] Responsive design using TailwindCss
- [x] Perform basic API integrations using Axios or Fetch
- [x] State management

### Backend
- [ ] Authentication & Authorization
- [x] CRUD notes
- [x] user can only read, update and delete their own notes
- [ ] using Dapper for ORM with a SQL Server database

### Deployment
- [x] Frontend deploys automatically to GitHub Pages
- [x] Backend deploys automatically to Railway
- [ ] SQL server

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

- Frontend automatically deploys when you push to the repo (GitHub Pages)
- Backend automatically deploys when you push to the repo (Railway)

Key files:

- railway.json
- backend/Dockerfile
- .github/workflows/frontend-pages.yml

## Notes API contract

All /api/notes routes require this header:

- X-User-Name: your_username
