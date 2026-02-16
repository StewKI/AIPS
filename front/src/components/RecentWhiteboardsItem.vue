<script setup lang="ts">
import type { Whiteboard } from '@/types'

const props = defineProps<{ whiteboard: Whiteboard }>()
const emit = defineEmits<{ click: [whiteboard: Whiteboard] }>()

const formatDate = (date: string | Date) =>
  new Date(date).toLocaleDateString(
    (navigator.languages && navigator.languages[0]) || '',
    { day: '2-digit', month: '2-digit', year: 'numeric' }
  )

const handleClick = () => emit('click', props.whiteboard)
</script>

<template>
  <div
    class="card border rounded-3 p-3 cursor-pointer hover-card"
    @click="handleClick"
  >
    <div class="d-flex justify-content-between align-items-start mb-2">
      <h5 class="mb-0 text-dark">{{ whiteboard.title }}</h5>
      <small class="text-muted">{{ formatDate(whiteboard.createdAt) }}</small>
    </div>
  </div>
</template>

<style scoped>

.cursor-pointer {
  cursor: pointer;
}

.hover-card {
  transition: all 0.2s ease;
}

.hover-card:hover {
  border-color: #007bff !important;
  box-shadow: 0 4px 12px rgba(0, 123, 255, 0.15);
  transform: translateY(-2px);
}

</style>
