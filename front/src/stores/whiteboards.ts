import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Whiteboard } from '@/types'
import { whiteboardService } from '@/services/whiteboardService'

export const useWhiteboardsStore = defineStore('whiteboards', () => {
  const ownedWhiteboards = ref<Whiteboard[]>([])
  const recentWhiteboards = ref<Whiteboard[]>([])
  const currentWhiteboard = ref<Whiteboard | null>(null)
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

  async function createNewWhiteboard(title: string): Promise<string> {
    let newWhiteboard: Whiteboard;
    isLoading.value = true
    error.value = null

    try {
      const newId = await whiteboardService.createNewWhiteboard(title)
      newWhiteboard = await whiteboardService.getWhiteboardById(newId)
    } catch (err: any) {
      error.value = err.message ?? 'Failed to create whiteboard'
      throw err
    } finally {
      isLoading.value = false
    }

    ownedWhiteboards.value.push(newWhiteboard)
    return newWhiteboard.id;
  }

  async function deleteWhiteboard(id: string): Promise<void> {
    isLoading.value = true
    error.value = null

    try {
      await whiteboardService.deleteWhiteboard(id)
      ownedWhiteboards.value = ownedWhiteboards.value.filter(wb => wb.id !== id)
    } catch (err: any) {
      error.value = err.message ?? 'Failed to delete whiteboard'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  function getCurrentWhiteboard(): Whiteboard | null {
    return currentWhiteboard.value
  }

  function selectWhiteboard(whiteboardId: string) {
    currentWhiteboard.value =
      ownedWhiteboards.value.find(wb => wb.id === whiteboardId) ||
      recentWhiteboards.value.find(wb => wb.id === whiteboardId) ||
      null
  }

  function deselectWhiteboard() {
    currentWhiteboard.value = null
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
    createNewWhiteboard: createNewWhiteboard,
    deleteWhiteboard: deleteWhiteboard,
    getCurrentWhiteboard: getCurrentWhiteboard,
    selectWhiteboard: selectWhiteboard,
    deselectWhiteboard: deselectWhiteboard,
    clearWhiteboards: clearWhiteboards,
  }
})
