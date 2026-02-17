<script setup lang="ts">
import { computed } from 'vue'
import type { Rectangle } from '@/types/whiteboard.ts'

const props = withDefaults(defineProps<{ rectangle: Rectangle; isSelected?: boolean }>(), {
  isSelected: false,
})

const x = computed(() => Math.min(props.rectangle.position.x, props.rectangle.endPosition.x))
const y = computed(() => Math.min(props.rectangle.position.y, props.rectangle.endPosition.y))
const w = computed(() => Math.abs(props.rectangle.endPosition.x - props.rectangle.position.x))
const h = computed(() => Math.abs(props.rectangle.endPosition.y - props.rectangle.position.y))
</script>

<template>
  <g :data-shape-id="props.rectangle.id" data-shape-type="rectangle">
    <rect
      :x="x"
      :y="y"
      :width="w"
      :height="h"
      :stroke="props.rectangle.color"
      :stroke-width="props.rectangle.borderThickness"
      fill="none"
    />
    <template v-if="isSelected">
      <rect
        :x="x"
        :y="y"
        :width="w"
        :height="h"
        stroke="#4f9dff"
        stroke-width="1"
        stroke-dasharray="4 2"
        fill="none"
      />
      <circle :cx="x" :cy="y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="x + w" :cy="y" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="x" :cy="y + h" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="x + w" :cy="y + h" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
    </template>
  </g>
</template>
