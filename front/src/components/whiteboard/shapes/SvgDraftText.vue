<script setup lang="ts">
const props = defineProps<{
  x: number
  y: number
  textValue: string
  textSize: number
  color: string
}>()
</script>

<template>
  <text
    :x="props.x"
    :y="props.y"
    :font-size="props.textSize"
    :fill="props.color"
    opacity="0.7"
    font-family="Arial, sans-serif"
    dominant-baseline="hanging"
  >
    {{ props.textValue }}
  </text>
</template>
