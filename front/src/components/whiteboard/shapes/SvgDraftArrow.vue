<script setup lang="ts">
const props = defineProps<{
  x1: number
  y1: number
  x2: number
  y2: number
  color: string
  thickness: number
}>()
</script>

<template>
  <defs>
    <marker
      id="draft-arrowhead"
      markerWidth="10"
      markerHeight="7"
      refX="10"
      refY="3.5"
      orient="auto"
    >
      <polygon points="0 0, 10 3.5, 0 7" :fill="props.color" opacity="0.7" />
    </marker>
  </defs>
  <line
    :x1="props.x1"
    :y1="props.y1"
    :x2="props.x2"
    :y2="props.y2"
    :stroke="props.color"
    :stroke-width="props.thickness"
    stroke-dasharray="6 3"
    opacity="0.7"
    marker-end="url(#draft-arrowhead)"
  />
</template>
