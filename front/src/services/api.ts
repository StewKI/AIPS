const ACCESS_TOKEN = 'auth_token'
const REFRESH_TOKEN = 'refresh_token'

async function parseJsonOrThrow(res: Response) {
  const text = await res.text()
  try {
    return text ? JSON.parse(text) : undefined
  } catch (e) {
    return text
  }
}

async function refreshTokens(): Promise<{ accessToken?: string; refreshToken?: string } | null> {
  const refreshToken = localStorage.getItem(REFRESH_TOKEN)
  if (!refreshToken) return null

  const res = await fetch('/api/User/refresh-login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ refreshToken }),
  })

  if (!res.ok) return null
  const data = await parseJsonOrThrow(res)
  const accessToken = data?.accessToken ?? data?.token ?? null
  const newRefresh = data?.refreshToken ?? null
  if (accessToken) {
    localStorage.setItem(ACCESS_TOKEN, accessToken)
  }
  if (newRefresh) {
    localStorage.setItem(REFRESH_TOKEN, newRefresh)
  }
  return { accessToken, refreshToken: newRefresh }
}

async function request<T = any>(method: string, url: string, body?: any, attemptRefresh = true): Promise<T> {
  const headers: Record<string, string> = {}
  headers['Content-Type'] = 'application/json'

  const token = localStorage.getItem(ACCESS_TOKEN)
  if (token) headers['Authorization'] = `Bearer ${token}`

  const payload = body ?? {}

  const res = await fetch(url, {
    method,
    headers,
    body: body ? JSON.stringify(payload) : null,
  })

  if (res.status === 401 && attemptRefresh) {
    // try refreshing tokens once
    const refreshed = await refreshTokens()
    if (refreshed && refreshed.accessToken) {
      // retry original request with new token
      const retryHeaders: Record<string, string> = {}
      if (body !== undefined && body !== null) retryHeaders['Content-Type'] = 'application/json'
      retryHeaders['Authorization'] = `Bearer ${refreshed.accessToken}`

      const retryRes = await fetch(url, {
        method,
        headers: retryHeaders,
        body: body !== undefined && body !== null ? JSON.stringify(body) : undefined,
      })
      if (!retryRes.ok) {
        const errorBody = await parseJsonOrThrow(retryRes)
        throw Object.assign(new Error(retryRes.statusText || 'Request failed'), { status: retryRes.status, body: errorBody })
      }
      return await parseJsonOrThrow(retryRes)
    }
  }

  if (!res.ok) {
    const errorBody = await parseJsonOrThrow(res)
    throw Object.assign(new Error(res.statusText || 'Request failed'), { status: res.status, body: errorBody })
  }

  return await parseJsonOrThrow(res)
}

export const api = {
  get: <T = any>(url: string) => request<T>('GET', url),
  post: <T = any>(url: string, body?: any) => request<T>('POST', url, body),
  put: <T = any>(url: string, body?: any) => request<T>('PUT', url, body),
  delete: <T = any>(url: string, body?: any) => request<T>('DELETE', url, body),
}
