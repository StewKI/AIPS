<script setup lang="ts">
import type { Arrow } from '@/types/whiteboard.ts'

const props = withDefaults(defineProps<{ arrow: Arrow; isSelected?: boolean }>(), {
  isSelected: false,
})

const markerId = `arrowhead-${props.arrow.id}`
</script>

<template>
  <g :data-shape-id="props.arrow.id" data-shape-type="arrow">
    <defs>
      <marker
        :id="markerId"
        markerWidth="10"
        markerHeight="7"
        refX="10"
        refY="3.5"
        orient="auto"
      >
        <polygon points="0 0, 10 3.5, 0 7" :fill="props.arrow.color" />
      </marker>
    </defs>
    <line
      :x1="props.arrow.position.x"
      :y1="props.arrow.position.y"
      :x2="props.arrow.endPosition.x"
      :y2="props.arrow.endPosition.y"
      stroke="transparent"
      stroke-width="12"
    />
    <line
      :x1="props.arrow.position.x"
      :y1="props.arrow.position.y"
      :x2="props.arrow.endPosition.x"
      :y2="props.arrow.endPosition.y"
      :stroke="props.arrow.color"
      :stroke-width="props.arrow.thickness"
      :marker-end="`url(#${markerId})`"
    />
    <template v-if="isSelected">
      <circle :cx="props.arrow.position.x" :cy="props.arrow.position.y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="props.arrow.endPosition.x" :cy="props.arrow.endPosition.y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
    </template>
  </g>
</template>
