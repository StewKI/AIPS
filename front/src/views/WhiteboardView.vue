<script setup lang="ts">
import { onMounted, onUnmounted, onBeforeMount, onBeforeUnmount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import { useWhiteboardsStore } from "@/stores/whiteboards.ts";
import WhiteboardToolbar from '@/components/whiteboard/WhiteboardToolbar.vue'
import WhiteboardCanvas from '@/components/whiteboard/WhiteboardCanvas.vue'

const route = useRoute()
const router = useRouter()
const sessionStore = useWhiteboardStore()
const infoStore = useWhiteboardsStore()

const whiteboardId = route.params.id as string

onBeforeMount(() => {
  infoStore.selectWhiteboard(whiteboardId)
})

onMounted(() => {
  sessionStore.joinWhiteboard(whiteboardId)
})

onBeforeUnmount(() => {
  infoStore.deselectWhiteboard()
})

onUnmounted(() => {
  sessionStore.leaveWhiteboard()
})

async function handleLeave() {
  await sessionStore.leaveWhiteboard()
  router.back()
}
</script>

<template>
  <div v-if="sessionStore.isLoading" class="d-flex flex-column justify-content-center align-items-center vh-100">
    <div class="spinner-border text-primary mb-3" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
    <p class="text-muted fs-5 text-center">
      Please wait while your whiteboard is loading...
    </p>
  </div>

  <div v-else-if="sessionStore.error" class="d-flex justify-content-center align-items-center vh-100">
    <div class="alert alert-danger" role="alert">
      {{ sessionStore.error }}
    </div>
  </div>

  <div v-else class="d-flex vh-100">
    <WhiteboardToolbar @leave="handleLeave" />
    <WhiteboardCanvas />
  </div>
</template>
