# techbodia-notes-app

Full-stack CRUD notes app for internship practice.

Users log in with username only (no password/JWT auth). Notes are isolated by username.

## Stack

- Frontend: Vue 3 + TypeScript + Tailwind CSS + Pinia + Axios
- Backend: ASP.NET Core Web API (.NET 9)
- ORM: Dapper
- Database: InMemory (default local) or MySQL (Railway)
- Hosting: GitHub Pages (frontend) + Railway (backend)

## Live URLs

- Frontend: https://praseth-002.github.io/techbodia-notes-app/
- Backend health: https://techbodia-notes-app-production.up.railway.app/health
- Backend API base: https://techbodia-notes-app-production.up.railway.app/api

## Requirement Status

### Core Notes Features
- [x] Create note (title required, content optional)
- [x] Read notes list and note details
- [x] Update note and refresh `updatedAt`
- [x] Delete note

### Frontend Requirements
- [x] Notes list page with full CRUD
- [x] Search
- [x] Simple sorting (newest/oldest)
- [x] Responsive layout
- [x] API integration with Axios
- [x] State management with Pinia
- [x] Username login form (no password)
- [ ] Register form (optional item)

### Backend Requirements
- [x] Notes CRUD API
- [x] Per-user access scoping (by `X-User-Name`)
- [x] Dapper ORM
- [ ] SQL Server database (current implementation uses MySQL)
- [ ] Authentication/Authorization (optional item)

## Run Locally

1. Start backend:

```sh
cd backend
dotnet run --launch-profile http
```

2. Start frontend:

```sh
cd frontend
npm install
npm run dev
```

3. Open:

- http://127.0.0.1:5173

## API Contract

All `/api/notes` endpoints require request header:

- `X-User-Name: your_username`

If header is missing/empty, API returns `400`.
