import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { User, LoginCredentials, SignupCredentials } from '@/types'

const TOKEN_KEY = 'auth_token'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(null)

  const isAuthenticated = computed(() => !!user.value)

  function initialize() {
    const saved = localStorage.getItem(TOKEN_KEY)
    if (saved) {
      token.value = saved
      // TODO: call authService.getCurrentUser() to validate token & hydrate user
      user.value = { username: 'User', email: 'user@example.com' }
    }
  }

  async function login(credentials: LoginCredentials) {
    // TODO: const res = await authService.login(credentials)
    // Mock successful response for now
    const res = {
      user: { username: credentials.email.split('@')[0] ?? credentials.email, email: credentials.email },
      token: 'mock-jwt-token',
    }
    user.value = res.user
    token.value = res.token
    localStorage.setItem(TOKEN_KEY, res.token)
  }

  async function signup(credentials: SignupCredentials) {
    // TODO: const res = await authService.signup(credentials)
    // Mock successful response for now
    const res = {
      user: { username: credentials.username, email: credentials.email },
      token: 'mock-jwt-token',
    }
    user.value = res.user
    token.value = res.token
    localStorage.setItem(TOKEN_KEY, res.token)
  }

  function logout() {
    user.value = null
    token.value = null
    localStorage.removeItem(TOKEN_KEY)
  }

  return { user, token, isAuthenticated, initialize, login, signup, logout }
})
