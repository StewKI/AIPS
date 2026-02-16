<script setup lang="ts">
import { ref, computed } from 'vue'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import type { Rectangle } from '@/types/whiteboard.ts'
import SvgRectangle from '@/components/whiteboard/shapes/SvgRectangle.vue'
import SvgDraftRect from '@/components/whiteboard/shapes/SvgDraftRect.vue'

const store = useWhiteboardStore()

const isDragging = ref(false)
const dragStart = ref({ x: 0, y: 0 })
const dragEnd = ref({ x: 0, y: 0 })

const draftRect = computed(() => {
  if (!isDragging.value) return null
  const x = Math.min(dragStart.value.x, dragEnd.value.x)
  const y = Math.min(dragStart.value.y, dragEnd.value.y)
  const width = Math.abs(dragEnd.value.x - dragStart.value.x)
  const height = Math.abs(dragEnd.value.y - dragStart.value.y)
  return { x, y, width, height }
})

function onMouseDown(e: MouseEvent) {
  if (store.selectedTool !== 'rectangle') return
  const svg = (e.currentTarget as SVGSVGElement)
  const rect = svg.getBoundingClientRect()
  const x = e.clientX - rect.left
  const y = e.clientY - rect.top
  dragStart.value = { x, y }
  dragEnd.value = { x, y }
  isDragging.value = true
}

function onMouseMove(e: MouseEvent) {
  if (!isDragging.value) return
  const svg = (e.currentTarget as SVGSVGElement)
  const rect = svg.getBoundingClientRect()
  dragEnd.value = {
    x: e.clientX - rect.left,
    y: e.clientY - rect.top,
  }
}

function onMouseUp() {
  if (!isDragging.value) return
  isDragging.value = false

  const x1 = Math.min(dragStart.value.x, dragEnd.value.x)
  const y1 = Math.min(dragStart.value.y, dragEnd.value.y)
  const x2 = Math.max(dragStart.value.x, dragEnd.value.x)
  const y2 = Math.max(dragStart.value.y, dragEnd.value.y)

  if (x2 - x1 < 5 && y2 - y1 < 5) return

  const rectangle: Rectangle = {
    id: crypto.randomUUID(),
    ownerId: '00000000-0000-0000-0000-000000000000',
    position: { x: Math.round(x1), y: Math.round(y1) },
    endPosition: { x: Math.round(x2), y: Math.round(y2) },
    color: '#4f9dff',
    borderThickness: 2,
  }

  store.addRectangle(rectangle)
}
</script>

<template>
  <svg
    class="whiteboard-canvas"
    @mousedown="onMouseDown"
    @mousemove="onMouseMove"
    @mouseup="onMouseUp"
    @mouseleave="onMouseUp"
  >
    <SvgRectangle
      v-for="rect in store.whiteboard?.rectangles"
      :key="rect.id"
      :rectangle="rect"
    />
    <SvgDraftRect
      v-if="draftRect"
      :x="draftRect.x"
      :y="draftRect.y"
      :width="draftRect.width"
      :height="draftRect.height"
    />
  </svg>
</template>

<style scoped>
.whiteboard-canvas {
  flex: 1;
  width: 100%;
  height: 100%;
  background-color: #1a1a2e;
  cursor: crosshair;
  display: block;
}
</style>
