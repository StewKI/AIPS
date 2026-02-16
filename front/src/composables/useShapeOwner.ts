import { ref, watch, type Ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { fetchUser } from '@/services/userService'

function avatarColorFromString(str: string): string {
  let hash = 5381
  for (let i = 0; i < str.length; i++) {
    hash = ((hash << 5) + hash + str.charCodeAt(i)) & 0xffffffff
  }
  return `hsl(${Math.abs(hash) % 360}, 65%, 55%)`
}

export function useShapeOwner(ownerId: Ref<string | null>) {
  const auth = useAuthStore()

  const displayName = ref('...')
  const avatarColor = ref('#888')
  const isLoading = ref(false)

  async function resolve(id: string | null) {
    if (!id) {
      displayName.value = '...'
      avatarColor.value = '#888'
      isLoading.value = false
      return
    }

    if (id === auth.user?.userId) {
      displayName.value = 'Me'
      avatarColor.value = avatarColorFromString(auth.user.username || 'Me')
      isLoading.value = false
      return
    }

    isLoading.value = true
    try {
      const user = await fetchUser(id)
      displayName.value = user.username || 'Unknown'
      avatarColor.value = avatarColorFromString(user.username || id)
    } catch {
      displayName.value = 'Unknown'
      avatarColor.value = '#888'
    } finally {
      isLoading.value = false
    }
  }

  watch(ownerId, resolve, { immediate: true })

  return { displayName, avatarColor, isLoading }
}
