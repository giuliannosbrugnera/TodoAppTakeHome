<template>
  <div class="task-list p-6 max-w-3xl mx-auto">
    <h1 class="text-3xl font-bold text-blue-600 mb-4">Tasks</h1>

    <!-- Add Task Form -->
    <TaskForm @task-added="handleTaskAdded" />

    <div v-if="loading" class="text-gray-500 mt-4">Loading tasks...</div>
    <div v-else-if="tasks.length === 0" class="text-gray-500 mt-4">No tasks found.</div>

    <ul v-else class="mt-4 space-y-4">
      <li
        v-for="task in tasks"
        :key="task.id"
        class="p-4 border rounded shadow-sm hover:bg-gray-50 transition"
      >
        <strong class="text-lg">{{ task.title }}</strong>
        <p v-if="task.description" class="text-gray-700 mt-1">{{ task.description }}</p>
        <small class="text-gray-500 block mt-2">
          Status: {{ task.status }} | Created: {{ formatDate(task.createdAt) }}
        </small>
        <div class="mt-2 flex items-center space-x-2">
          <select v-model="task.status" @change="updateTaskStatus(task)" class="border p-1 rounded">
            <option value="Todo">Todo</option>
            <option value="InProgress">In Progress</option>
            <option value="Done">Done</option>
          </select>

          <button
            @click="deleteTaskItem(task.id)"
            class="px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600 transition"
          >
            Delete
          </button>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import TaskForm from './TaskForm.vue';
import { getAllTasks, deleteTask, updateTask } from '../api/tasks';
import type { TaskResponse } from '../types/tasks';

// Reactive state
const tasks = ref<TaskResponse[]>([]);
const loading = ref(true);

// Fetch tasks from API
const fetchTasks = async () => {
  loading.value = true;
  try {
    tasks.value = await getAllTasks();
  } finally {
    loading.value = false;
  }
};

// Delete task
const deleteTaskItem = async (id: string) => {
  await deleteTask(id);
  tasks.value = tasks.value.filter((t) => t.id !== id);
};

// Update task status
const updateTaskStatus = async (task: TaskResponse) => {
  await updateTask(task.id, { status: task.status });
};

// Handle event emitted from TaskForm
const handleTaskAdded = (task: TaskResponse) => {
  // Add new task to top
  tasks.value.unshift(task);
};

// Format date
const formatDate = (dateStr: string) => new Date(dateStr).toLocaleString();

// Fetch tasks on mount
onMounted(() => {
  fetchTasks();
});
</script>
