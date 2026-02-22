<template>
  <div class="task-list">
    <h1 class="text-3xl font-bold text-blue-600">Tasks</h1>

    <div v-if="loading">Loading tasks...</div>
    <div v-else-if="tasks.length === 0">No tasks found.</div>
    <ul v-else>
      <li v-for="task in tasks" :key="task.id" class="task-item">
        <strong>{{ task.title }}</strong>
        <p v-if="task.description">{{ task.description }}</p>
        <small>Status: {{ task.status }} | Created: {{ formatDate(task.createdAt) }}</small>
        <button @click="deleteTaskItem(task.id)">Delete</button>
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

<style scoped>
.task-list {
  max-width: 600px;
  margin: 0 auto;
}

.task-item {
  border: 1px solid #ccc;
  padding: 12px;
  margin-bottom: 8px;
  border-radius: 4px;
}

.task-item button {
  margin-top: 4px;
  background-color: #ff4d4f;
  color: white;
  border: none;
  padding: 4px 8px;
  cursor: pointer;
  border-radius: 3px;
}

.task-item button:hover {
  background-color: #d9363e;
}
</style>
