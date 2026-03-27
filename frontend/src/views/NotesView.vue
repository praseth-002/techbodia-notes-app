<template>
  <div class="min-h-screen bg-gray-50">
    <div class="max-w-4xl mx-auto px-4 py-8">

      <div v-if="!auth.isLoggedIn" class="max-w-md mx-auto bg-white rounded-xl p-6 shadow-sm border border-gray-100">
        <h1 class="text-2xl font-bold text-gray-800 mb-2">Quick Login</h1>
        <p class="text-sm text-gray-500 mb-5">Use any username/password to start. Notes are scoped by username.</p>
        <div class="space-y-3">
          <input
            v-model="loginForm.username"
            type="text"
            placeholder="Username"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <input
            v-model="loginForm.password"
            type="password"
            placeholder="Password"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <p v-if="loginError" class="text-sm text-red-500">{{ loginError }}</p>
          <button
            @click="handleLogin"
            class="w-full bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium"
          >
            Enter Notes App
          </button>
        </div>
      </div>

      <template v-else>

      <!-- Header -->
      <div class="flex items-center justify-between mb-6">
        <div>
          <h1 class="text-3xl font-bold text-gray-800">My Notes</h1>
          <p class="text-sm text-gray-500 mt-1">Signed in as {{ auth.username }}</p>
        </div>
        <div class="flex items-center gap-2">
          <button
            @click="openCreate"
            class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium"
          >
            + New Note
          </button>
          <button
            @click="handleLogout"
            class="border border-gray-300 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-100 font-medium"
          >
            Logout
          </button>
        </div>
      </div>

      <!-- Search + Sort -->
      <div class="flex gap-3 mb-6">
        <input
          v-model="store.searchQuery"
          type="text"
          placeholder="Search notes..."
          class="flex-1 border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <select
          v-model="store.sortOrder"
          class="border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="newest">Newest</option>
          <option value="oldest">Oldest</option>
        </select>
      </div>

      <!-- Loading -->
      <div v-if="store.loading" class="text-center text-gray-400 py-12">Loading...</div>

      <!-- Empty -->
      <div v-else-if="store.filteredNotes.length === 0" class="text-center text-gray-400 py-12">
        No notes found. Create one!
      </div>

      <!-- Notes Grid -->
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-4">
        <div
          v-for="note in store.filteredNotes"
          :key="note.id"
          class="bg-white rounded-xl p-5 shadow-sm border border-gray-100 hover:shadow-md transition cursor-pointer"
          @click="openDetail(note)"
        >
          <div class="flex justify-between items-start">
            <h2 class="font-semibold text-gray-800 text-lg truncate flex-1">{{ note.title }}</h2>
            <div class="flex gap-2 ml-2" @click.stop>
              <button @click="openEdit(note)" class="text-gray-400 hover:text-blue-600 text-sm">Edit</button>
              <button @click="handleDelete(note.id)" class="text-gray-400 hover:text-red-500 text-sm">Delete</button>
            </div>
          </div>
          <p class="text-gray-500 text-sm mt-2 line-clamp-2">{{ note.content ?? 'No content' }}</p>
          <p class="text-gray-400 text-xs mt-3">{{ formatDate(note.createdAt) }}</p>
        </div>
      </div>
      </template>
    </div>

    <!-- Detail Modal -->
    <div
      v-if="selectedNote"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      @click.self="selectedNote = null"
    >
      <div class="bg-white rounded-xl p-6 w-full max-w-md shadow-xl">
        <h2 class="text-2xl font-bold text-gray-800 mb-2">{{ selectedNote.title }}</h2>
        <p class="text-xs text-gray-400 mb-4">
          Created: {{ formatDate(selectedNote.createdAt) }} ·
          Updated: {{ formatDate(selectedNote.updatedAt) }}
        </p>
        <p class="text-gray-600 whitespace-pre-wrap">{{ selectedNote.content ?? 'No content' }}</p>
        <button
          @click="selectedNote = null"
          class="mt-6 w-full border border-gray-300 rounded-lg py-2 text-gray-600 hover:bg-gray-50"
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
const loginForm = ref({ username: auth.username, password: '' })

onMounted(async () => {
  if (auth.isLoggedIn) {
    await store.fetchNotes()
  }
})

async function handleLogin() {
  loginError.value = ''
  const username = loginForm.value.username.trim()
  const password = loginForm.value.password

  if (!username) {
    loginError.value = 'Username is required'
    return
  }

  if (!password) {
    loginError.value = 'Password is required'
    return
  }

  auth.login(username, password)
  await store.fetchNotes()
}

function handleLogout() {
  auth.logout()
  store.notes = []
  loginForm.value = { username: '', password: '' }
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
  if (editingNote.value) {
    await store.updateNote(editingNote.value.id, { title, content })
  } else {
    await store.createNote({ title, content })
  }
  showModal.value = false
}

async function handleDelete(id: number) {
  if (confirm('Delete this note?')) {
    await store.deleteNote(id)
  }
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric', month: 'short', day: 'numeric'
  })
}
</script>