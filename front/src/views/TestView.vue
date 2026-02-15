<script setup lang="ts">
import {onMounted, ref} from "vue";
import {testHubService} from "@/services/testHubService.ts";

const displayText = ref("");

function addText(textToAdd: string): void {
  displayText.value = displayText.value + textToAdd;
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
</template>
