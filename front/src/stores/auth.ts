import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { User, LoginCredentials, SignupCredentials, AuthResponse } from '@/types'
import { useWhiteboardStore } from '@/stores/whiteboards'
import { authService } from '@/services/authService'

const ACCESS_TOKEN = 'auth_token'
const REFRESH_TOKEN = 'refresh_token'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!user.value)

  let refreshPromise: Promise<void> | null = null

  function setTokens(access: string | null, refresh: string | null) {
    accessToken.value = access
    if (access) {
      localStorage.setItem(ACCESS_TOKEN, access)
    } else {
      localStorage.removeItem(ACCESS_TOKEN)
    }

    if (refresh) {
      localStorage.setItem(REFRESH_TOKEN, refresh)
    } else if (refresh === null) {
      localStorage.removeItem(REFRESH_TOKEN)
    }
  }

  async function tryRefresh(): Promise<void> {
    if (refreshPromise) return refreshPromise

    const refreshToken = localStorage.getItem(REFRESH_TOKEN)
    if (!refreshToken) {
      throw new Error('No refresh token')
    }

    refreshPromise = (async () => {
      try {
        const res = await authService.refreshLogin(refreshToken)
        setTokens(res.accessToken ?? null, res.refreshToken ?? null)
      } finally {
        refreshPromise = null
      }
    })()

    return refreshPromise
  }

  async function initialize() {
    isLoading.value = true
    error.value = null
    const saved = localStorage.getItem(ACCESS_TOKEN)
    const savedRefresh = localStorage.getItem(REFRESH_TOKEN)

    if (saved) {
      accessToken.value = saved
      try {
        user.value = await authService.getMe()
        isLoading.value = false
        return
      } catch (e) {
        if (savedRefresh) {
          try {
            await tryRefresh()
            user.value = await authService.getMe()
            isLoading.value = false
            return
          } catch (err) {
            console.warn('Token refresh failed during initialize', err)
          }
        }
      }
    }


    setTokens(null, null)
    user.value = null
    isLoading.value = false
  }

  async function login(credentials: LoginCredentials) {
    isLoading.value = true
    error.value = null
    try {
      const res: AuthResponse = await authService.login(credentials)

      setTokens(res.accessToken ?? null, res.refreshToken ?? null)

      try {
        user.value = await authService.getMe()
      } catch (e) {
        console.error('Logged in but failed to fetch user profile', e)

        user.value = null
      }
    } catch (e: any) {
      error.value = e?.message ?? 'Login failed'
      throw e
    } finally {
      isLoading.value = false
    }
  }

  async function signup(credentials: SignupCredentials) {
    isLoading.value = true
    error.value = null

    try {
      const res: AuthResponse = await authService.signup(credentials)
      setTokens(res.accessToken ?? null, res.refreshToken ?? null)
      try {
        user.value = await authService.getMe()
      } catch (e) {

        user.value = null
      }
    } catch (e: any) {
      error.value = e?.message ?? 'Signup failed'
      throw e
    } finally {
      isLoading.value = false
    }
  }

  async function logout(allDevices = false) {
    const whiteboardStore = useWhiteboardStore()
    isLoading.value = true
    error.value = null
    try {
      if (allDevices) await authService.logoutAll()
      else await authService.logout(localStorage.getItem(REFRESH_TOKEN)!)
    } catch (e) {
      console.warn('Logout request failed', e)
    } finally {
      setTokens(null, null)
      whiteboardStore.clearWhiteboards()
      user.value = null
      isLoading.value = false
    }
  }

  return {
    user,
    accessToken,
    isAuthenticated,
    isLoading,
    error,
    initialize,
    login,
    signup,
    logout,
  }
})
