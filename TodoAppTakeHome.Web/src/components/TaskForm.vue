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
    <div class="flex flex-col space-y-1">
      <label for="dueDate" class="text-sm font-medium text-gray-700">Due Date</label>
      <input id="dueDate" v-model="dueDate" type="date" class="border p-2 rounded w-full" />
    </div>
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
  (e: 'error', message: string): void;
}>();

// Reactive form state
const title = ref('');
const description = ref('');
const dueDate = ref<string | null>(null);

// Add task handler
const addTask = async () => {
  if (!title.value) return;

  try {
    const newTask = await createTask({
      title: title.value,
      description: description.value || undefined,
      dueDate: dueDate.value || undefined,
    });

    // Emit event to parent
    emit('task-added', newTask);

    // Reset form
    title.value = '';
    description.value = '';
    dueDate.value = null;
  } catch (err) {
    console.log(err);
    emit('error', 'Failed to add task item.');
  }
};
</script>
