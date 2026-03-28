# techbodia-notes-app

Full-stack notes application built for internship assessment.

## Stack

- Frontend: Vue 3 (web framework), TypeScript (language), Tailwind CSS (styling)
- Backend: .NET 9 (web server)
- Storage: In-memory (data stored while app runs, resets on restart)
- Hosting: GitHub Pages (frontend), Railway (backend)

## Live URLs

- Frontend: https://praseth-002.github.io/techbodia-notes-app/
- Backend health: https://techbodia-notes-app-production.up.railway.app/health
- Backend API base: https://techbodia-notes-app-production.up.railway.app/api

## How It Works

Your notes are stored in the backend's memory, so they disappear if the server restarts. This is fine for testing, but in the future you can switch to a real database.

## Feature Checklist

### Core Features
- [x] Create notes with a title and optional content
- [x] View list of all your notes with dates
- [x] Click on a note to read it fully
- [x] Edit notes (change title and content)
- [x] Delete notes
- [x] Dates are set automatically

### Frontend Features
- [x] Login with username
- [x] Create, edit, delete notes
- [x] Search notes
- [x] Sort by newest/oldest
- [x] Works on mobile and desktop
- [x] Connects to the backend API
- [x] Saves application state locally
- [x] View full note details
- [x] Create/edit notes form

### Backend Features
- [x] Web API that handles note requests
- [x] User login via username
- [x] Only show your own notes
- [x] Create, read, update, delete notes
- [x] Save notes when you create/edit them
- [x] Runs in Docker containers
- [ ] Use a real database (currently stores in memory)

### Deployment
- [x] Frontend deploys automatically to GitHub Pages
- [x] Backend deploys automatically to Railway
- [x] Docker configuration

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

## To Use a Real Database (Optional)

If you want your notes to stay saved forever, you can switch from the in-memory storage to a real database:

- Set: StorageProvider=SqlServer
- Set: ConnectionStrings__DefaultConnection=your_database_connection
