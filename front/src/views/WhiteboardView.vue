<script setup lang="ts">
import { onMounted, onUnmounted, onBeforeMount, onBeforeUnmount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.ts'
import { useWhiteboardStore } from '@/stores/whiteboard.ts'
import { useWhiteboardsStore } from "@/stores/whiteboards.ts";
import WhiteboardToolbar from '@/components/whiteboard/WhiteboardToolbar.vue'
import WhiteboardCanvas from '@/components/whiteboard/WhiteboardCanvas.vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
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
  if (infoStore.isWaitingToJoin) {
    await sessionStore.cancelJoinRequest()
  } else {
    await sessionStore.leaveWhiteboard()
  }
  router.back()
}
</script>

<template>
  <div
    v-if="infoStore.isWaitingToJoin"
    class="d-flex flex-column justify-content-center align-items-center vh-100 text-center"
    data-testid="whiteboard-view-waiting-room"
  >
    <div class="spinner-border text-primary mb-4" role="status">
      <span class="visually-hidden">Waiting...</span>
    </div>

    <h5 class="mb-3">Waiting for owner's approval</h5>

    <button
      v-if="sessionStore.isConnected"
      class="btn btn-outline-danger"
      @click="handleLeave"
      data-testid="whiteboard-view-waiting-room-cancel-button"
    >
      Cancel
    </button>
  </div>

  <div
    v-else-if="sessionStore.isLoading"
    class="d-flex flex-column justify-content-center align-items-center vh-100"
  >
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

  <div
    v-else
    class="d-flex vh-100"
    data-testid="whiteboard-view"
  >
    <WhiteboardToolbar @leave="handleLeave" />
    <WhiteboardCanvas />

    <div v-if="sessionStore.whiteboard?.ownerId === authStore.user?.userId && sessionStore.pendingUsers.length > 0"
         class="position-fixed top-0 end-0 m-4 p-3 bg-dark border border-primary rounded shadow-lg"
         style="z-index: 1050; width: 300px;">
      <h6 class="text-primary mb-3">Pending Join Requests ({{ sessionStore.pendingUsers.length }})</h6>
      <div class="list-group list-group-flush bg-transparent">
        <div v-for="user in sessionStore.pendingUsers" :key="user.userId"
             class="list-group-item bg-transparent text-light border-secondary d-flex justify-content-between align-items-center px-0">
          <small class="text-truncate" :title="user.username">{{ user.username }}</small>
          <div class="btn-group btn-group-sm">
            <button class="btn btn-success" @click="sessionStore.approveUser(user.userId)">✓</button>
            <button class="btn btn-danger" @click="sessionStore.rejectUser(user.userId)">✕</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
