<template>
  <div class="task-list p-6 max-w-3xl mx-auto">
    <h1 class="text-3xl font-bold text-blue-600 mb-4">Tasks</h1>

    <div v-if="error" class="bg-red-100 text-red-700 p-2 rounded mb-4">
      {{ error }}
    </div>

    <div class="flex space-x-4 items-center mb-4">
      <select v-model="filterStatus" class="border p-1 rounded">
        <option value="">All Status</option>
        <option v-for="status in allStatuses" :key="status" :value="status">
          {{ getStatusLabel(status) }}
        </option>
      </select>

      <select v-model="sortOrder" class="border p-1 rounded">
        <option value="asc">Due Date Asc</option>
        <option value="desc">Due Date Desc</option>
      </select>
    </div>

    <!-- Add Task Form -->
    <TaskForm @task-added="onTaskAdded" @error="error = $event" />

    <div v-if="loading" class="text-gray-500 mt-4">Loading tasks...</div>
    <div v-else-if="tasks.length === 0" class="text-gray-500 mt-4">No tasks found.</div>

    <ul v-else class="mt-4 space-y-4">
      <TaskItem
        v-for="task in filteredAndSortedTasks"
        :key="task.id"
        :task="task"
        :editing-task-id="editingTaskId"
        @deleted="onTaskDeleted"
        @updated="onTaskUpdated"
        @start-edit="editingTaskId = $event?.id ?? null"
        @cancel-edit="editingTaskId = null"
        @error="error = $event"
      />
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import TaskForm from './TaskForm.vue';
import TaskItem from './TaskItem.vue';
import { getAllTasks } from '../api/tasks';
import type { TaskResponse } from '../types/tasks';
import { useTaskStatus } from '../composables/useTaskStatus';

const { getStatusLabel, allStatuses } = useTaskStatus();

// Reactive state
const tasks = ref<TaskResponse[]>([]);
const loading = ref(true);
const error = ref<string | null>(null);
const filterStatus = ref<string>('');
const sortOrder = ref<'asc' | 'desc'>('asc');

// Clear error after a few seconds
watch(error, (val) => {
  if (val) {
    setTimeout(() => (error.value = null), 5000);
  }
});

// Editing state
const editingTaskId = ref<string | null>(null);

// Fetch tasks from API
const fetchTasks = async () => {
  loading.value = true;
  try {
    tasks.value = await getAllTasks();
  } catch (err) {
    error.value = 'Failed to fetch tasks from API.';
    console.error(err);
  } finally {
    loading.value = false;
  }
};

// Delete task
const onTaskDeleted = async (id: string) => {
  tasks.value = tasks.value.filter((t) => t.id !== id);
};

// Handle event emitted from TaskForm
const onTaskAdded = (task: TaskResponse) => {
  // Add new task to top
  tasks.value.unshift(task);
};

const onTaskUpdated = (updatedTask: TaskResponse) => {
  const index = tasks.value.findIndex((t) => t.id === updatedTask.id);
  if (index !== -1) {
    tasks.value[index] = updatedTask;
  }

  // Exit edit mode
  editingTaskId.value = null;
};

const filteredAndSortedTasks = computed(() => {
  let list = [...tasks.value];

  if (filterStatus.value) {
    list = list.filter((t) => t.status === filterStatus.value);
  }

  return list.sort((a, b) => {
    const dateA = a.dueDate ? new Date(a.dueDate).getTime() : 0;
    const dateB = b.dueDate ? new Date(b.dueDate).getTime() : 0;
    return sortOrder.value === 'asc' ? dateA - dateB : dateB - dateA;
  });
});

// Fetch tasks on mount
onMounted(fetchTasks);
</script>
