import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

const USERNAME_KEY = 'notes.username'
const PASSWORD_KEY = 'notes.password'

export const useAuthStore = defineStore('auth', () => {
  const username = ref(localStorage.getItem(USERNAME_KEY) ?? '')
  const password = ref(localStorage.getItem(PASSWORD_KEY) ?? '')

  const isLoggedIn = computed(() => username.value.trim().length > 0 && password.value.length > 0)

  function login(nextUsername: string, nextPassword: string) {
    username.value = nextUsername.trim()
    password.value = nextPassword
    localStorage.setItem(USERNAME_KEY, username.value)
    localStorage.setItem(PASSWORD_KEY, password.value)
  }

  function logout() {
    username.value = ''
    password.value = ''
    localStorage.removeItem(USERNAME_KEY)
    localStorage.removeItem(PASSWORD_KEY)
  }

  return {
    username,
    isLoggedIn,
    login,
    logout
  }
})
