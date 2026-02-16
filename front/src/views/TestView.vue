<script setup lang="ts">
import {onMounted, ref} from "vue";
import {useRouter} from "vue-router";
import {testHubService} from "@/services/testHubService.ts";

const router = useRouter();
const displayText = ref("");
const whiteboardId = ref("");

function addText(textToAdd: string): void {
  displayText.value = displayText.value + textToAdd;
}

function joinWhiteboard() {
  const id = whiteboardId.value.trim();
  if (!id) return;
  router.push(`/whiteboard/${id}`);
}

onMounted(async () => {
  await testHubService.connect();

  testHubService.onTest((text) => {
    addText(text);
  })
})
</script>

<template>
<h1>{{ displayText }}</h1>

<div class="mt-4" style="max-width: 500px">
  <div class="input-group">
    <input
      v-model="whiteboardId"
      type="text"
      class="form-control"
      placeholder="Whiteboard ID (GUID)"
      @keyup.enter="joinWhiteboard"
    />
    <button class="btn btn-primary" @click="joinWhiteboard">
      Join Whiteboard
    </button>
  </div>
</div>
</template>
