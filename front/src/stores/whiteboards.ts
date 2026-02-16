import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Whiteboard } from '@/types'
import { whiteboardService } from '@/services/whiteboardService'

export const useWhiteboardStore = defineStore('whiteboards', () => {
  const ownedWhiteboards = ref<Whiteboard[]>([])
  const recentWhiteboards = ref<Whiteboard[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  async function getWhiteboardHistory() {
    isLoading.value = true
    error.value = null
    try {
      ownedWhiteboards.value = await whiteboardService.getWhiteboardHistory()
    } catch (err: any) {
      error.value = err.message ?? 'Failed to load whiteboards'
    } finally {
      isLoading.value = false
    }
  }

  async function getRecentWhiteboards() {
    isLoading.value = true
    error.value = null
    try {
      recentWhiteboards.value = await whiteboardService.getRecentWhiteboards()
    } catch (err: any) {
      error.value = err.message ?? 'Failed to load whiteboards'
    } finally {
      isLoading.value = false
    }
  }

  function clearWhiteboards() {
    ownedWhiteboards.value = []
    recentWhiteboards.value = []
  }

  return {
    ownedWhiteboards: ownedWhiteboards,
    recentWhiteboards: recentWhiteboards,
    isLoading,
    error,
    getWhiteboardHistory: getWhiteboardHistory,
    getRecentWhiteboards: getRecentWhiteboards,
    clearWhiteboards: clearWhiteboards
  }
})
