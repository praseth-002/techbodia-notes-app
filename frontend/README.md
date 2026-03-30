# frontend

Vue 3 + TypeScript + Tailwind notes UI.

This app has a simple username login gate (no password auth flow).

## Features

- Notes CRUD UI
- Notes list + detail modal
- Search (title/content)
- Sort by created date (newest/oldest)
- Responsive layout
- Pinia state management
- Axios API integration

## API Behavior

Frontend sends `X-User-Name` header automatically from local storage.

Backend uses that header to scope notes by user.

## Local Development

```sh
npm install
npm run dev
```

Default API target:

- http://localhost:5187/api

Set custom API URL in `.env.local`:

```sh
VITE_API_BASE_URL=http://localhost:5187/api
VITE_BASE_PATH=/
```

## Production Build

```sh
npm run build
```

## Deploy

- Frontend: GitHub Pages
- Backend API: Railway

Live frontend URL:

- https://praseth-002.github.io/techbodia-notes-app/
