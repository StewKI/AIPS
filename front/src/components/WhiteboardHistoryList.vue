<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useWhiteboardsStore } from '@/stores/whiteboards'
import WhiteboardHistoryItem from './WhiteboardHistoryItem.vue'
import type {Whiteboard} from "@/types";

const router = useRouter()
const store = useWhiteboardsStore()

onMounted(() => {
  if (store.ownedWhiteboards.length === 0) store.getWhiteboardHistory()
})

const sortedWhiteboards = computed(() =>
  [...store.ownedWhiteboards].sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  )
)

const handleClick = (whiteboard: any) => {
  router.push({ name: 'whiteboard', params: { id: whiteboard.id } })
}

const handleDelete = async (whiteboard: Whiteboard) => {
  await store.deleteWhiteboard(whiteboard.id)
}

</script>

<template>
  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-if="sortedWhiteboards.length > 0">
    <WhiteboardHistoryItem
      v-for="wb in sortedWhiteboards"
      :key="wb.id"
      :whiteboard="wb"
      @click="handleClick"
      @delete="handleDelete"
    />
  </div>
  <div class="d-flex flex-column gap-3 overflow-auto h-100 w-100 p-3" v-else>
    <p class="text-muted">You have not created a whiteboard yet</p>
  </div>
</template>
