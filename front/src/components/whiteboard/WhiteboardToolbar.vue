<script setup lang="ts">
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import type { ShapeTool } from '@/types/whiteboard.ts'

const store = useWhiteboardStore()
const emit = defineEmits<{ leave: [] }>()

const tools: { name: ShapeTool; label: string; icon: string; enabled: boolean }[] = [
  { name: 'rectangle', label: 'Rectangle', icon: '▭', enabled: true },
  { name: 'arrow', label: 'Arrow', icon: '→', enabled: false },
  { name: 'line', label: 'Line', icon: '╱', enabled: false },
  { name: 'text', label: 'Text', icon: 'T', enabled: false },
]
</script>

<template>
  <div class="toolbar d-flex flex-column align-items-center py-2 gap-2">
    <button
      v-for="tool in tools"
      :key="tool.name"
      class="btn btn-sm"
      :class="[
        store.selectedTool === tool.name ? 'btn-primary' : 'btn-outline-secondary',
        { disabled: !tool.enabled },
      ]"
      :disabled="!tool.enabled"
      :title="tool.enabled ? tool.label : `${tool.label} (coming soon)`"
      style="width: 40px; height: 40px; font-size: 1.1rem"
      @click="tool.enabled && store.selectTool(tool.name)"
    >
      {{ tool.icon }}
    </button>

    <div class="mt-auto mb-2">
      <button
        class="btn btn-sm btn-outline-danger"
        title="Leave whiteboard"
        style="width: 40px; height: 40px"
        @click="emit('leave')"
      >
        ✕
      </button>
    </div>
  </div>
</template>

<style scoped>
.toolbar {
  width: 56px;
  background-color: #0d0d1a;
  border-right: 1px solid #2a2a3e;
  height: 100%;
}
</style>
