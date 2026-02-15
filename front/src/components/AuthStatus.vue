<template>
  <div class="auth-status">
    <template v-if="isLoading">
      <span class="loading">Loading...</span>
    </template>
    <template v-else-if="isAuthenticated">
      <span class="username">{{ user?.username ?? user?.email }}</span>
      <button @click="logout" class="logout">Logout</button>
    </template>
    <template v-else>
      <slot>
        <!-- Default: show nothing or provide links in parent -->
      </slot>
    </template>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

const store = useAuthStore()
const user = computed(() => store.user)
const isAuthenticated = computed(() => store.isAuthenticated)
const isLoading = computed(() => store.isLoading)

function logout() {
  store.logout()
}

onMounted(() => {
  // ensure store is hydrated; safe to call multiple times
  void store.initialize()
})
</script>

<style scoped>
.auth-status {
  display: flex;
  align-items: center;
  gap: 8px;
}
.username {
  font-weight: 600;
}
.logout {
  padding: 4px 8px;
  border: 1px solid #ccc;
  background: white;
  cursor: pointer;
}
.loading {
  color: #666;
}
</style>
