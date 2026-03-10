<script setup lang="ts">

import {ref} from 'vue'
import {useRouter} from 'vue-router'
import WhiteboardHistorySidebar from '@/components/WhiteboardHistorySidebar.vue'
import RecentWhiteboardsPanel from '@/components/RecentWhiteboardsPanel.vue'
import {useAuthStore} from '@/stores/auth'
import {useWhiteboardsStore} from "@/stores/whiteboards.ts";
import {MembershipStatus, WhiteboardJoinPolicy} from "@/enums";

const auth = useAuthStore()
const whiteboards = useWhiteboardsStore()
const router = useRouter()

const joinCode = ref('')
const whiteboardTitle = ref('')
const selectedJoinPolicy = ref(WhiteboardJoinPolicy.FreeToJoin)
const maxParticipants = ref(10)
const showCreateModal = ref(false)

const newWhiteboardError = ref<string[]>([])
const joinWithCodeError = ref<string[]>([])

async function handleCreateNewWhiteboard() {
  newWhiteboardError.value = []

  if (!whiteboardTitle.value.trim()) {
    alert('Please enter a title for the whiteboard.')
    return
  }

  try {
    const newWhiteboardId = await whiteboards.createNewWhiteboard(
      whiteboardTitle.value.trim(),
      selectedJoinPolicy.value,
      maxParticipants.value
    )

    showCreateModal.value = false
    whiteboardTitle.value = ''
    selectedJoinPolicy.value = WhiteboardJoinPolicy.FreeToJoin
    maxParticipants.value = 10

    await router.push({ name: 'whiteboard', params: { id: newWhiteboardId } })
  } catch (e: any) {
    newWhiteboardError.value = e.messages || ['Failed to create whiteboard']
  } finally {
    whiteboards.isLoading = false
  }
}

async function joinWithCode() {
  joinWithCodeError.value = []

  if (joinCode.value.length !== 8) {
    alert('Please enter a valid 8-digit code.')
    return
  }

  try {
    const joinResult = await whiteboards.joinWhiteboardWithCode(joinCode.value)

    if (joinResult.status === MembershipStatus.Pending) {
      whiteboards.startWaitingToJoin()
    } else {
      whiteboards.stopWaitingToJoin()
    }

    await router.push({ name: 'whiteboard', params: { id: joinResult.whiteboardId } })
  } catch (err: any) {
    joinWithCodeError.value = err.messages || ['Failed to join whiteboard with code']
  }
}

</script>

<template>
  <div
    v-if="auth.isAuthenticated"
    class="position-fixed start-0 top-50 translate-middle-y d-flex flex-column"
    style="z-index: 1040;"
  >
    <button
      class="btn btn-dark rounded-0 rounded-end py-3 px-2"
      type="button"
      data-bs-toggle="offcanvas"
      data-bs-target="#whiteboardSidebar"
      aria-controls="whiteboardSidebar"
      style="writing-mode: vertical-rl;"
    >
      My Whiteboards
    </button>
    <button
      class="btn btn-dark rounded-0 rounded-end py-3 px-2"
      type="button"
      data-bs-toggle="offcanvas"
      data-bs-target="#recentWhiteboardsSidebar"
      aria-controls="recentWhiteboardsSidebar"
      style="writing-mode: vertical-rl;"
    >
      Recent Whiteboards
    </button>
  </div>

  <div
    v-if="auth.isAuthenticated"
    class="d-flex align-items-center justify-content-center"
    style="min-height: calc(100vh - 56px);"
  >
    <div style="width: 320px;">

      <div v-if="joinWithCodeError.length" class="alert alert-danger">
        <ul>
          <li v-for="errorMessage in joinWithCodeError" :key="errorMessage">{{ errorMessage }}</li>
        </ul>
      </div>

      <input
        v-model="joinCode"
        type="text"
        class="form-control text-center bg-dark text-light border-secondary"
        style="font-size: 1.5rem; padding: 0.75rem;"
        placeholder="Enter 8-digit code"
        maxlength="8"
        inputmode="numeric"
        pattern="[0-9]*"
        @input="joinCode = joinCode.replace(/\D/g, '')"
      />
      <button class="btn btn-primary w-75 mt-2 d-block mx-auto" :disabled="whiteboards.isLoading" @click="joinWithCode">
        <span v-if="whiteboards.isLoading" class="spinner-border spinner-border-sm me-2"></span>
        Join with code
      </button>
      <div class="text-center">
        <small class="text-muted my-4 d-inline-block">or</small>
      </div>
      <button class="btn btn-outline-light w-75 d-block mx-auto" @click="showCreateModal = true">Create new whiteboard</button>
    </div>
  </div>

  <div v-if="showCreateModal" class="modal d-block" tabindex="-1" @click.self="showCreateModal = false">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content bg-dark text-light">
        <div class="modal-header border-secondary">
          <h5 class="modal-title">Create new whiteboard</h5>
          <button type="button" class="btn-close btn-close-white" @click="showCreateModal = false"></button>
        </div>
        <div class="modal-body">

          <div v-if="newWhiteboardError.length" class="alert alert-danger">
            <ul>
              <li v-for="errorMessage in newWhiteboardError" :key="errorMessage">{{ errorMessage }}</li>
            </ul>
          </div>

          <input
            v-model="whiteboardTitle"
            type="text"
            class="form-control bg-dark text-light border-secondary"
            placeholder="Whiteboard title"
          />
          <label class="form-label mt-3 mb-1">Join Policy</label>
          <select
            v-model="selectedJoinPolicy"
            class="form-select bg-dark text-light border-secondary"
          >
            <option :value="WhiteboardJoinPolicy.FreeToJoin">Free to Join</option>
            <option :value="WhiteboardJoinPolicy.RequestToJoin">Request to Join</option>
            <option :value="WhiteboardJoinPolicy.Private">Private</option>
          </select>
          <label class="form-label mt-3 mb-1">Max Participants</label>
          <input
            v-model.number="maxParticipants"
            type="number"
            min="2"
            max="50"
            class="form-control bg-dark text-light border-secondary"
          />
        </div>
        <div class="modal-footer border-secondary">
          <button type="button" class="btn btn-secondary" @click="showCreateModal = false">Cancel</button>
          <button type="button" class="btn btn-primary" @click="handleCreateNewWhiteboard">
            <span v-if="whiteboards.isLoading" class="spinner-border spinner-border-sm me-2"></span>
            Create
          </button>
        </div>
      </div>
    </div>
  </div>
  <div v-if="showCreateModal" class="modal-backdrop fade show"></div>

  <WhiteboardHistorySidebar v-if="auth.isAuthenticated" />
  <RecentWhiteboardsPanel v-if="auth.isAuthenticated" />
</template>

<style scoped>

</style>
