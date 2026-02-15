<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
</script>

<template>
  <nav class="navbar navbar-expand-md navbar-dark bg-dark border-bottom border-secondary">
    <div class="container">
      <RouterLink class="navbar-brand" to="/">AIPS</RouterLink>

      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarNav"
        aria-controls="navbarNav"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div id="navbarNav" class="collapse navbar-collapse">
        <ul class="navbar-nav me-auto">
          <li class="nav-item">
            <RouterLink class="nav-link" active-class="active" to="/test">Test</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" active-class="active" to="/about">About</RouterLink>
          </li>
        </ul>

        <ul class="navbar-nav">
          <template v-if="auth.isAuthenticated">
            <li class="nav-item">
              <span class="nav-link text-light">{{ auth.user?.username }}</span>
            </li>
            <li class="nav-item">
              <button class="btn btn-outline-light btn-sm my-1" @click="auth.logout()">
                Logout
              </button>
            </li>
          </template>
          <template v-else>
            <li class="nav-item">
              <RouterLink class="nav-link" active-class="active" to="/login">Login</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink class="nav-link" active-class="active" to="/signup">Sign Up</RouterLink>
            </li>
          </template>
        </ul>
      </div>
    </div>
  </nav>
</template>
