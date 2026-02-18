<script setup lang="ts">

import type { Whiteboard } from '@/types'
import { ref } from 'vue'

const props = defineProps<{ whiteboard: Whiteboard }>()
const emit = defineEmits<{
  (e: 'click', whiteboard: Whiteboard): void
  (e: 'delete', whiteboard: Whiteboard): void
}>()

const showConfirm = ref(false)

const formatDate = (date: string | Date) =>
  new Date(date).toLocaleDateString(
    (navigator.languages && navigator.languages[0]) || '',
    { day: '2-digit', month: '2-digit', year: 'numeric' }
  )

const handleClick = () => emit('click', props.whiteboard)

const handleDeleteClick = (e: MouseEvent) => {
  e.stopPropagation()
  showConfirm.value = true
}

const handleCancel = (e: MouseEvent) => {
  e.stopPropagation()
  showConfirm.value = false
}

const handleConfirmDelete = (e: MouseEvent) => {
  e.stopPropagation()
  showConfirm.value = false
  emit('delete', props.whiteboard)
}

</script>

<template>

  <div
    class="card border rounded-3 p-3 cursor-pointer hover-card"
    @click="handleClick"
  >
    <div class="d-flex justify-content-between align-items-start mb-2">
      <h5 class="mb-0 text-dark">{{ whiteboard.title }}</h5>
      <div class="d-flex align-items-center gap-2">
        <small class="text-muted">{{ formatDate(whiteboard.createdAt) }}</small>
        <button
          class="btn btn-link p-0 text-danger"
          title="Delete whiteboard"
          @click="handleDeleteClick"
        >
          <i class="bi bi-trash fs-5"></i>
        </button>
      </div>
    </div>
  </div>

  <!-- Confirmation Modal -->
  <Teleport to="body">
    <div
      v-if="showConfirm"
      class="modal d-block"
      tabindex="-1"
      style="background: rgba(0,0,0,0.5)"
      @click.self="handleCancel"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header border-0 pb-0">
            <h5 class="modal-title">Delete Whiteboard</h5>
            <button
              type="button"
              class="btn-close"
              @click="handleCancel"
            ></button>
          </div>
          <div class="modal-body">
            Are you sure you want to delete this whiteboard?
          </div>
          <div class="modal-footer border-0 pt-0">
            <button
              type="button"
              class="btn btn-outline-secondary"
              @click="handleCancel"
            >
              Cancel
            </button>
            <button
              type="button"
              class="btn btn-danger"
              @click="handleConfirmDelete"
            >
              Yes, delete
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>

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
