<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-xl p-6 w-full max-w-md shadow-xl">
      <h2 class="text-xl font-bold mb-4">{{ isEditing ? 'Edit Note' : 'New Note' }}</h2>

      <div class="mb-4">
        <label class="block text-sm font-medium text-gray-700 mb-1">Title *</label>
        <input
          v-model="form.title"
          type="text"
          placeholder="Note title"
          class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <p v-if="error" class="text-red-500 text-sm mt-1">{{ error }}</p>
      </div>

      <div class="mb-6">
        <label class="block text-sm font-medium text-gray-700 mb-1">Content</label>
        <textarea
          v-model="form.content"
          placeholder="Note content (optional)"
          rows="4"
          class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
        />
      </div>

      <div class="flex justify-end gap-3">
        <button
          @click="$emit('close')"
          class="px-4 py-2 rounded-lg border border-gray-300 text-gray-600 hover:bg-gray-50"
        >
          Cancel
        </button>
        <button
          @click="submit"
          class="px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700"
        >
          {{ isEditing ? 'Save Changes' : 'Create Note' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
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
