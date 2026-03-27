import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { notesApi } from '../api/notes'
import type { Note, CreateNoteDto, UpdateNoteDto } from '../types/note'

export const useNoteStore = defineStore('notes', () => {
  const notes = ref<Note[]>([])
  const searchQuery = ref('')
  const sortOrder = ref<'newest' | 'oldest'>('newest')
  const loading = ref(false)
  const error = ref('')

  const filteredNotes = computed(() => {
    let result = [...notes.value]

    if (searchQuery.value) {
      result = result.filter(n =>
        n.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        n.content?.toLowerCase().includes(searchQuery.value.toLowerCase())
      )
    }

    result.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime()
      const dateB = new Date(b.createdAt).getTime()
      return sortOrder.value === 'newest' ? dateB - dateA : dateA - dateB
    })

    return result
  })

  async function fetchNotes() {
    loading.value = true
    error.value = ''
    try {
      const res = await notesApi.getAll()
      notes.value = res.data
    } catch {
      error.value = 'Failed to load notes. Check API URL and backend status.'
      notes.value = []
    } finally {
      loading.value = false
    }
  }

  async function createNote(dto: CreateNoteDto) {
    error.value = ''
    try {
      const res = await notesApi.create(dto)
      notes.value.push(res.data)
    } catch {
      error.value = 'Failed to create note.'
      throw new Error(error.value)
    }
  }

  async function updateNote(id: number, dto: UpdateNoteDto) {
    error.value = ''
    try {
      const res = await notesApi.update(id, dto)
      const index = notes.value.findIndex(n => n.id === id)
      if (index !== -1) notes.value[index] = res.data
    } catch {
      error.value = 'Failed to update note.'
      throw new Error(error.value)
    }
  }

  async function deleteNote(id: number) {
    error.value = ''
    try {
      await notesApi.delete(id)
      notes.value = notes.value.filter(n => n.id !== id)
    } catch {
      error.value = 'Failed to delete note.'
      throw new Error(error.value)
    }
  }

  return {
    notes, searchQuery, sortOrder, loading, error, filteredNotes,
    fetchNotes, createNote, updateNote, deleteNote
  }
})