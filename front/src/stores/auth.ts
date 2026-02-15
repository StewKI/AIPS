import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { User, LoginCredentials, SignupCredentials, AuthResponse } from '@/types'
import { authService } from '@/services/authService'

const ACCESS_TOKEN = 'auth_token'
const REFRESH_TOKEN = 'refresh_token'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!user.value)

  // single-flight promise for refresh to avoid concurrent refresh requests
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
      // explicit null means remove
      localStorage.removeItem(REFRESH_TOKEN)
    }
  }

  async function tryRefresh(): Promise<void> {
    // if a refresh is already in progress, return that promise
    if (refreshPromise) return refreshPromise

    const refreshToken = localStorage.getItem(REFRESH_TOKEN)
    if (!refreshToken) {
      // nothing to do
      throw new Error('No refresh token')
    }

    refreshPromise = (async () => {
      try {
        const res = await authService.refreshLogin(refreshToken)
        // API expected to return { accessToken, refreshToken }
        setTokens(res.accessToken ?? null, res.refreshToken ?? null)
      } finally {
        // clear so subsequent calls can create a new promise
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
        // token might be expired; try refresh if we have refresh token
        if (savedRefresh) {
          try {
            await tryRefresh()
            user.value = await authService.getMe()
            isLoading.value = false
            return
          } catch (err) {
            // refresh failed - fallthrough to clearing auth
            console.warn('Token refresh failed during initialize', err)
          }
        }
      }
    }

    // not authenticated or refresh failed
    setTokens(null, null)
    user.value = null
    isLoading.value = false
  }

  async function login(credentials: LoginCredentials) {
    isLoading.value = true
    error.value = null
    try {
      const res: AuthResponse = await authService.login(credentials)
      // expect AuthResponse to have accessToken and refreshToken
      setTokens(res.accessToken ?? null, res.refreshToken ?? null)

      try {
        user.value = await authService.getMe()
      } catch (e) {
        console.error('Logged in but failed to fetch user profile', e)
        // keep tokens but clear user
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
        // keep tokens but no profile
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
    isLoading.value = true
    error.value = null
    try {
      if (allDevices) await authService.logoutAll()
      else await authService.logout(localStorage.getItem(REFRESH_TOKEN)!)
    } catch (e) {
      // ignore network errors on logout
      console.warn('Logout request failed', e)
    } finally {
      setTokens(null, null)
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
