<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-50 via-sky-50 to-indigo-50">
    <div class="mx-auto max-w-5xl px-4 py-8 sm:px-6 lg:px-8">

      <div v-if="!auth.isLoggedIn" class="mx-auto max-w-md rounded-2xl border border-white/70 bg-white/90 p-6 shadow-xl shadow-slate-200/60 backdrop-blur">
        <h1 class="mb-2 text-2xl font-bold text-slate-900">Quick Login</h1>
        <p class="mb-5 text-sm text-slate-600">Use any username to start. Notes are scoped by username.</p>
        <div class="space-y-3">
          <input
            v-model="loginForm.username"
            type="text"
            placeholder="Username"
            class="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-800 placeholder:text-slate-400 outline-none transition focus:border-sky-400 focus:ring-2 focus:ring-sky-200"
          />
          <p v-if="loginError" class="text-sm text-red-500">{{ loginError }}</p>
          <button
            @click="handleLogin"
            class="w-full rounded-lg bg-slate-900 px-4 py-2 font-medium text-white transition hover:bg-slate-800"
          >
            Enter Notes App
          </button>
        </div>
      </div>

      <template v-else>

      <!-- Header -->
      <div class="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900">My Notes</h1>
          <p class="mt-1 text-sm text-slate-600">Signed in as <span class="font-semibold text-slate-800">{{ auth.username }}</span></p>
        </div>
        <div class="flex items-center gap-2">
          <button
            @click="openCreate"
            class="rounded-lg bg-sky-600 px-4 py-2 font-medium text-white transition hover:bg-sky-700"
          >
            + New Note
          </button>
          <button
            @click="handleLogout"
            class="rounded-lg border border-slate-300 bg-white px-4 py-2 font-medium text-slate-700 transition hover:bg-slate-100"
          >
            Logout
          </button>
        </div>
      </div>

      <!-- Search + Sort -->
      <div class="mb-6 flex flex-col gap-3 sm:flex-row">
        <input
          v-model="store.searchQuery"
          type="text"
          placeholder="Search notes..."
          class="flex-1 rounded-lg border border-slate-300 bg-white px-4 py-2 text-slate-800 placeholder:text-slate-400 outline-none transition focus:border-sky-400 focus:ring-2 focus:ring-sky-200"
        />
        <select
          v-model="store.sortOrder"
          class="rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-700 outline-none transition focus:border-sky-400 focus:ring-2 focus:ring-sky-200"
        >
          <option value="newest">Newest</option>
          <option value="oldest">Oldest</option>
        </select>
      </div>

      <div v-if="store.error" class="mb-4 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700 shadow-sm">
        {{ store.error }}
      </div>

      <!-- Loading -->
      <div v-if="store.loading" class="py-16 text-center text-slate-500">
        <div class="mx-auto mb-3 h-7 w-7 animate-spin rounded-full border-2 border-slate-300 border-t-sky-600"></div>
        Loading notes...
      </div>

      <!-- Empty -->
      <div v-else-if="store.filteredNotes.length === 0" class="rounded-2xl border border-dashed border-slate-300 bg-white/70 py-16 text-center text-slate-500">
        <p class="text-lg font-medium text-slate-700">No notes found</p>
        <p class="mt-1 text-sm">Create one to get started.</p>
      </div>

      <!-- Notes Grid -->
      <div v-else class="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <div
          v-for="note in store.filteredNotes"
          :key="note.id"
          class="cursor-pointer rounded-2xl border border-slate-200 bg-white p-5 shadow-sm shadow-slate-200/60 transition hover:-translate-y-0.5 hover:shadow-md"
          @click="openDetail(note)"
        >
          <div class="flex justify-between items-start">
            <h2 class="flex-1 truncate text-lg font-semibold text-slate-900">{{ note.title }}</h2>
            <div class="flex gap-2 ml-2" @click.stop>
              <button @click="openEdit(note)" class="text-sm text-slate-400 transition hover:text-sky-600">Edit</button>
              <button @click="handleDelete(note.id)" class="text-sm text-slate-400 transition hover:text-red-500">Delete</button>
            </div>
          </div>
          <p class="mt-2 text-xs text-slate-400">Created: {{ formatDate(note.createdAt) }}</p>
        </div>
      </div>
      </template>
    </div>

    <!-- Detail Modal -->
    <div
      v-if="selectedNote"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4"
      @click.self="selectedNote = null"
    >
      <div class="w-full max-w-md rounded-2xl bg-white p-6 shadow-2xl shadow-slate-900/20">
        <h2 class="mb-2 text-2xl font-bold text-slate-900">{{ selectedNote.title }}</h2>
        <p class="mb-4 text-xs text-slate-400">
          Created: {{ formatDate(selectedNote.createdAt) }} ·
          Updated: {{ formatDate(selectedNote.updatedAt) }}
        </p>
        <p class="whitespace-pre-wrap text-slate-700">{{ selectedNote.content ?? 'No content' }}</p>
        <button
          @click="selectedNote = null"
          class="mt-6 w-full rounded-lg border border-slate-300 py-2 text-slate-600 transition hover:bg-slate-50"
        >
          Close
        </button>
      </div>
    </div>

    <!-- Create/Edit Modal -->
    <NoteModal
      v-if="showModal"
      :note="editingNote"
      @close="showModal = false"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useNoteStore } from '../stores/noteStore'
import { useAuthStore } from '../stores/authStore'
import NoteModal from '../components/NoteModal.vue'
import type { Note } from '../types/note'

const store = useNoteStore()
const auth = useAuthStore()
const showModal = ref(false)
const editingNote = ref<Note | null>(null)
const selectedNote = ref<Note | null>(null)
const loginError = ref('')
const loginForm = ref({ username: auth.username })

onMounted(async () => {
  if (auth.isLoggedIn) {
    await store.fetchNotes()
  }
})

async function handleLogin() {
  loginError.value = ''
  const username = loginForm.value.username.trim()

  if (!username) {
    loginError.value = 'Username is required'
    return
  }

  auth.login(username)
  await store.fetchNotes()
  if (store.error) {
    loginError.value = store.error
  }
}

function handleLogout() {
  auth.logout()
  store.notes = []
  loginForm.value = { username: '' }
  selectedNote.value = null
  showModal.value = false
}

function openCreate() {
  editingNote.value = null
  showModal.value = true
}

function openEdit(note: Note) {
  editingNote.value = note
  showModal.value = true
}

function openDetail(note: Note) {
  selectedNote.value = note
}

async function handleSubmit(title: string, content: string | null) {
  try {
    if (editingNote.value) {
      await store.updateNote(editingNote.value.id, { title, content })
    } else {
      await store.createNote({ title, content })
    }
    showModal.value = false
  } catch {
    // Store already holds the displayable error message.
  }
}

async function handleDelete(id: number) {
  if (confirm('Delete this note?')) {
    try {
      await store.deleteNote(id)
    } catch {
      // Store already holds the displayable error message.
    }
  }
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric', month: 'short', day: 'numeric'
  })
}
</script>