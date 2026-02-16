import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Rectangle, ShapeTool, Whiteboard } from '@/types/whiteboard.ts'
import { whiteboardHubService } from '@/services/whiteboardHubService.ts'

export const useWhiteboardStore = defineStore('whiteboard', () => {
  const whiteboard = ref<Whiteboard | null>(null)
  const selectedTool = ref<ShapeTool>('rectangle')
  const isConnected = ref(false)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  async function joinWhiteboard(id: string) {
    isLoading.value = true
    error.value = null

    try {
      await whiteboardHubService.connect()
      isConnected.value = true

      whiteboardHubService.onInitWhiteboard((wb) => {
        whiteboard.value = wb
        isLoading.value = false
      })

      whiteboardHubService.onAddedRectangle((rectangle) => {
        whiteboard.value?.rectangles.push(rectangle)
      })

      whiteboardHubService.onJoined((userId) => {
        console.log('User joined:', userId)
      })

      whiteboardHubService.onLeaved((userId) => {
        console.log('User left:', userId)
      })

      await whiteboardHubService.joinWhiteboard(id)
    } catch (e: any) {
      error.value = e?.message ?? 'Failed to join whiteboard'
      isLoading.value = false
    }
  }

  async function leaveWhiteboard() {
    if (!whiteboard.value) return

    try {
      await whiteboardHubService.leaveWhiteboard(whiteboard.value.whiteboardId)
    } catch (e) {
      console.warn('Leave request failed', e)
    }

    whiteboardHubService.offAll()
    await whiteboardHubService.disconnect()

    whiteboard.value = null
    isConnected.value = false
    selectedTool.value = 'rectangle'
    error.value = null
  }

  async function addRectangle(rectangle: Rectangle) {
    whiteboard.value?.rectangles.push(rectangle)

    try {
      await whiteboardHubService.addRectangle(rectangle)
    } catch (e: any) {
      console.error('Failed to send rectangle', e)
    }
  }

  function selectTool(tool: ShapeTool) {
    selectedTool.value = tool
  }

  return {
    whiteboard,
    selectedTool,
    isConnected,
    isLoading,
    error,
    joinWhiteboard,
    leaveWhiteboard,
    addRectangle,
    selectTool,
  }
})
