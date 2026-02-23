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
        <!-- View Mode -->
        <div v-if="editingTaskId !== task.id">
          <strong class="text-lg">{{ task.title }}</strong>

          <p v-if="task.description" class="text-gray-700 mt-1">
            {{ task.description }}
          </p>

          <small class="text-gray-500 block mt-2">
            Status: {{ getStatusLabel(task.status) }} | Created: {{ formatDate(task.createdAt) }}
          </small>

          <div class="mt-2 flex items-center space-x-2">
            <select
              v-model="task.status"
              @change="updateTaskStatus(task)"
              class="border p-1 rounded"
            >
              <option v-for="status in Object.values(TaskItemStatus)" :key="status" :value="status">
                {{ getStatusLabel(status) }}
              </option>
            </select>

            <button
              @click="startEditing(task)"
              class="px-2 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600 transition"
            >
              Edit
            </button>

            <button
              @click="deleteTaskItem(task.id)"
              class="px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600 transition"
            >
              Delete
            </button>
          </div>
        </div>

        <!-- Edit Mode -->
        <div v-else class="space-y-2">
          <input v-model="editTitle" class="w-full border p-2 rounded" />

          <textarea v-model="editDescription" class="w-full border p-2 rounded" />

          <div class="flex space-x-2">
            <button
              @click="saveEdit(task)"
              class="px-3 py-1 bg-green-600 text-white rounded hover:bg-green-700 transition"
            >
              Save
            </button>

            <button
              @click="cancelEdit"
              class="px-3 py-1 bg-gray-300 rounded hover:bg-gray-400 transition"
            >
              Cancel
            </button>
          </div>
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
import { TaskItemStatus } from '../types/tasks';

// Status Label Mapping (UI helper)
const statusLabels: Record<TaskItemStatus, string> = {
  [TaskItemStatus.Todo]: 'Todo',
  [TaskItemStatus.InProgress]: 'In Progress',
  [TaskItemStatus.Done]: 'Done',
};

const getStatusLabel = (status: TaskItemStatus) => statusLabels[status] ?? 'Unknown';

// Reactive state
const tasks = ref<TaskResponse[]>([]);
const loading = ref(true);

// Editing state
const editingTaskId = ref<string | null>(null);
const editTitle = ref('');
const editDescription = ref('');

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

// Start editing
const startEditing = (task: TaskResponse) => {
  editingTaskId.value = task.id;
  editTitle.value = task.title;
  editDescription.value = task.description ?? '';
};

// Cancel editing
const cancelEdit = () => {
  editingTaskId.value = null;
};

// Save edit
const saveEdit = async (task: TaskResponse) => {
  const updated = await updateTask(task.id, {
    title: editTitle.value,
    description: editDescription.value || undefined,
  });

  const index = tasks.value.findIndex((t) => t.id === task.id);
  if (index !== -1) {
    tasks.value[index] = updated;
  }

  editingTaskId.value = null;
};

// Handle event emitted from TaskForm
const handleTaskAdded = (task: TaskResponse) => {
  // Add new task to top
  tasks.value.unshift(task);
};

// Format date
const formatDate = (dateStr: string) => new Date(dateStr).toLocaleString();

// Fetch tasks on mount
onMounted(fetchTasks);
</script>
