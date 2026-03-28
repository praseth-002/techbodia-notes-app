<template>
  <div class="app-shell">

    <!-- LOGIN SCREEN -->
    <div v-if="!auth.isLoggedIn" class="login-wrap">
      <div class="login-card">
        <h1 class="login-title">Notes</h1>
        <p class="login-sub">Enter a username to get started.</p>
        <input
          v-model="loginForm.username"
          type="text"
          placeholder="your username"
          class="login-input"
          @keydown.enter="handleLogin"
        />
        <p v-if="loginError" class="login-error">{{ loginError }}</p>
        <button @click="handleLogin" class="login-btn">Enter →</button>
      </div>
    </div>

    <!-- MAIN APP -->
    <template v-else>

      <!-- SIDEBAR -->
      <aside class="sidebar">
        <div class="sidebar-top">
          <span class="brand">Notes</span>
          <span class="user-badge">User: {{ auth.username }}</span>
        </div>
        <div class="sidebar-meta">
          <span>{{ store.notes.length }} note{{ store.notes.length !== 1 ? 's' : '' }}</span>
        </div>
        <button @click="openCreate" class="new-btn">+ New note</button>
        <button @click="handleLogout" class="logout-btn">Sign out</button>
      </aside>

      <!-- MAIN CONTENT -->
      <main class="main">

        <!-- TOOLBAR -->
        <div class="toolbar">
          <input
            v-model="store.searchQuery"
            type="text"
            placeholder="Search notes…"
            class="search-input"
          />
          <select v-model="store.sortOrder" class="sort-select">
            <option value="newest">Newest first</option>
            <option value="oldest">Oldest first</option>
          </select>
        </div>

        <!-- ERROR -->
        <div v-if="store.error" class="error-bar">{{ store.error }}</div>

        <!-- LOADING -->
        <div v-if="store.loading" class="state-msg">
          <span class="spinner"></span> Loading…
        </div>

        <!-- EMPTY -->
        <div v-else-if="store.filteredNotes.length === 0" class="state-msg empty">
          <span class="empty-icon">◯</span>
          <p>No notes yet.</p>
          <button @click="openCreate" class="empty-cta">Create your first note</button>
        </div>

        <!-- NOTES GRID -->
        <div v-else class="notes-grid">
          <div
            v-for="(note, i) in store.filteredNotes"
            :key="note.id"
            class="note-card"
            :style="{ animationDelay: `${i * 40}ms` }"
            @click="openDetail(note)"
          >
            <div class="note-card-inner">
              <div class="note-head">
                <h2 class="note-title">{{ note.title }}</h2>
                <div class="note-actions" @click.stop>
                  <button @click="openEdit(note)" class="action-btn edit-btn">Edit</button>
                  <button @click="handleDelete(note.id)" class="action-btn del-btn">Delete</button>
                </div>
              </div>
              <div class="note-footer">
                <span class="note-date">{{ formatDate(note.createdAt) }}</span>
              </div>
            </div>
          </div>
        </div>

      </main>
    </template>

    <!-- DETAIL MODAL -->
    <Transition name="modal">
      <div v-if="selectedNote" class="modal-overlay" @click.self="selectedNote = null">
        <div class="modal">
          <button class="modal-close" @click="selectedNote = null">✕</button>
          <h2 class="modal-title">{{ selectedNote.title }}</h2>
          <div class="modal-meta">
            <span>Created {{ formatDate(selectedNote.createdAt) }}</span>
            <span class="dot">·</span>
            <span>Updated {{ formatDate(selectedNote.updatedAt) }}</span>
          </div>
          <p class="modal-content">{{ selectedNote.content ?? 'No content.' }}</p>
        </div>
      </div>
    </Transition>

    <!-- CREATE / EDIT MODAL -->
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
  store.notes = []
  loginForm.value = { username: '' }
  selectedNote.value = null
  showModal.value = false
}

function openCreate() { editingNote.value = null; showModal.value = true }
function openEdit(note: Note) { editingNote.value = note; showModal.value = true }
function openDetail(note: Note) { selectedNote.value = note }

async function handleSubmit(title: string, content: string | null) {
  try {
    if (editingNote.value) await store.updateNote(editingNote.value.id, { title, content })
    else await store.createNote({ title, content })
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

<style scoped>
:root {
  --bg: #fff7fb;
  --surface: #ffffff;
  --surface-soft: #fff0f7;
  --border: #e0a4c4;
  --accent: #ff1f8f;
  --accent-strong: #db0f74;
  --text: #3a1130;
  --muted: #8d5e79;
  --muted-soft: #bc89a4;
  --danger: #d54572;
}

.app-shell {
  min-height: 100vh;
  background: radial-gradient(circle at top right, #ffe8f4 0%, var(--bg) 48%);
  color: var(--text);
  font-family: 'Poppins', 'Segoe UI', sans-serif;
  display: flex;
}

.login-wrap {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: transparent;
}

.login-card {
  width: 100%;
  max-width: 380px;
  padding: 40px 34px;
  border: 1.5px solid var(--border);
  border-radius: 18px;
  background: var(--surface);
  box-shadow: 0 18px 40px rgba(255, 31, 143, 0.12);
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.login-mark { font-size: 30px; color: var(--accent); line-height: 1; }
.login-title { font-size: 34px; margin: 0; color: var(--text); }
.login-sub { font-size: 13px; color: var(--muted); line-height: 1.55; margin: 0; }

.login-input {
  background: #fff;
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 12px 14px;
  color: var(--text);
  font-size: 14px;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s;
}
.login-input:focus {
  border-color: var(--accent);
  box-shadow: 0 0 0 3px rgba(255, 31, 143, 0.16);
}
.login-input::placeholder { color: var(--muted-soft); }
.login-error { font-size: 12px; color: var(--danger); margin: 0; }

.login-btn {
  background: var(--accent);
  color: var(--text);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 12px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s, transform 0.1s;
}
.login-btn:hover { background: #fff0f7; }
.login-btn:active { transform: translateY(1px); }

.sidebar {
  width: 230px;
  min-height: 100vh;
  border-right: 1.5px solid var(--border);
  background: var(--surface-soft);
  padding: 26px 18px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  position: sticky;
  top: 0;
  height: 100vh;
  flex-shrink: 0;
}

.sidebar-top {
  display: flex;
  flex-direction: column;
  gap: 8px;
  /* margin-bottom: 6px; */
}

.brand { font-size: 20px; color: var(--accent); font-weight: 700; }

.user-badge {
  font-size: 12px;
  color: var(--muted);
  background: #fff;
  border-radius: 999px;
  border: 1.5px solid var(--border);
  width: fit-content;
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.new-btn {
  background: transparent;
  color: var(--accent);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 11px 14px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  text-align: left;
  transition: background 0.2s, color 0.2s;
}
.new-btn:hover { background: #fff0f7; }

.sidebar-meta {
  font-size: 11px;
  color: var(--muted-soft);
  /* margin-top: 8px; */
}

.logout-btn {
  background: var(--surface);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 9px 14px;
  color: var(--text);
  font-size: 12px;
  font-weight: 500;
  cursor: pointer;
  text-align: left;
  margin-top: 4px;
  transition: color 0.2s, background 0.2s;
}
.logout-btn:hover { background: #fff0f7; color: var(--danger); border-color: var(--danger); }

.main {
  flex: 1;
  margin: 16px;
  padding: 28px 30px;
  max-width: 980px;
  border: 1.5px solid var(--border);
  border-radius: 18px;
  background: rgba(255, 255, 255, 0.88);
  box-shadow: 0 14px 30px rgba(255, 31, 143, 0.08);
}

.toolbar {
  display: flex;
  gap: 12px;
  margin-bottom: 24px;
  padding: 10px;
  border: 1.5px solid var(--border);
  border-radius: 12px;
  background: #fff;
}

.search-input,
.sort-select {
  background: #fff;
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  color: var(--text);
  font-size: 13px;
  outline: none;
}

.search-input {
  flex: 1;
  padding: 10px 14px;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.search-input:focus {
  border-color: var(--accent);
  box-shadow: 0 0 0 3px rgba(255, 31, 143, 0.16);
}

.search-input::placeholder { color: var(--muted-soft); }
.sort-select { padding: 10px 12px; cursor: pointer; }

.error-bar {
  background: #fff1f5;
  border: 1.5px solid #ef9ab9;
  border-radius: 10px;
  padding: 12px 16px;
  font-size: 13px;
  color: var(--danger);
  margin-bottom: 18px;
}

.state-msg {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 12px;
  padding: 80px 0;
  color: var(--muted);
  font-size: 13px;
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid var(--border);
  border-top-color: var(--accent);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  display: inline-block;
}
@keyframes spin { to { transform: rotate(360deg); } }

.empty-icon { font-size: 32px; color: var(--border); }

.empty-cta {
  margin-top: 6px;
  background: #fff;
  border: 1.5px solid var(--border);
  border-radius: 10px;
  padding: 9px 16px;
  color: var(--muted);
  font-size: 12px;
  cursor: pointer;
  transition: border-color 0.2s, color 0.2s;
}
.empty-cta:hover { border-color: var(--accent); color: var(--accent); }

.notes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 16px;
}

.note-card {
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  background: #fff;
  cursor: pointer;
  transition: border-color 0.2s, transform 0.15s, box-shadow 0.2s;
  animation: fadeUp 0.3s ease both;
}
.note-card:hover {
  border-color: #ef9ab9;
  transform: translateY(-2px);
  box-shadow: 0 12px 24px rgba(255, 31, 143, 0.12);
}

@keyframes fadeUp {
  from { opacity: 0; transform: translateY(8px); }
  to { opacity: 1; transform: translateY(0); }
}

.note-card-inner { padding: 18px; display: flex; flex-direction: column; gap: 10px; }
.note-title { font-size: 16px; color: var(--text); margin: 0; }

.note-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 10px;
}

.note-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 2px;
}

.note-date { font-size: 11px; color: var(--muted-soft); }

.note-actions {
  display: flex;
  gap: 10px;
  opacity: 1;
}

.action-btn {
  background: none;
  border: none;
  font-size: 11px;
  cursor: pointer;
  padding: 2px 0;
  transition: color 0.15s;
}
.edit-btn { color: var(--muted); }
.edit-btn:hover { color: var(--accent); }
.del-btn { color: var(--muted); }
.del-btn:hover { color: var(--danger); }

.modal-overlay {
  position: fixed;
  inset: 0;
  background: #fff;
  z-index: 50;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.modal {
  background: #fff;
  border: 1.5px solid var(--border);
  border-radius: 16px;
  padding: 32px;
  width: 100%;
  max-width: 480px;
  position: relative;
  box-shadow: 0 22px 40px rgba(255, 31, 143, 0.2);
}

.modal-close {
  position: absolute;
  top: 16px;
  right: 16px;
  background: none;
  border: none;
  color: var(--muted-soft);
  font-size: 16px;
  cursor: pointer;
}
.modal-close:hover { color: var(--text); }

.modal-title {
  font-size: 24px;
  color: var(--text);
  margin: 0 0 8px;
}

.modal-meta {
  display: flex;
  gap: 8px;
  font-size: 11px;
  color: var(--muted-soft);
  margin-bottom: 16px;
}
.dot { color: var(--border); }

.modal-content {
  font-size: 15px;
  color: #5c2a47;
  line-height: 1.7;
  white-space: pre-wrap;
  margin: 0;
}

.modal-enter-active,
.modal-leave-active { transition: opacity 0.2s; }
.modal-enter-from,
.modal-leave-to { opacity: 0; }

@media (max-width: 640px) {
  .app-shell { flex-direction: column; }

  .sidebar {
    width: 100%;
    height: auto;
    min-height: unset;
    position: static;
    flex-direction: row;
    flex-wrap: wrap;
    padding: 10px;
    border-right: none;
    border-bottom: 1.5px solid var(--border);
    gap: 6px;
  }

  .sidebar-top {
    flex-direction: row;
    align-items: center;
    gap: 4px;
    min-width: 0;
  }

  .brand { font-size: 16px; }

  .user-badge {
    font-size: 10px;
    padding: 2px 6px;
    min-width: 0;
    flex-shrink: 1;
  }

  .new-btn {
    padding: 8px 10px;
    font-size: 12px;
    flex-shrink: 0;
  }

  .logout-btn {
    padding: 8px 10px;
    font-size: 11px;
    flex-shrink: 0;
  }

  .sidebar-meta { display: none; }
  .main {
    margin: 10px;
    padding: 18px 14px;
  }
}
</style>