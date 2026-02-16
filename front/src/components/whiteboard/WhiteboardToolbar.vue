<script setup lang="ts">
import { computed } from 'vue'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import type { ShapeTool, Arrow, Line, Rectangle, TextShape } from '@/types/whiteboard.ts'

const store = useWhiteboardStore()
const emit = defineEmits<{ leave: [] }>()

const tools: { name: ShapeTool; label: string; icon: string; enabled: boolean }[] = [
  { name: 'hand', label: 'Select', icon: '\u270B', enabled: true },
  { name: 'rectangle', label: 'Rectangle', icon: '\u25AD', enabled: true },
  { name: 'arrow', label: 'Arrow', icon: '\u2192', enabled: true },
  { name: 'line', label: 'Line', icon: '\u2571', enabled: true },
  { name: 'text', label: 'Text', icon: 'T', enabled: true },
]

const colors = ['#4f9dff', '#ff4f4f', '#4fff4f', '#ffff4f', '#ff4fff', '#ffffff', '#ff9f4f', '#4fffff']

const isReadOnly = computed(() => store.selectedTool === 'hand' && !!store.selectedShape)

const showProperties = computed(() => {
  if (['rectangle', 'arrow', 'line', 'text'].includes(store.selectedTool)) return true
  if (store.selectedTool === 'hand' && store.selectedShape) return true
  return false
})

const showThickness = computed(() => {
  if (['rectangle', 'arrow', 'line'].includes(store.selectedTool)) return true
  if (isReadOnly.value && store.selectedShapeType && ['rectangle', 'arrow', 'line'].includes(store.selectedShapeType)) return true
  return false
})

const showTextSize = computed(() => {
  if (store.selectedTool === 'text') return true
  if (isReadOnly.value && store.selectedShapeType === 'textShape') return true
  return false
})

const displayColor = computed(() => {
  if (isReadOnly.value && store.selectedShape) return store.selectedShape.color
  return store.toolColor
})

const displayThickness = computed(() => {
  if (isReadOnly.value && store.selectedShape) {
    if (store.selectedShapeType === 'rectangle') return (store.selectedShape as Rectangle).borderThickness
    return (store.selectedShape as Arrow | Line).thickness
  }
  return store.toolThickness
})

const displayTextSize = computed(() => {
  if (isReadOnly.value && store.selectedShape) return (store.selectedShape as TextShape).textSize
  return store.toolTextSize
})
</script>

<template>
  <div class="toolbar">
    <div class="tools-grid">
      <button
        v-for="tool in tools"
        :key="tool.name"
        class="tool-btn"
        :class="{ active: store.selectedTool === tool.name, disabled: !tool.enabled }"
        :disabled="!tool.enabled"
        :title="tool.enabled ? tool.label : `${tool.label} (coming soon)`"
        @click="tool.enabled && store.selectTool(tool.name)"
      >
        {{ tool.icon }}
      </button>
    </div>

    <div v-if="showProperties" class="properties-panel">
      <div>
        <div class="property-label">Color</div>
        <div v-if="isReadOnly" class="color-swatches">
          <div
            class="color-swatch"
            :style="{ backgroundColor: displayColor }"
          />
        </div>
        <div v-else class="color-swatches">
          <div
            v-for="c in colors"
            :key="c"
            class="color-swatch"
            :class="{ active: store.toolColor === c }"
            :style="{ backgroundColor: c }"
            @click="store.setToolColor(c)"
          />
        </div>
      </div>

      <div v-if="showThickness">
        <div class="property-label">Thickness: {{ displayThickness }}</div>
        <input
          v-if="!isReadOnly"
          type="range"
          class="property-range"
          min="1"
          max="10"
          step="1"
          :value="store.toolThickness"
          @input="store.setToolThickness(Number(($event.target as HTMLInputElement).value))"
        />
      </div>

      <div v-if="showTextSize">
        <div class="property-label">Text Size: {{ displayTextSize }}</div>
        <input
          v-if="!isReadOnly"
          type="range"
          class="property-range"
          min="12"
          max="72"
          step="2"
          :value="store.toolTextSize"
          @input="store.setToolTextSize(Number(($event.target as HTMLInputElement).value))"
        />
      </div>
    </div>

    <div class="toolbar-footer">
      <button
        class="btn btn-sm btn-outline-danger leave-btn"
        title="Leave whiteboard"
        @click="emit('leave')"
      >
        Leave
      </button>
    </div>
  </div>
</template>

<style scoped>
.toolbar {
  width: 180px;
  background-color: #0d0d1a;
  border-right: 1px solid #2a2a3e;
  height: 100%;
  display: flex;
  flex-direction: column;
  padding: 8px;
}

.tools-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
}

.tool-btn {
  width: 32px;
  height: 32px;
  font-size: 1rem;
  border: 1px solid #2a2a3e;
  border-radius: 6px;
  background: transparent;
  color: #8a8a9e;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.15s, color 0.15s;
}

.tool-btn:hover {
  background-color: #1a1a2e;
  color: #fff;
}

.tool-btn.active {
  background-color: #4f9dff;
  color: #fff;
  border-color: #4f9dff;
}

.tool-btn.disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.properties-panel {
  margin-top: 12px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.property-label {
  font-size: 0.75rem;
  color: #8a8a9e;
  margin-bottom: 4px;
}

.color-swatches {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 6px;
}

.color-swatch {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  border: 2px solid transparent;
  cursor: pointer;
  transition: border-color 0.15s;
}

.color-swatch.active {
  border-color: white;
}

.property-range {
  width: 100%;
  accent-color: #4f9dff;
}

.toolbar-footer {
  margin-top: auto;
  padding-top: 8px;
}

.leave-btn {
  width: 100%;
  height: 36px;
}
</style>
