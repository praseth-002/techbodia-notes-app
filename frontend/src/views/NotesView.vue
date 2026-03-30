<template>
  <div class="min-h-screen flex flex-col sm:flex-row font-sans bg-gradient-to-br from-pink-50 to-rose-50 text-rose-950">

    <!-- ─── LOGIN ─── -->
    <div v-if="!auth.isLoggedIn" class="w-full flex items-center justify-center min-h-screen px-4">
      <div class="w-full max-w-sm flex flex-col gap-4 px-8 py-10 bg-white border border-pink-200 rounded-2xl shadow-lg shadow-pink-100">
        <h1 class="text-4xl font-bold m-0">Notes</h1>
        <p class="text-sm text-rose-400 m-0">Enter a username to get started.</p>

        <input
          v-model="loginForm.username"
          type="text"
          placeholder="your username"
          class="w-full px-4 py-3 bg-white border border-pink-200 rounded-xl text-sm placeholder:text-pink-300 outline-none transition focus:border-pink-400 focus:ring-2 focus:ring-pink-100"
          @keydown.enter="handleLogin"
        />
        <p v-if="loginError" class="text-xs text-rose-500 m-0">{{ loginError }}</p>

        <button
          @click="handleLogin"
          class="w-full py-3 bg-pink-500 border border-pink-300 rounded-xl text-sm font-semibold text-white cursor-pointer transition hover:bg-pink-400 active:translate-y-px"
        >
          Enter →
        </button>
      </div>
    </div>

    <!-- ─── MAIN APP ─── -->
    <template v-else>

      <!-- SIDEBAR -->
      <aside class="w-56 min-h-screen flex-shrink-0 sticky top-0 h-screen flex flex-col gap-3 px-4 py-6 bg-pink-50 border-r border-pink-200
                    sm:w-56 max-sm:w-full max-sm:h-auto max-sm:min-h-0 max-sm:static max-sm:flex-row max-sm:items-center max-sm:px-3 max-sm:py-2 max-sm:border-r-0 max-sm:border-b max-sm:gap-2">

        <div class="flex flex-col gap-2 max-sm:flex-row max-sm:items-center max-sm:gap-2 max-sm:flex-1 max-sm:min-w-0">
          <span class="text-lg font-bold text-pink-500 max-sm:text-base">Notes</span>
          <span class="text-xs text-rose-400 bg-white border border-pink-200 rounded-full px-2 py-0.5 w-fit max-w-[120px] overflow-hidden text-ellipsis whitespace-nowrap max-sm:text-[10px]">
            {{ auth.username }}
          </span>
        </div>

        <span class="text-xs text-pink-300 max-sm:hidden">
          {{ store.notes.length }} note{{ store.notes.length !== 1 ? 's' : '' }}
        </span>

        <button
          @click="openCreate"
          class="px-4 py-2.5 bg-transparent text-pink-500 border border-pink-200 rounded-xl text-sm font-semibold text-left cursor-pointer transition hover:bg-pink-100
                 max-sm:px-3 max-sm:py-1.5 max-sm:text-xs max-sm:flex-shrink-0"
        >
          + New
        </button>

        <button
          @click="handleLogout"
          class="px-4 py-2 bg-white border border-pink-200 rounded-xl text-xs font-medium text-rose-950 text-left cursor-pointer mt-1 transition hover:bg-pink-50 hover:text-rose-500 hover:border-rose-300
                 max-sm:px-3 max-sm:py-1.5 max-sm:mt-0 max-sm:flex-shrink-0"
        >
          Sign out
        </button>
      </aside>

      <!-- MAIN -->
      <main class="flex-1 m-4 px-8 py-6 max-w-5xl border border-pink-200 rounded-2xl bg-white/90 shadow-md shadow-pink-100
                   max-sm:m-2 max-sm:px-4 max-sm:py-4 max-sm:w-full max-sm:min-w-0">

        <!-- TOOLBAR -->
        <div class="flex gap-3 mb-6 p-2 border border-pink-200 rounded-xl bg-white">
          <input
            v-model="store.searchQuery"
            type="text"
            placeholder="Search notes…"
            class="flex-1 px-4 py-2.5 bg-white border border-pink-200 rounded-xl text-sm placeholder:text-pink-300 outline-none transition focus:border-pink-400 focus:ring-2 focus:ring-pink-100"
          />
          <select
            v-model="store.sortOrder"
            class="px-3 py-2.5 bg-white border border-pink-200 rounded-xl text-sm text-rose-950 outline-none cursor-pointer"
          >
            <option value="newest">Newest first</option>
            <option value="oldest">Oldest first</option>
          </select>
        </div>

        <!-- ERROR -->
        <div v-if="store.error"
             class="mb-4 px-4 py-3 bg-rose-50 border border-rose-200 rounded-xl text-sm text-rose-500">
          {{ store.error }}
        </div>

        <!-- LOADING -->
        <div v-if="store.loading" class="flex flex-col items-center justify-center gap-3 py-20 text-rose-400 text-sm">
          <span class="w-5 h-5 border-2 border-pink-200 border-t-pink-500 rounded-full animate-spin"></span>
          Loading…
        </div>

        <!-- EMPTY -->
        <div v-else-if="store.filteredNotes.length === 0"
             class="flex flex-col items-center justify-center gap-3 py-20 text-rose-400 text-sm">
          <span class="text-4xl text-pink-200">◯</span>
          <p class="m-0">No notes yet.</p>
          <button
            @click="openCreate"
            class="mt-1 px-4 py-2 bg-white border border-pink-200 rounded-xl text-xs text-rose-400 cursor-pointer transition hover:border-pink-400 hover:text-pink-500"
          >
            Create your first note
          </button>
        </div>

        <!-- NOTES GRID -->
        <div v-else class="grid gap-4 [grid-template-columns:repeat(auto-fill,minmax(260px,1fr))]">
          <div
            v-for="(note, i) in store.filteredNotes"
            :key="note.id"
            class="border border-pink-200 rounded-xl bg-white cursor-pointer transition-all animate-[fadeUp_0.3s_ease_both] hover:-translate-y-0.5 hover:border-pink-300 hover:shadow-md hover:shadow-pink-100"
            :style="{ animationDelay: `${i * 40}ms` }"
            @click="openDetail(note)"
          >
            <div class="p-4 flex flex-col gap-2">
              <div class="flex items-center justify-between gap-2">
                <h2 class="text-base font-semibold text-rose-950 m-0">{{ note.title }}</h2>
                <div class="flex gap-2" @click.stop>
                  <button @click="openEdit(note)"        class="bg-transparent border-none text-xs text-rose-400 cursor-pointer transition hover:text-pink-500">Edit</button>
                  <button @click="handleDelete(note.id)" class="bg-transparent border-none text-xs text-rose-400 cursor-pointer transition hover:text-rose-500">Delete</button>
                </div>
              </div>
              <span class="text-xs text-pink-300">{{ formatDate(note.createdAt) }}</span>
            </div>
          </div>
        </div>

      </main>
    </template>

    <!-- ─── DETAIL MODAL ─── -->
    <Transition name="modal">
      <div v-if="selectedNote"
           class="fixed inset-0 bg-white/80 backdrop-blur-sm z-50 flex items-center justify-center p-6"
           @click.self="selectedNote = null">
        <div class="relative w-full max-w-lg bg-white border border-pink-200 rounded-2xl p-8 shadow-xl shadow-pink-100">
          <button class="absolute top-4 right-4 bg-transparent border-none text-pink-300 text-base cursor-pointer transition hover:text-rose-950"
                  @click="selectedNote = null">✕</button>
          <h2 class="text-2xl font-semibold text-rose-950 m-0 mb-2">{{ selectedNote.title }}</h2>
          <div class="flex gap-2 text-xs text-pink-300 mb-4">
            <span>Created {{ formatDate(selectedNote.createdAt) }}</span>
            <span>·</span>
            <span>Updated {{ formatDate(selectedNote.updatedAt) }}</span>
          </div>
          <p class="text-sm text-rose-800 leading-relaxed whitespace-pre-wrap m-0">{{ selectedNote.content ?? 'No content.' }}</p>
        </div>
      </div>
    </Transition>

    <!-- ─── CREATE / EDIT MODAL ─── -->
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

const store        = useNoteStore()
const auth         = useAuthStore()
const showModal    = ref(false)
const editingNote  = ref<Note | null>(null)
const selectedNote = ref<Note | null>(null)
const loginError   = ref('')
const loginForm    = ref({ username: auth.username })

onMounted(async () => {
  if (auth.isLoggedIn) await store.fetchNotes()
})

async function handleLogin() {
  loginError.value = ''
  const username = loginForm.value.username.trim()
  if (!username) { loginError.value = 'Username is required'; return }
  auth.login(username)
  await store.fetchNotes()
  if (store.error) loginError.value = store.error
}

function handleLogout() {
  auth.logout()
  store.notes        = []
  loginForm.value    = { username: '' }
  selectedNote.value = null
  showModal.value    = false
}

function openCreate()           { editingNote.value = null; showModal.value = true }
function openEdit(note: Note)   { editingNote.value = note; showModal.value = true }
function openDetail(note: Note) { selectedNote.value = note }

async function handleSubmit(title: string, content: string | null) {
  try {
    if (editingNote.value) await store.updateNote(editingNote.value.id, { title, content })
    else                   await store.createNote({ title, content })
    showModal.value = false
  } catch {}
}

async function handleDelete(id: number) {
  if (confirm('Delete this note?')) {
    try { await store.deleteNote(id) } catch {}
  }
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' })
}
</script>

<style>
@keyframes fadeUp {
  from { opacity: 0; transform: translateY(8px); }
  to   { opacity: 1; transform: translateY(0); }
}
.modal-enter-active, .modal-leave-active { transition: opacity 0.2s; }
.modal-enter-from,   .modal-leave-to     { opacity: 0; }
</style>
