<script setup lang="ts">
import { computed, toRef } from 'vue'
import { useShapeOwner } from '@/composables/useShapeOwner'

const props = defineProps<{
  ownerId: string
  cursorX: number
  cursorY: number
}>()

const ownerIdRef = toRef(props, 'ownerId')
const { displayName, avatarColor } = useShapeOwner(ownerIdRef)

const firstLetter = computed(() => displayName.value.charAt(0).toUpperCase())
</script>

<template>
  <div
    class="shape-owner-tooltip"
    :style="{ left: cursorX + 12 + 'px', top: cursorY - 8 + 'px' }"
  >
    <span class="avatar" :style="{ backgroundColor: avatarColor }">
      {{ firstLetter }}
    </span>
    <span class="name">{{ displayName }}</span>
  </div>
</template>

<style scoped>
.shape-owner-tooltip {
  position: fixed;
  pointer-events: none;
  z-index: 1000;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 6px 10px;
  background: rgba(13, 13, 26, 0.95);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 8px;
  white-space: nowrap;
}

.avatar {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 700;
  color: #fff;
  flex-shrink: 0;
}

.name {
  font-size: 13px;
  color: rgba(255, 255, 255, 0.85);
}
</style>
