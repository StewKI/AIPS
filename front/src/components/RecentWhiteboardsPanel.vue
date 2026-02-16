<script setup lang="ts">

import RecentWhiteboardsList from './RecentWhiteboardsList.vue'

</script>

<template>

  <div class="whiteboards-panel card border-secondary shadow-sm h-100">
    <div class="card-header bg-light text-dark">
      Recent Whiteboards
    </div>
    <div class="card-body p-0 overflow-auto">
      <RecentWhiteboardsList />
    </div>
  </div>

</template>

<style scoped>

.whiteboards-panel {
  max-height: 500px;
  width: 100%;
}

</style>
