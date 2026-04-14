<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const username = ref('')
const email = ref('')
const password = ref('')
const error = ref<string[]>([])
const loading = ref(false)

async function onSubmit() {
  error.value = []
  loading.value = true
  try {
    await auth.signup({ username: username.value, email: email.value, password: password.value })
    await auth.login({ email: email.value, password: password.value})
    router.push('/')
  } catch (e: any) {
    error.value = e.messages || ['Signup failed']
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="auth-card card">
    <div class="card-body p-4">
      <h2 class="card-title text-center mb-4">Sign Up</h2>

      <div v-if="error.length" class="alert alert-danger" data-testid="signup-error">
        <ul>
          <li v-for="m in error" :key="m">{{ m }}</li>
        </ul>
      </div>

      <form @submit.prevent="onSubmit" data-testid="signup-form">
        <div class="mb-3">
          <label for="username" class="form-label">Username</label>
          <input
            id="username"
            v-model="username"
            type="text"
            class="form-control"
            required
            autocomplete="username"
            data-testid="signup-username-input"
          />
        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input
            id="email"
            v-model="email"
            type="email"
            class="form-control"
            required
            autocomplete="email"
            data-testid="signup-email-input"
          />
        </div>

        <div class="mb-3">
          <label for="password" class="form-label">Password</label>
          <input
            id="password"
            v-model="password"
            type="password"
            class="form-control"
            required
            autocomplete="new-password"
            data-testid="signup-password-input"
          />
        </div>

        <button type="submit" class="btn btn-primary w-100" :disabled="loading" data-testid="signup-submit-button">
          {{ loading ? 'Signing up...' : 'Sign Up' }}
        </button>
      </form>

      <p class="text-center mt-3 mb-0">
        Already have an account?
        <RouterLink to="/login" data-testid="signup-login-link">Log in</RouterLink>
      </p>
    </div>
  </div>
</template>
