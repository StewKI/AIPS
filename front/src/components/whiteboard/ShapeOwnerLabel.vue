<script setup lang="ts">
import { computed, toRef } from 'vue'
import { useShapeOwner } from '@/composables/useShapeOwner'

const props = defineProps<{
  ownerId: string
  anchorX: number
  anchorY: number
}>()

const ownerIdRef = toRef(props, 'ownerId')
const { displayName, avatarColor } = useShapeOwner(ownerIdRef)

const firstLetter = computed(() => displayName.value.charAt(0).toUpperCase())
const labelY = computed(() => props.anchorY - 24)
</script>

<template>
  <g :transform="`translate(${anchorX}, ${labelY})`">
    <circle r="8" :fill="avatarColor" cx="0" cy="0" />
    <text
      x="0"
      y="0"
      text-anchor="middle"
      dominant-baseline="central"
      fill="#fff"
      font-size="9"
      font-weight="700"
    >
      {{ firstLetter }}
    </text>
    <text
      x="14"
      y="0"
      dominant-baseline="central"
      fill="rgba(255,255,255,0.85)"
      font-size="12"
    >
      {{ displayName }}
    </text>
  </g>
</template>
