<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useWhiteboardStore } from '@/stores/whiteboards'
import WhiteboardHistoryItem from './WhiteboardHistoryItem.vue'

const store = useWhiteboardStore()

onMounted(() => {
  if (store.ownedWhiteboards.length === 0) store.getWhiteboardHistory()
})

const sortedWhiteboards = computed(() =>
  [...store.ownedWhiteboards].sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  )
)

const handleClick = (whiteboard: any) => {
  console.log('Clicked:', whiteboard)
}
</script>

<template>
  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-if="sortedWhiteboards.length > 0">
    <WhiteboardHistoryItem
      v-for="wb in sortedWhiteboards"
      :key="wb.id"
      :whiteboard="wb"
      @click="handleClick"
    />
  </div>
  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-else>
    <p class="text-muted">You have not created a whiteboard yet</p>
  </div>
</template>
