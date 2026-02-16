<script setup lang="ts">

import WhiteboardHistorySidebar from '@/components/WhiteboardHistorySidebar.vue'
import RecentWhiteboardsPanel from '@/components/RecentWhiteboardsPanel.vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()

</script>

<template>
  <WhiteboardHistorySidebar v-if="auth.isAuthenticated" />

  <div class="container py-4">
    <div
      v-if="auth.isAuthenticated"
      class="d-flex justify-content-center position-absolute bottom-0 start-50 translate-middle-x mb-3 w-50"
    >
      <RecentWhiteboardsPanel />
    </div>
  </div>
</template>

<style scoped>

</style>
