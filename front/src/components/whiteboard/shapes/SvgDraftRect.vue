<script setup lang="ts">
const props = defineProps<{
  x: number
  y: number
  width: number
  height: number
  color: string
  thickness: number
}>()
</script>

<template>
  <rect
    :x="props.x"
    :y="props.y"
    :width="props.width"
    :height="props.height"
    :stroke="props.color"
    :stroke-width="props.thickness"
    stroke-dasharray="6 3"
    fill="none"
    opacity="0.7"
  />
</template>
