import axios from 'axios'
import type { Note, CreateNoteDto, UpdateNoteDto } from '../types/note'

function resolveApiBaseUrl() {
  const configured = import.meta.env.VITE_API_BASE_URL?.trim()
  if (!configured) {
    return 'http://localhost:5187/api'
  }

  const withProtocol = /^https?:\/\//i.test(configured)
    ? configured
    : `https://${configured}`

  return withProtocol.replace(/\/+$/, '').endsWith('/api')
    ? withProtocol.replace(/\/+$/, '')
    : `${withProtocol.replace(/\/+$/, '')}/api`
}

const apiBaseUrl = resolveApiBaseUrl()

const api = axios.create({
  baseURL: apiBaseUrl,
})

api.interceptors.request.use((config) => {
  const userName = localStorage.getItem('notes.username')
  if (userName?.trim()) {
    config.headers['X-User-Name'] = userName.trim()
  }
  return config
})

export const notesApi = {
  getAll: () => api.get<Note[]>('/notes'),
  getById: (id: number) => api.get<Note>(`/notes/${id}`),
  create: (dto: CreateNoteDto) => api.post<Note>('/notes', dto),
  update: (id: number, dto: UpdateNoteDto) => api.put<Note>(`/notes/${id}`, dto),
  delete: (id: number) => api.delete(`/notes/${id}`),
}