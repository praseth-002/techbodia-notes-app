import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

const USERNAME_KEY = 'notes.username'
const LEGACY_PASSWORD_KEY = 'notes.password'

export const useAuthStore = defineStore('auth', () => {
  const username = ref(localStorage.getItem(USERNAME_KEY) ?? '')
  localStorage.removeItem(LEGACY_PASSWORD_KEY)

  const isLoggedIn = computed(() => username.value.trim().length > 0)

  function login(nextUsername: string) {
    username.value = nextUsername.trim()
    localStorage.setItem(USERNAME_KEY, username.value)
    localStorage.removeItem(LEGACY_PASSWORD_KEY)
  }

  function logout() {
    username.value = ''
    localStorage.removeItem(USERNAME_KEY)
    localStorage.removeItem(LEGACY_PASSWORD_KEY)
  }

  return {
    username,
    isLoggedIn,
    login,
    logout
  }
})
