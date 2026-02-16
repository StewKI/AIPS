import type { User } from '@/types'
import { api } from './api'

const userCache = new Map<string, User>()
const pendingRequests = new Map<string, Promise<User>>()

export async function fetchUser(userId: string): Promise<User> {
  const cached = userCache.get(userId)
  if (cached) return cached

  const pending = pendingRequests.get(userId)
  if (pending) return pending

  const promise = api.get<any>(`/api/User?userId=${userId}`).then((raw) => {
    const id = raw?.userId ?? raw?.UserId ?? raw?.id ?? raw?.Id ?? userId
    const username = raw?.userName ?? raw?.UserName ?? raw?.username ?? raw?.name ?? ''
    const email = raw?.email ?? raw?.Email ?? ''
    const user: User = { userId: id, username, email }
    userCache.set(userId, user)
    pendingRequests.delete(userId)
    return user
  }).catch((err) => {
    pendingRequests.delete(userId)
    throw err
  })

  pendingRequests.set(userId, promise)
  return promise
}

export function clearUserCache() {
  userCache.clear()
  pendingRequests.clear()
}
