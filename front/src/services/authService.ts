import type { AuthResponse, LoginCredentials, SignupCredentials, User } from '@/types'

// TODO: Wire up to real API endpoints via `api` helper
// import { api } from './api'

export const authService = {
  async login(_credentials: LoginCredentials): Promise<AuthResponse> {
    // TODO: return api.post<AuthResponse>('/auth/login', credentials)
    throw new Error('Not implemented')
  },

  async signup(_credentials: SignupCredentials): Promise<AuthResponse> {
    // TODO: return api.post<AuthResponse>('/auth/signup', credentials)
    throw new Error('Not implemented')
  },

  async logout(): Promise<void> {
    // TODO: return api.post<void>('/auth/logout')
    throw new Error('Not implemented')
  },

  async getCurrentUser(): Promise<User> {
    // TODO: return api.get<User>('/auth/me')
    throw new Error('Not implemented')
  },
}
