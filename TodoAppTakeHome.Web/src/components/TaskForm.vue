<template>
  <form @submit.prevent="addTask" class="space-y-4">
    <input
      v-model="title"
      type="text"
      placeholder="Task title"
      class="w-full border p-2 rounded"
      required
    />
    <textarea
      v-model="description"
      placeholder="Description"
      class="w-full border p-2 rounded"
    ></textarea>
    <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
      Add Task
    </button>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { createTask } from '../api/tasks';
import type { TaskResponse } from '../types/tasks';

// Define a typed event emitter for the parent
const emit = defineEmits<{
  (e: 'task-added', task: TaskResponse): void;
}>();

// Reactive form state
const title = ref('');
const description = ref('');

// Add task handler
const addTask = async () => {
  if (!title.value) return;

  const newTask = await createTask({
    title: title.value,
    description: description.value || undefined,
  });

  // Emit event to parent
  emit('task-added', newTask);

  // Reset form
  title.value = '';
  description.value = '';
};
</script>
