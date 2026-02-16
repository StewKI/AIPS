<script setup lang="ts">

import RecentWhiteboardsList from './RecentWhiteboardsList.vue'

</script>

<template>

  <div
    id="recentWhiteboardsSidebar"
    class="offcanvas offcanvas-start bg-dark text-light"
    tabindex="-1"
    aria-labelledby="recentWhiteboardsSidebarLabel"
  >
    <div class="offcanvas-header border-bottom border-secondary">
      <h5 id="recentWhiteboardsSidebarLabel" class="offcanvas-title">
        Recent Whiteboards
      </h5>
      <button
        type="button"
        class="btn-close btn-close-white"
        data-bs-dismiss="offcanvas"
        aria-label="Close"
      />
    </div>
    <div class="offcanvas-body p-0">
      <RecentWhiteboardsList />
    </div>
  </div>

</template>

<style scoped>

#recentWhiteboardsSidebar.offcanvas-start {
  top: 56px;
  height: calc(100vh - 56px);
}

</style>
