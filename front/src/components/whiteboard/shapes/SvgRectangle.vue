<script setup lang="ts">
import type { Rectangle } from '@/types/whiteboard.ts'

const props = defineProps<{ rectangle: Rectangle }>()
</script>

<template>
  <rect
    :x="Math.min(props.rectangle.position.x, props.rectangle.endPosition.x)"
    :y="Math.min(props.rectangle.position.y, props.rectangle.endPosition.y)"
    :width="Math.abs(props.rectangle.endPosition.x - props.rectangle.position.x)"
    :height="Math.abs(props.rectangle.endPosition.y - props.rectangle.position.y)"
    :stroke="props.rectangle.color"
    :stroke-width="props.rectangle.borderThickness"
    fill="none"
  />
</template>
