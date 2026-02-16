<script setup lang="ts">
const props = defineProps<{
  x: number
  y: number
  width: number
  height: number
}>()
</script>

<template>
  <rect
    :x="props.x"
    :y="props.y"
    :width="props.width"
    :height="props.height"
    stroke="#4f9dff"
    :stroke-width="2"
    stroke-dasharray="6 3"
    fill="none"
    opacity="0.7"
  />
</template>
