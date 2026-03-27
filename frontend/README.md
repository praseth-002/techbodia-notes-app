# frontend

Vue 3 + TypeScript + Tailwind frontend for the notes app.

## Features

- Notes CRUD UI (create, read, update, delete)
- Search and sort by created date
- Simple login gate (username only)
- Per-user note scoping via `X-User-Name` header
- Responsive Tailwind layout
- API integration via Axios

## Deploy Targets

- Frontend: GitHub Pages
- Backend API: Railway

## Live URL

- https://praseth-002.github.io/techbodia-notes-app/

## Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Recommended Browser Setup

- Chromium-based browsers (Chrome, Edge, Brave, etc.):
  - [Vue.js devtools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
  - [Turn on Custom Object Formatter in Chrome DevTools](http://bit.ly/object-formatters)
- Firefox:
  - [Vue.js devtools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)
  - [Turn on Custom Object Formatter in Firefox DevTools](https://fxdx.dev/firefox-devtools-custom-object-formatters/)

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

By default, the app calls:

- http://localhost:5187/api

To use a different backend URL, create `frontend/.env.local`:

```sh
VITE_API_BASE_URL=http://localhost:5187/api
VITE_BASE_PATH=/
```

For hosted deployment values, see `frontend/.env.production.example`.

For GitHub Pages, set `VITE_BASE_PATH` to your repo path, for example:

```sh
VITE_BASE_PATH=/techbodia-notes-app/
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

## Full Local Run

Backend:

```sh
cd backend
dotnet run --launch-profile http
```

Frontend:

```sh
cd frontend
npm install
npm run dev
```

## GitHub Pages Deployment

A GitHub Actions workflow is already included at `.github/workflows/frontend-pages.yml`.

Required repository variable:

- VITE_API_BASE_URL=https://techbodia-notes-app-production.up.railway.app/api

The workflow sets `VITE_BASE_PATH` automatically from repository name.
