<script setup lang="ts">
import type { Line } from '@/types/whiteboard.ts'

const props = withDefaults(defineProps<{ line: Line; isSelected?: boolean }>(), {
  isSelected: false,
})
</script>

<template>
  <g :data-shape-id="props.line.id" data-shape-type="line">
    <line
      :x1="props.line.position.x"
      :y1="props.line.position.y"
      :x2="props.line.endPosition.x"
      :y2="props.line.endPosition.y"
      stroke="transparent"
      stroke-width="12"
    />
    <line
      :x1="props.line.position.x"
      :y1="props.line.position.y"
      :x2="props.line.endPosition.x"
      :y2="props.line.endPosition.y"
      :stroke="props.line.color"
      :stroke-width="props.line.thickness"
      fill="none"
    />
    <template v-if="isSelected">
      <circle :cx="props.line.position.x" :cy="props.line.position.y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="props.line.endPosition.x" :cy="props.line.endPosition.y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
    </template>
  </g>
</template>
