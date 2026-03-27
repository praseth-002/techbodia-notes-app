<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4">
    <div class="w-full max-w-md rounded-2xl bg-white p-6 shadow-2xl shadow-slate-900/20">
      <h2 class="mb-4 text-xl font-bold text-slate-900">{{ isEditing ? 'Edit Note' : 'New Note' }}</h2>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700">Title *</label>
        <input
          v-model="form.title"
          type="text"
          placeholder="Note title"
          class="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-800 placeholder:text-slate-400 outline-none transition focus:border-sky-400 focus:ring-2 focus:ring-sky-200"
        />
        <p v-if="error" class="text-red-500 text-sm mt-1">{{ error }}</p>
      </div>

      <div class="mb-6">
        <label class="mb-1 block text-sm font-medium text-slate-700">Content</label>
        <textarea
          v-model="form.content"
          placeholder="Note content (optional)"
          rows="4"
          class="w-full resize-none rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-800 placeholder:text-slate-400 outline-none transition focus:border-sky-400 focus:ring-2 focus:ring-sky-200"
        />
      </div>

      <div class="flex justify-end gap-3">
        <button
          @click="$emit('close')"
          class="rounded-lg border border-slate-300 px-4 py-2 text-slate-600 transition hover:bg-slate-50"
        >
          Cancel
        </button>
        <button
          @click="submit"
          class="rounded-lg bg-sky-600 px-4 py-2 text-white transition hover:bg-sky-700"
        >
          {{ isEditing ? 'Save Changes' : 'Create Note' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { Note } from '../types/note'

const props = defineProps<{
  note?: Note | null
}>()

const emit = defineEmits<{
  close: []
  submit: [title: string, content: string | null]
}>()

const isEditing = !!props.note
const error = ref('')

const form = ref({
  title: props.note?.title ?? '',
  content: props.note?.content ?? '',
})

function submit() {
  if (!form.value.title.trim()) {
    error.value = 'Title is required'
    return
  }
  emit('submit', form.value.title.trim(), form.value.content || null)
}
</script>
