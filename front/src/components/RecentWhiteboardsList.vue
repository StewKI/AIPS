<script setup lang="ts">

import { onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useWhiteboardsStore } from '@/stores/whiteboards'
import RecentWhiteboardsItem from './RecentWhiteboardsItem.vue'

const router = useRouter()
const store = useWhiteboardsStore()

onMounted(() => {
  if (store.recentWhiteboards.length === 0) store.getRecentWhiteboards()
})

const sortedWhiteboards = computed(() =>
  [...store.recentWhiteboards].sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  )
)

const handleClick = (whiteboard: any) => {
  router.push({ name: 'whiteboard', params: { id: whiteboard.id } })
}

</script>

<template>

  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-if="sortedWhiteboards.length > 0">
    <RecentWhiteboardsItem
      v-for="wb in sortedWhiteboards"
      :key="wb.id"
      :whiteboard="wb"
      @click="handleClick"
    />
  </div>
  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-else>
    <p class="text-muted">No recent whiteboards</p>
  </div>

</template>
