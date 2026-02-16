import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { Arrow, Line, Rectangle, Shape, ShapeTool, ShapeType, TextShape, Whiteboard } from '@/types/whiteboard.ts'
import { whiteboardHubService } from '@/services/whiteboardHubService.ts'

export const useWhiteboardStore = defineStore('whiteboard', () => {
  const whiteboard = ref<Whiteboard | null>(null)
  const selectedTool = ref<ShapeTool>('hand')
  const isConnected = ref(false)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const selectedShapeId = ref<string | null>(null)
  const selectedShapeType = ref<ShapeType | null>(null)
  const toolColor = ref('#4f9dff')
  const toolThickness = ref(2)
  const toolTextSize = ref(24)

  const selectedShape = computed(() => {
    if (!selectedShapeId.value || !selectedShapeType.value || !whiteboard.value) return null
    switch (selectedShapeType.value) {
      case 'rectangle': return whiteboard.value.rectangles.find(s => s.id === selectedShapeId.value) ?? null
      case 'arrow': return whiteboard.value.arrows.find(s => s.id === selectedShapeId.value) ?? null
      case 'line': return whiteboard.value.lines.find(s => s.id === selectedShapeId.value) ?? null
      case 'textShape': return whiteboard.value.textShapes.find(s => s.id === selectedShapeId.value) ?? null
      default: return null
    }
  })

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

      whiteboardHubService.onAddedArrow((arrow) => {
        whiteboard.value?.arrows.push(arrow)
      })

      whiteboardHubService.onAddedLine((line) => {
        whiteboard.value?.lines.push(line)
      })

      whiteboardHubService.onAddedTextShape((textShape) => {
        whiteboard.value?.textShapes.push(textShape)
      })

      whiteboardHubService.onMovedShape((command) => {
        applyMoveShape(command.shapeId, command.newPositionX, command.newPositionY)
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
    selectedTool.value = 'hand'
    deselectShape()
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

  async function addArrow(arrow: Arrow) {
    whiteboard.value?.arrows.push(arrow)

    try {
      await whiteboardHubService.addArrow(arrow)
    } catch (e: any) {
      console.error('Failed to send arrow', e)
    }
  }

  async function addLine(line: Line) {
    whiteboard.value?.lines.push(line)

    try {
      await whiteboardHubService.addLine(line)
    } catch (e: any) {
      console.error('Failed to send line', e)
    }
  }

  async function addTextShape(textShape: TextShape) {
    whiteboard.value?.textShapes.push(textShape)

    try {
      await whiteboardHubService.addTextShape(textShape)
    } catch (e: any) {
      console.error('Failed to send text shape', e)
    }
  }

  function selectTool(tool: ShapeTool) {
    selectedTool.value = tool
    deselectShape()
  }

  function selectShape(id: string, type: ShapeType) {
    selectedShapeId.value = id
    selectedShapeType.value = type
  }

  function deselectShape() {
    selectedShapeId.value = null
    selectedShapeType.value = null
  }

  function applyMoveShape(shapeId: string, newPosX: number, newPosY: number) {
    const wb = whiteboard.value
    if (!wb) return
    const all: Shape[] = [...wb.rectangles, ...wb.arrows, ...wb.lines, ...wb.textShapes]
    const shape = all.find(s => s.id === shapeId)
    if (!shape) return

    const dx = newPosX - shape.position.x
    const dy = newPosY - shape.position.y
    shape.position.x = newPosX
    shape.position.y = newPosY

    if ('endPosition' in shape) {
      (shape as any).endPosition.x += dx
      ;(shape as any).endPosition.y += dy
    }
  }

  function setToolColor(color: string) { toolColor.value = color }
  function setToolThickness(thickness: number) { toolThickness.value = thickness }
  function setToolTextSize(size: number) { toolTextSize.value = size }

  return {
    whiteboard,
    selectedTool,
    isConnected,
    isLoading,
    error,
    selectedShapeId,
    selectedShapeType,
    selectedShape,
    toolColor,
    toolThickness,
    toolTextSize,
    joinWhiteboard,
    leaveWhiteboard,
    addRectangle,
    addArrow,
    addLine,
    addTextShape,
    selectTool,
    selectShape,
    deselectShape,
    applyMoveShape,
    setToolColor,
    setToolThickness,
    setToolTextSize,
  }
})
