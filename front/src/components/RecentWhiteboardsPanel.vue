<script setup lang="ts">

import RecentWhiteboardsList from './RecentWhiteboardsList.vue'

</script>

<template>

  <div
    id="recentWhiteboardsSidebar"
    class="offcanvas offcanvas-start bg-dark text-light"
    tabindex="-1"
    aria-labelledby="recentWhiteboardsSidebarLabel"
    data-testid="recent-whiteboards-sidebar"
  >
    <div class="offcanvas-header border-bottom border-secondary">
      <h5 id="recentWhiteboardsSidebarLabel" class="offcanvas-title" data-testid="recent-whiteboards-sidebar-title">
        Recent Whiteboards
      </h5>
      <button
        type="button"
        class="btn-close"
        data-bs-dismiss="offcanvas"
        aria-label="Close"
        data-testid="recent-whiteboards-sidebar-close-button"
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
