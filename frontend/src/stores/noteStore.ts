import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { notesApi } from '../api/notes'
import type { Note, CreateNoteDto, UpdateNoteDto } from '../types/note'

export const useNoteStore = defineStore('notes', () => {
  const notes = ref<Note[]>([])
  const searchQuery = ref('')
  const sortOrder = ref<'newest' | 'oldest'>('newest')
  const loading = ref(false)

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
    const res = await notesApi.getAll()
    notes.value = res.data
    loading.value = false
  }

  async function createNote(dto: CreateNoteDto) {
    const res = await notesApi.create(dto)
    notes.value.push(res.data)
  }

  async function updateNote(id: number, dto: UpdateNoteDto) {
    const res = await notesApi.update(id, dto)
    const index = notes.value.findIndex(n => n.id === id)
    if (index !== -1) notes.value[index] = res.data
  }

  async function deleteNote(id: number) {
    await notesApi.delete(id)
    notes.value = notes.value.filter(n => n.id !== id)
  }

  return {
    notes, searchQuery, sortOrder, loading, filteredNotes,
    fetchNotes, createNote, updateNote, deleteNote
  }
})