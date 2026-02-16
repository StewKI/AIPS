<script setup lang="ts">

import { onMounted } from 'vue'
import { RouterView } from 'vue-router'
import AppTopBar from './components/AppTopBar.vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()

onMounted(() => {
  auth.initialize()
})

</script>


<template>
  <AppTopBar />
  <main class="container py-4">
    <RouterView />
  </main>
</template>
