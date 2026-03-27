<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-edit">
      <button class="modal-close" @click="$emit('close')">✕</button>
      <h2 class="modal-title">{{ isEditing ? 'Edit Note' : 'New Note' }}</h2>

      <div class="form-group">
        <label class="form-label">Title *</label>
        <input
          v-model="form.title"
          type="text"
          placeholder="Note title"
          class="form-input"
        />
        <p v-if="error" class="form-error">{{ error }}</p>
      </div>

      <div class="form-group">
        <label class="form-label">Content</label>
        <textarea
          v-model="form.content"
          placeholder="Note content (optional)"
          rows="5"
          class="form-textarea"
        />
      </div>

      <div class="form-actions">
        <button @click="$emit('close')" class="btn-cancel">Cancel</button>
        <button @click="submit" class="btn-submit">{{ isEditing ? 'Save Changes' : 'Create Note' }}</button>
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
const error = ref('')

const form = ref({
  title: props.note?.title ?? '',
  content: props.note?.content ?? '',
})

watch(
  () => props.note,
  (note) => {
    form.value = {
      title: note?.title ?? '',
      content: note?.content ?? '',
    }
    error.value = ''
  },
  { immediate: true }
)

watch(
  () => form.value.title,
  () => {
    if (error.value) {
      error.value = ''
    }
  }
)

function submit() {
  if (!form.value.title.trim()) {
    error.value = 'Title is required'
    return
  }
  emit('submit', form.value.title.trim(), form.value.content || null)
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

.modal-overlay {
  position: fixed;
  inset: 0;
  background: #ffff;
  z-index: 50;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.modal-edit {
  background: var(--surface);
  border: 1.5px solid var(--border);
  border-radius: 16px;
  padding: 32px;
  width: 100%;
  max-width: 480px;
  margin: 0 auto;
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
  transition: color 0.2s;
}
.modal-close:hover {
  color: var(--text);
}

.modal-title {
  font-size: 24px;
  color: var(--text);
  margin: 0 0 24px;
  font-weight: 600;
}

.form-group {
  margin-bottom: 20px;
}

.form-label {
  display: block;
  font-size: 13px;
  font-weight: 500;
  color: var(--text);
  margin-bottom: 8px;
}

.form-input,
.form-textarea {
  width: calc(100% - 4px);
  background: var(--surface);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 12px 14px;
  color: var(--text);
  font-size: 14px;
  font-family: inherit;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s;
  box-sizing: border-box;
}

.form-input:focus,
.form-textarea:focus {
  border-color: var(--accent);
  box-shadow: 0 0 0 3px rgba(255, 31, 143, 0.16);
}

.form-input::placeholder,
.form-textarea::placeholder {
  color: var(--muted-soft);
}

.form-textarea {
  resize: none;
}

.form-error {
  font-size: 12px;
  color: var(--danger);
  margin: 6px 0 0;
}

.form-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 28px;
}

.btn-cancel {
  background: var(--surface);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 10px 20px;
  color: var(--muted);
  font-size: 13px;
  font-weight: 500;
  cursor: pointer;
  transition: border-color 0.2s, color 0.2s;
}
.btn-cancel:hover {
  border-color: var(--accent);
  color: var(--accent);
}

.btn-submit {
  background: var(--accent);
  color: var(--text);
  border: 1.5px solid #efb0cd;
  border-radius: 14px;
  padding: 10px 20px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s, transform 0.1s;
}
.btn-submit:hover {
  background: var(--accent-strong);
}
.btn-submit:active {
  transform: translateY(1px);
}
</style>
