<script setup lang="ts">
import { ref, computed, nextTick } from 'vue'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import type { Arrow, Line, Rectangle, TextShape } from '@/types/whiteboard.ts'
import type { ShapeType } from '@/types/whiteboard.ts'
import SvgRectangle from '@/components/whiteboard/shapes/SvgRectangle.vue'
import SvgDraftRect from '@/components/whiteboard/shapes/SvgDraftRect.vue'
import SvgArrow from '@/components/whiteboard/shapes/SvgArrow.vue'
import SvgDraftArrow from '@/components/whiteboard/shapes/SvgDraftArrow.vue'
import SvgLine from '@/components/whiteboard/shapes/SvgLine.vue'
import SvgDraftLine from '@/components/whiteboard/shapes/SvgDraftLine.vue'
import SvgTextShape from '@/components/whiteboard/shapes/SvgTextShape.vue'
import SvgDraftText from '@/components/whiteboard/shapes/SvgDraftText.vue'
import ShapeOwnerLabel from '@/components/whiteboard/ShapeOwnerLabel.vue'
import ShapeOwnerTooltip from '@/components/whiteboard/ShapeOwnerTooltip.vue'
import { whiteboardHubService } from '@/services/whiteboardHubService.ts'
import { useAuthStore } from '@/stores/auth'

const store = useWhiteboardStore()
const auth = useAuthStore()

const isMoving = ref(false)
const moveStartMouse = ref({ x: 0, y: 0 })
const moveStartPos = ref({ x: 0, y: 0 })
const moveStartEndPos = ref({ x: 0, y: 0 })
const movingShapeId = ref<string | null>(null)
let lastMoveHubTime = 0

const hoveredShapeId = ref<string | null>(null)
const hoveredOwnerId = ref<string | null>(null)
const cursorX = ref(0)
const cursorY = ref(0)

function findOwnerIdByShapeId(id: string): string | null {
  const wb = store.whiteboard
  if (!wb) return null
  const all = [
    ...wb.rectangles,
    ...wb.arrows,
    ...wb.lines,
    ...wb.textShapes,
  ]
  return all.find(s => s.id === id)?.ownerId ?? null
}

const selectedShapeAnchor = computed(() => {
  const shape = store.selectedShape
  if (!shape) return null
  const type = store.selectedShapeType

  if (type === 'rectangle') {
    const r = shape as Rectangle
    return {
      x: (r.position.x + r.endPosition.x) / 2,
      y: Math.min(r.position.y, r.endPosition.y),
    }
  }
  if (type === 'arrow' || type === 'line') {
    const s = shape as Arrow | Line
    return {
      x: (s.position.x + s.endPosition.x) / 2,
      y: Math.min(s.position.y, s.endPosition.y),
    }
  }
  if (type === 'textShape') {
    const t = shape as TextShape
    return { x: t.position.x, y: t.position.y }
  }
  return null
})

const isDragging = ref(false)
const dragStart = ref({ x: 0, y: 0 })
const dragEnd = ref({ x: 0, y: 0 })

const textInputState = ref({ active: false, x: 0, y: 0, value: '', textSize: 24 })
const textInputRef = ref<HTMLInputElement | null>(null)

const draftRect = computed(() => {
  if (!isDragging.value || store.selectedTool !== 'rectangle') return null
  const x = Math.min(dragStart.value.x, dragEnd.value.x)
  const y = Math.min(dragStart.value.y, dragEnd.value.y)
  const width = Math.abs(dragEnd.value.x - dragStart.value.x)
  const height = Math.abs(dragEnd.value.y - dragStart.value.y)
  return { x, y, width, height }
})

const draftArrow = computed(() => {
  if (!isDragging.value || store.selectedTool !== 'arrow') return null
  return {
    x1: dragStart.value.x,
    y1: dragStart.value.y,
    x2: dragEnd.value.x,
    y2: dragEnd.value.y,
  }
})

const draftLine = computed(() => {
  if (!isDragging.value || store.selectedTool !== 'line') return null
  return {
    x1: dragStart.value.x,
    y1: dragStart.value.y,
    x2: dragEnd.value.x,
    y2: dragEnd.value.y,
  }
})

const showDraftText = computed(() => {
  return textInputState.value.active && textInputState.value.value.length > 0
})

function getCanvasCoords(e: MouseEvent) {
  const el = (e.currentTarget as HTMLElement).querySelector('svg') ?? (e.currentTarget as SVGSVGElement)
  const rect = el.getBoundingClientRect()
  return { x: e.clientX - rect.left, y: e.clientY - rect.top }
}

function onMouseDown(e: MouseEvent) {
  if (textInputState.value.active) return

  if (store.selectedTool === 'hand') {
    const shapeEl = (e.target as Element).closest('[data-shape-id]')
    if (shapeEl) {
      const shapeId = shapeEl.getAttribute('data-shape-id')!
      const shapeType = shapeEl.getAttribute('data-shape-type')! as ShapeType
      store.selectShape(shapeId, shapeType)

      const ownerId = findOwnerIdByShapeId(shapeId)
      if (ownerId === auth.user?.userId) {
        const { x, y } = getCanvasCoords(e)
        moveStartMouse.value = { x, y }

        const shape = store.selectedShape
        if (shape) {
          moveStartPos.value = { x: shape.position.x, y: shape.position.y }
          if ('endPosition' in shape) {
            moveStartEndPos.value = { x: (shape as any).endPosition.x, y: (shape as any).endPosition.y }
          }
          isMoving.value = true
          movingShapeId.value = shapeId
        }
      }
    } else {
      store.deselectShape()
    }
    return
  }

  if (store.selectedTool === 'text') {
    e.preventDefault()
    const { x, y } = getCanvasCoords(e)
    textInputState.value = { active: true, x, y, value: '', textSize: store.toolTextSize }
    nextTick(() => textInputRef.value?.focus())
    return
  }

  if (store.selectedTool !== 'rectangle' && store.selectedTool !== 'arrow' && store.selectedTool !== 'line') return
  const svg = (e.target as Element).closest('svg') as SVGSVGElement
  const rect = svg.getBoundingClientRect()
  const x = e.clientX - rect.left
  const y = e.clientY - rect.top
  dragStart.value = { x, y }
  dragEnd.value = { x, y }
  isDragging.value = true
}

function onMouseMove(e: MouseEvent) {
  if (isMoving.value && movingShapeId.value) {
    const { x, y } = getCanvasCoords(e)
    const dx = x - moveStartMouse.value.x
    const dy = y - moveStartMouse.value.y
    const newPosX = moveStartPos.value.x + dx
    const newPosY = moveStartPos.value.y + dy
    store.applyMoveShape(movingShapeId.value, newPosX, newPosY)

    hoveredShapeId.value = null
    hoveredOwnerId.value = null

    const now = Date.now()
    if (now - lastMoveHubTime >= 30) {
      whiteboardHubService.moveShape({ shapeId: movingShapeId.value, newPositionX: newPosX, newPositionY: newPosY })
      lastMoveHubTime = now
    }
    return
  }

  if (isDragging.value) {
    const svg = (e.target as Element).closest('svg') as SVGSVGElement
    if (!svg) return
    const rect = svg.getBoundingClientRect()
    dragEnd.value = {
      x: e.clientX - rect.left,
      y: e.clientY - rect.top,
    }
    return
  }

  if (store.selectedTool === 'hand') {
    const shapeEl = (e.target as Element).closest('[data-shape-id]')
    if (shapeEl) {
      const id = shapeEl.getAttribute('data-shape-id')!
      if (id !== store.selectedShapeId) {
        hoveredShapeId.value = id
        hoveredOwnerId.value = findOwnerIdByShapeId(id)
        cursorX.value = e.clientX
        cursorY.value = e.clientY
        return
      }
    }
  }

  hoveredShapeId.value = null
  hoveredOwnerId.value = null
}

function onMouseUp() {
  if (isMoving.value && movingShapeId.value) {
    const shape = store.whiteboard
      ? [...store.whiteboard.rectangles, ...store.whiteboard.arrows, ...store.whiteboard.lines, ...store.whiteboard.textShapes].find(s => s.id === movingShapeId.value)
      : null
    const dx = Math.abs(moveStartPos.value.x - (shape?.position.x ?? moveStartPos.value.x))
    const dy = Math.abs(moveStartPos.value.y - (shape?.position.y ?? moveStartPos.value.y))
    if (dx > 2 || dy > 2) {
      whiteboardHubService.placeShape({
        shapeId: movingShapeId.value,
        newPositionX: shape?.position.x ?? moveStartPos.value.x,
        newPositionY: shape?.position.y ?? moveStartPos.value.y,
      })
    }
    isMoving.value = false
    movingShapeId.value = null
    return
  }

  if (!isDragging.value) return
  isDragging.value = false

  const sx = dragStart.value.x
  const sy = dragStart.value.y
  const ex = dragEnd.value.x
  const ey = dragEnd.value.y

  const dx = Math.abs(ex - sx)
  const dy = Math.abs(ey - sy)
  if (dx < 5 && dy < 5) return

  if (store.selectedTool === 'rectangle') {
    const rectangle: Rectangle = {
      id: crypto.randomUUID(),
      ownerId: auth.user?.userId ?? '',
      position: { x: Math.round(Math.min(sx, ex)), y: Math.round(Math.min(sy, ey)) },
      endPosition: { x: Math.round(Math.max(sx, ex)), y: Math.round(Math.max(sy, ey)) },
      color: store.toolColor,
      borderThickness: store.toolThickness,
    }
    store.addRectangle(rectangle)
    store.selectTool('hand')
  } else if (store.selectedTool === 'arrow') {
    const arrow: Arrow = {
      id: crypto.randomUUID(),
      ownerId: auth.user?.userId ?? '',
      position: { x: Math.round(sx), y: Math.round(sy) },
      endPosition: { x: Math.round(ex), y: Math.round(ey) },
      color: store.toolColor,
      thickness: store.toolThickness,
    }
    store.addArrow(arrow)
    store.selectTool('hand')
  } else if (store.selectedTool === 'line') {
    const line: Line = {
      id: crypto.randomUUID(),
      ownerId: auth.user?.userId ?? '',
      position: { x: Math.round(sx), y: Math.round(sy) },
      endPosition: { x: Math.round(ex), y: Math.round(ey) },
      color: store.toolColor,
      thickness: store.toolThickness,
    }
    store.addLine(line)
    store.selectTool('hand')
  }
}

function commitTextShape() {
  if (!textInputState.value.active) return
  const hasText = textInputState.value.value.trim().length > 0
  if (hasText) {
    const textShape: TextShape = {
      id: crypto.randomUUID(),
      ownerId: auth.user?.userId ?? '',
      position: { x: Math.round(textInputState.value.x), y: Math.round(textInputState.value.y) },
      color: store.toolColor,
      textValue: textInputState.value.value,
      textSize: textInputState.value.textSize,
    }
    store.addTextShape(textShape)
  }
  cancelTextInput()
  if (hasText) {
    store.selectTool('hand')
  }
}

function cancelTextInput() {
  textInputState.value = { active: false, x: 0, y: 0, value: '', textSize: store.toolTextSize }
}

function onMouseLeave() {
  if (isMoving.value && movingShapeId.value) {
    const shape = store.whiteboard
      ? [...store.whiteboard.rectangles, ...store.whiteboard.arrows, ...store.whiteboard.lines, ...store.whiteboard.textShapes].find(s => s.id === movingShapeId.value)
      : null
    const dx = Math.abs(moveStartPos.value.x - (shape?.position.x ?? moveStartPos.value.x))
    const dy = Math.abs(moveStartPos.value.y - (shape?.position.y ?? moveStartPos.value.y))
    if (dx > 2 || dy > 2) {
      whiteboardHubService.placeShape({
        shapeId: movingShapeId.value,
        newPositionX: shape?.position.x ?? moveStartPos.value.x,
        newPositionY: shape?.position.y ?? moveStartPos.value.y,
      })
    }
    isMoving.value = false
    movingShapeId.value = null
  }
  onMouseUp()
  hoveredShapeId.value = null
  hoveredOwnerId.value = null
}

function onTextInputKeydown(e: KeyboardEvent) {
  if (e.key === 'Enter') {
    e.preventDefault()
    commitTextShape()
  } else if (e.key === 'Escape') {
    e.preventDefault()
    cancelTextInput()
  }
}
</script>

<template>
  <div
    class="canvas-container"
    @mousedown="onMouseDown"
    @mousemove="onMouseMove"
    @mouseup="onMouseUp"
    @mouseleave="onMouseLeave"
  >
    <svg :class="['whiteboard-canvas', store.selectedTool === 'hand' ? 'select-mode' : 'draw-mode']">
      <SvgRectangle
        v-for="rect in store.whiteboard?.rectangles"
        :key="rect.id"
        :rectangle="rect"
        :is-selected="store.selectedShapeId === rect.id"
      />
      <SvgArrow
        v-for="arrow in store.whiteboard?.arrows"
        :key="arrow.id"
        :arrow="arrow"
        :is-selected="store.selectedShapeId === arrow.id"
      />
      <SvgLine
        v-for="line in store.whiteboard?.lines"
        :key="line.id"
        :line="line"
        :is-selected="store.selectedShapeId === line.id"
      />
      <SvgTextShape
        v-for="ts in store.whiteboard?.textShapes"
        :key="ts.id"
        :text-shape="ts"
        :is-selected="store.selectedShapeId === ts.id"
      />
      <ShapeOwnerLabel
        v-if="store.selectedShape && selectedShapeAnchor"
        :owner-id="store.selectedShape.ownerId"
        :anchor-x="selectedShapeAnchor.x"
        :anchor-y="selectedShapeAnchor.y"
      />
      <SvgDraftRect
        v-if="draftRect"
        :x="draftRect.x"
        :y="draftRect.y"
        :width="draftRect.width"
        :height="draftRect.height"
        :color="store.toolColor"
        :thickness="store.toolThickness"
      />
      <SvgDraftArrow
        v-if="draftArrow"
        :x1="draftArrow.x1"
        :y1="draftArrow.y1"
        :x2="draftArrow.x2"
        :y2="draftArrow.y2"
        :color="store.toolColor"
        :thickness="store.toolThickness"
      />
      <SvgDraftLine
        v-if="draftLine"
        :x1="draftLine.x1"
        :y1="draftLine.y1"
        :x2="draftLine.x2"
        :y2="draftLine.y2"
        :color="store.toolColor"
        :thickness="store.toolThickness"
      />
      <SvgDraftText
        v-if="showDraftText"
        :x="textInputState.x"
        :y="textInputState.y"
        :text-value="textInputState.value"
        :text-size="textInputState.textSize"
        :color="store.toolColor"
      />
    </svg>
    <ShapeOwnerTooltip
      v-if="hoveredOwnerId && hoveredShapeId"
      :owner-id="hoveredOwnerId"
      :cursor-x="cursorX"
      :cursor-y="cursorY"
    />
    <input
      v-if="textInputState.active"
      ref="textInputRef"
      v-model="textInputState.value"
      class="text-input-overlay"
      :style="{
        left: textInputState.x + 'px',
        top: textInputState.y + 'px',
        fontSize: textInputState.textSize + 'px',
        borderColor: store.toolColor,
        color: store.toolColor,
      }"
      @blur="commitTextShape"
      @keydown="onTextInputKeydown"
    />
  </div>
</template>

<style scoped>
.canvas-container {
  position: relative;
  flex: 1;
  width: 100%;
  height: 100%;
}

.whiteboard-canvas {
  width: 100%;
  height: 100%;
  background-color: #1a1a2e;
  display: block;
}

.whiteboard-canvas.draw-mode {
  cursor: crosshair;
}

.whiteboard-canvas.draw-mode :deep([data-shape-id]) {
  pointer-events: none;
}

.whiteboard-canvas.select-mode {
  cursor: default;
}

.whiteboard-canvas.select-mode :deep([data-shape-id]) {
  pointer-events: all;
  cursor: pointer;
}

.text-input-overlay {
  position: absolute;
  background: rgba(31, 31, 47, 0.95);
  border: 2px solid #4f9dff;
  border-radius: 4px;
  padding: 4px 8px;
  color: #4f9dff;
  font-family: Arial, sans-serif;
  outline: none;
  min-width: 150px;
  z-index: 10;
}
</style>
