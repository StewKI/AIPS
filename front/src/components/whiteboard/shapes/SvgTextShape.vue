<script setup lang="ts">
import { ref, computed, watch, onMounted, nextTick } from 'vue'
import type { TextShape } from '@/types/whiteboard.ts'

const props = withDefaults(defineProps<{ textShape: TextShape; isSelected?: boolean }>(), {
  isSelected: false,
})

const textEl = ref<SVGTextElement>()
const textMetrics = ref<{ width: number; height: number; offsetX: number; offsetY: number } | null>(null)

function measureText() {
  if (textEl.value) {
    const b = textEl.value.getBBox()
    textMetrics.value = {
      width: b.width,
      height: b.height,
      offsetX: b.x - props.textShape.position.x,
      offsetY: b.y - props.textShape.position.y,
    }
  }
}

onMounted(measureText)
watch(
  () => [props.textShape.textValue, props.textShape.textSize],
  () => { nextTick(measureText) },
  { flush: 'post' },
)

const outlineX = computed(() => props.textShape.position.x + (textMetrics.value?.offsetX ?? 0) - 4)
const outlineY = computed(() => props.textShape.position.y + (textMetrics.value?.offsetY ?? 0) - 4)
const outlineW = computed(() => (textMetrics.value?.width ?? 0) + 8)
const outlineH = computed(() => (textMetrics.value?.height ?? 0) + 8)
</script>

<template>
  <g :data-shape-id="props.textShape.id" data-shape-type="textShape">
    <text
      ref="textEl"
      :x="props.textShape.position.x"
      :y="props.textShape.position.y"
      :fill="props.textShape.color"
      :font-size="props.textShape.textSize"
      font-family="Arial, sans-serif"
      dominant-baseline="hanging"
    >
      {{ props.textShape.textValue }}
    </text>
    <template v-if="isSelected && textMetrics">
      <rect
        :x="outlineX"
        :y="outlineY"
        :width="outlineW"
        :height="outlineH"
        stroke="#4f9dff"
        stroke-width="1"
        stroke-dasharray="4 2"
        fill="none"
      />
      <circle :cx="outlineX" :cy="outlineY" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="outlineX + outlineW" :cy="outlineY" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="outlineX" :cy="outlineY + outlineH" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="outlineX + outlineW" :cy="outlineY + outlineH" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
    </template>
  </g>
</template>
