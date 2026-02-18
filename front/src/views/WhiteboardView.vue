<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import WhiteboardToolbar from '@/components/whiteboard/WhiteboardToolbar.vue'
import WhiteboardCanvas from '@/components/whiteboard/WhiteboardCanvas.vue'

const route = useRoute()
const router = useRouter()
const store = useWhiteboardStore()

const whiteboardId = route.params.id as string

onMounted(() => {
  store.joinWhiteboard(whiteboardId)
})

onUnmounted(() => {
  store.leaveWhiteboard()
})

async function handleLeave() {
  await store.leaveWhiteboard()
  router.back()
}
</script>

<template>
  <div v-if="store.isLoading" class="d-flex flex-column justify-content-center align-items-center vh-100">
    <div class="spinner-border text-primary mb-3" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
    <p class="text-muted fs-5 text-center">
      Please wait while your whiteboard is loading...
    </p>
  </div>

  <div v-else-if="store.error" class="d-flex justify-content-center align-items-center vh-100">
    <div class="alert alert-danger" role="alert">
      {{ store.error }}
    </div>
  </div>

  <div v-else class="d-flex vh-100">
    <WhiteboardToolbar @leave="handleLeave" />
    <WhiteboardCanvas />
  </div>
</template>
