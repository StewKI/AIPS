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
  <line
    :x1="props.x1"
    :y1="props.y1"
    :x2="props.x2"
    :y2="props.y2"
    :stroke="props.color"
    :stroke-width="props.thickness"
    stroke-dasharray="6 3"
    opacity="0.7"
  />
</template>
