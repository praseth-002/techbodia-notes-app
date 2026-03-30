<template>
  <div class="fixed inset-0 bg-white/80 backdrop-blur-sm z-50 flex items-center justify-center p-6" @click.self="$emit('close')">
    <div class="relative w-full max-w-lg bg-white border border-pink-200 rounded-2xl p-8 shadow-xl shadow-pink-100">

      <button
        class="absolute top-4 right-4 bg-transparent border-none text-pink-300 text-base cursor-pointer transition hover:text-rose-950"
        @click="$emit('close')"
      >✕</button>

      <h2 class="text-2xl font-semibold text-rose-950 m-0 mb-6">
        {{ isEditing ? 'Edit Note' : 'New Note' }}
      </h2>

      <!-- TITLE -->
      <div class="mb-5">
        <label class="block text-sm font-medium text-rose-950 mb-2">Title *</label>
        <input
          v-model="form.title"
          type="text"
          placeholder="Note title"
          class="w-full px-4 py-3 bg-white border border-pink-200 rounded-xl text-sm placeholder:text-pink-300 outline-none transition focus:border-pink-400 focus:ring-2 focus:ring-pink-100"
        />
        <p v-if="error" class="text-xs text-rose-500 mt-1.5 mb-0">{{ error }}</p>
      </div>

      <!-- CONTENT -->
      <div class="mb-5">
        <label class="block text-sm font-medium text-rose-950 mb-2">Content</label>
        <textarea
          v-model="form.content"
          placeholder="Note content (optional)"
          rows="5"
          class="w-full px-4 py-3 bg-white border border-pink-200 rounded-xl text-sm placeholder:text-pink-300 outline-none resize-none transition focus:border-pink-400 focus:ring-2 focus:ring-pink-100"
        />
      </div>

      <!-- ACTIONS -->
      <div class="flex gap-3 justify-end mt-6">
        <button
          @click="$emit('close')"
          class="px-5 py-2.5 bg-white border border-pink-200 rounded-xl text-sm font-medium text-rose-400 cursor-pointer transition hover:border-pink-400 hover:text-pink-500"
        >
          Cancel
        </button>
        <button
          @click="submit"
          class="px-5 py-2.5 bg-pink-500 border border-pink-300 rounded-xl text-sm font-semibold text-white cursor-pointer transition hover:bg-pink-400 active:translate-y-px"
        >
          {{ isEditing ? 'Save Changes' : 'Create Note' }}
        </button>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import type { Note } from '../types/note'

const props = defineProps<{
  note?: Note | null
}>()

const emit = defineEmits<{
  close: []
  submit: [title: string, content: string | null]
}>()

const isEditing = computed(() => !!props.note)
const error     = ref('')
const form      = ref({ title: '', content: '' })

watch(
  () => props.note,
  (note) => {
    form.value  = { title: note?.title ?? '', content: note?.content ?? '' }
    error.value = ''
  },
  { immediate: true }
)

watch(
  () => form.value.title,
  () => { if (error.value) error.value = '' }
)

function submit() {
  if (!form.value.title.trim()) { error.value = 'Title is required'; return }
  emit('submit', form.value.title.trim(), form.value.content || null)
}
</script>
