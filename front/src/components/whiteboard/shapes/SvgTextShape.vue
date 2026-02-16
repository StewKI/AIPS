<script setup lang="ts">
import { ref, watch, onMounted, nextTick } from 'vue'
import type { TextShape } from '@/types/whiteboard.ts'

const props = withDefaults(defineProps<{ textShape: TextShape; isSelected?: boolean }>(), {
  isSelected: false,
})

const textEl = ref<SVGTextElement>()
const bbox = ref<{ x: number; y: number; width: number; height: number } | null>(null)

function updateBBox() {
  if (textEl.value) {
    const b = textEl.value.getBBox()
    bbox.value = { x: b.x, y: b.y, width: b.width, height: b.height }
  }
}

onMounted(updateBBox)
watch(() => props.isSelected, (val) => {
  if (val) nextTick(updateBBox)
}, { flush: 'post' })
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
    <template v-if="isSelected && bbox">
      <rect
        :x="bbox.x - 4"
        :y="bbox.y - 4"
        :width="bbox.width + 8"
        :height="bbox.height + 8"
        stroke="#4f9dff"
        stroke-width="1"
        stroke-dasharray="4 2"
        fill="none"
      />
      <circle :cx="bbox.x - 4" :cy="bbox.y - 4" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="bbox.x + bbox.width + 4" :cy="bbox.y - 4" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="bbox.x - 4" :cy="bbox.y + bbox.height + 4" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
      <circle :cx="bbox.x + bbox.width + 4" :cy="bbox.y + bbox.height + 4" r="4" fill="white" stroke="#4f9dff" stroke-width="1.5" />
    </template>
  </g>
</template>
