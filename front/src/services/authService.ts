import type { AuthResponse, LoginCredentials, SignupCredentials, User } from '@/types'
import { api } from './api'

function normalizeAuthResponse(raw: any): AuthResponse {
  const accessToken = raw?.accessToken ?? raw?.AccessToken ?? raw?.token ?? raw?.Token ?? raw?.access_token ?? null
  const refreshToken = raw?.refreshToken ?? raw?.RefreshToken ?? raw?.refresh_token ?? null
  return { accessToken, refreshToken }
}

export const authService = {
  async login(credentials: LoginCredentials): Promise<AuthResponse> {
    const raw = await api.post<any>('/api/User/login', credentials)
    return normalizeAuthResponse(raw)
  },

  async refreshLogin(refreshToken: string): Promise<AuthResponse> {
    const raw = await api.post<any>('/api/User/refresh-login', { refreshToken })
    return normalizeAuthResponse(raw)
  },

  async signup(credentials: SignupCredentials): Promise<AuthResponse> {
    const raw = await api.post<any>('/api/User/signup', credentials)
    return normalizeAuthResponse(raw)
  },

  async logout(refreshToken: string): Promise<void> {
    return api.delete<void>('/api/User/logout', {refreshToken: refreshToken})
  },

  async logoutAll(): Promise<void> {
    return api.delete<void>('/api/User/logout-all')
  },

  async getMe() : Promise<User> {
    const raw = await api.get<any>('/api/User/me')
    // backend User may have fields like userName / UserName and email / Email
    const username = raw?.userName ?? raw?.UserName ?? raw?.username ?? raw?.name ?? ''
    const email = raw?.email ?? raw?.Email ?? ''
    return { username, email }
  },
}
