<script setup lang="ts">
import { RouterView, useRoute } from 'vue-router'
import { computed } from 'vue'
import AppTopBar from './components/AppTopBar.vue'

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
