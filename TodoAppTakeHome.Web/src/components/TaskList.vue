<template>
  <div class="task-list p-6 max-w-3xl mx-auto">
    <h1 class="text-3xl font-bold text-blue-600 mb-4">Tasks</h1>

    <div v-if="loading" class="text-gray-500">Loading tasks...</div>
    <div v-else-if="tasks.length === 0" class="text-gray-500">No tasks found.</div>

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
        <button
          @click="deleteTaskItem(task.id)"
          class="mt-2 px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600 transition"
        >
          Delete
        </button>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue';
import { getAllTasks, deleteTask } from '../api/tasks';
import type { TaskResponse } from '../types/tasks';

export default {
  name: 'TaskList',
  setup() {
    const tasks = ref<TaskResponse[]>([]);
    const loading = ref(true);

    const fetchTasks = async () => {
      loading.value = true;
      try {
        tasks.value = await getAllTasks();
      } finally {
        loading.value = false;
      }
    };

    const deleteTaskItem = async (id: string) => {
      await deleteTask(id);
      // refresh list after deletion
      tasks.value = tasks.value.filter((t) => t.id !== id);
    };

    const formatDate = (dateStr: string) => new Date(dateStr).toLocaleString();

    onMounted(() => {
      fetchTasks();
    });

    return { tasks, loading, fetchTasks, deleteTaskItem, formatDate };
  },
};
</script>
