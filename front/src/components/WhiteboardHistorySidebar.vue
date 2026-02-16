<script setup lang="ts">

import WhiteboardHistoryList from './WhiteboardHistoryList.vue'

</script>

<template>

  <div
    id="whiteboardSidebar"
    class="offcanvas offcanvas-start bg-dark text-light"
    tabindex="-1"
    aria-labelledby="whiteboardSidebarLabel"
  >
    <div class="offcanvas-header border-bottom border-secondary">
      <h5 id="whiteboardSidebarLabel" class="offcanvas-title">
        My Whiteboards
      </h5>
      <button
        type="button"
        class="btn-close btn-close-white"
        data-bs-dismiss="offcanvas"
        aria-label="Close"
      />
    </div>
    <div class="offcanvas-body p-0">
      <WhiteboardHistoryList />
    </div>
  </div>

</template>

<style scoped>

#whiteboardSidebar.offcanvas-start {
  top: 56px;
  height: calc(100vh - 56px);
}

@media (max-width: 768px) {
  #whiteboardSidebar.offcanvas-start {
    top: 56px;
    height: calc(100vh - 56px);
  }
}

</style>
