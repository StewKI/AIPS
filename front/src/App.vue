<script setup lang="ts">
import { RouterView } from 'vue-router'
import AppTopBar from './components/AppTopBar.vue'
</script>

<template>
  <AppTopBar />
  <main class="container py-4">
    <RouterView />
  </main>
</template>
