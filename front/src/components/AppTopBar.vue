<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const showLogoutConfirm = ref(false)
</script>

<template>
  <nav class="navbar navbar-expand-md navbar-dark bg-dark border-bottom border-secondary sticky-top">
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
          <!-- <li class="nav-item">
            <RouterLink class="nav-link" active-class="active" to="/test">Test</RouterLink>
          </li> -->
        </ul>

        <ul class="navbar-nav">
          <template v-if="auth.isAuthenticated">
            <li class="nav-item">
              <span
                class="nav-link text-light"
                data-testid="topbar-username"
              >
                {{ auth.user?.username }}
              </span>
            </li>
            <li class="nav-item">
              <button
                class="btn btn-outline-light btn-sm my-1"
                @click="showLogoutConfirm = true"
                data-testid="topbar-logout-button"
              >
                Logout
              </button>
            </li>
          </template>
          <template v-else>
            <li class="nav-item">
              <RouterLink
                class="nav-link"
                active-class="active"
                to="/login"
                data-testid="topbar-login-link"
              >
                Login
              </RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink
                class="nav-link"
                active-class="active"
                to="/signup"
                data-testid="topbar-signup-link"
              >
                Sign Up
              </RouterLink>
            </li>
          </template>
        </ul>
      </div>
    </div>
  </nav>

  <Teleport to="body">
    <div
      v-if="showLogoutConfirm"
      class="modal d-block"
      tabindex="-1"
      style="background: rgba(0,0,0,0.5)"
      @click.self="showLogoutConfirm = false"
      data-testid="logout-confirm-modal"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header border-0 pb-0">
            <h5 class="modal-title">Logout</h5>
            <button
              type="button"
              class="btn-close"
              @click="showLogoutConfirm = false"
              data-testid="logout-modal-close-button"
            ></button>
          </div>
          <div class="modal-body">
            Are you sure you want to logout?
          </div>
          <div class="modal-footer border-0 pt-0">
            <button
              type="button"
              class="btn btn-outline-secondary"
              @click="showLogoutConfirm = false"
              data-testid="logout-modal-cancel-button"
            >
              Cancel
            </button>
            <button
              type="button"
              class="btn btn-danger"
              @click="showLogoutConfirm = false; auth.logout()"
              data-testid="logout-modal-confirm-button"
            >
              Yes, logout
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>
