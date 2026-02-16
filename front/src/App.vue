<script setup lang="ts">

import { onMounted } from 'vue'
import { RouterView, useRoute } from 'vue-router'
import { computed } from 'vue'
import AppTopBar from './components/AppTopBar.vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()

onMounted(() => {
  auth.initialize()
})


const route = useRoute()
const hideTopBar = computed(() => route.meta.hideTopBar === true)
</script>


<template>
  <template v-if="hideTopBar">
    <RouterView />
  </template>
  <template v-else>
    <AppTopBar />
    <main class="container py-4">
      <RouterView />
    </main>
  </template>
</template>
