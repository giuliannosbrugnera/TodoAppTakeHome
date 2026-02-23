<template>
  <li class="p-4 border rounded shadow-sm hover:bg-gray-50 transition">
    <!-- View Mode -->
    <div v-if="editingTaskId !== task.id">
      <strong class="text-lg">{{ task.title }}</strong>
      <p v-if="task.description" class="text-gray-700 mt-1">{{ task.description }}</p>
      <small class="text-gray-500 block mt-2">
        Status: {{ getStatusLabel(task.status) }} | Created: {{ formatDate(task.createdAt) }}
        <span v-if="task.dueDate">| Due: {{ formatDate(task.dueDate) }}</span>
      </small>

      <div class="mt-2 flex items-center space-x-2">
        <select v-model="task.status" @change="updateStatus" class="border p-1 rounded">
          <option v-for="status in Object.values(TaskItemStatus)" :key="status" :value="status">
            {{ getStatusLabel(status) }}
          </option>
        </select>

        <button
          @click="startEdit"
          class="px-2 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600 transition"
        >
          Edit
        </button>

        <button
          @click="deleteTaskItem"
          class="px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600 transition"
        >
          Delete
        </button>
      </div>
    </div>

    <!-- Edit Mode -->
    <div v-else class="space-y-2">
      <input v-model="editTitle" class="w-full border p-2 rounded" />
      <textarea v-model="editDescription" class="w-full border p-2 rounded"></textarea>
      <div class="flex flex-col space-y-1">
        <label for="dueDate" class="text-sm font-medium text-gray-700">Due Date</label>
        <input id="dueDate" v-model="editDueDate" type="date" class="border p-2 rounded w-full" />
      </div>
      <div class="flex space-x-2">
        <button
          @click="saveEdit"
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
</template>

<script setup lang="ts">
import { ref } from 'vue';
import type { TaskResponse } from '../types/tasks';
import { TaskItemStatus } from '../types/tasks';
import { updateTask, deleteTask } from '../api/tasks';

// Props
const props = defineProps<{
  task: TaskResponse;
  editingTaskId: string | null;
}>();

// Emits
const emit = defineEmits<{
  (e: 'deleted', id: string): void;
  (e: 'updated', task: TaskResponse): void;
  (e: 'start-edit', task: TaskResponse): void;
  (e: 'cancel-edit'): void;
  (e: 'error', message: string): void;
}>();

// Reactive state
const editTitle = ref(props.task.title);
const editDescription = ref(props.task.description ?? '');
const editDueDate = ref(props.task.dueDate ?? null);

// Helpers
const statusLabels: Record<TaskItemStatus, string> = {
  [TaskItemStatus.Todo]: 'Todo',
  [TaskItemStatus.InProgress]: 'In Progress',
  [TaskItemStatus.Done]: 'Done',
};
const getStatusLabel = (status: TaskItemStatus) => statusLabels[status] ?? 'Unknown';
const formatDate = (dateStr: string) => new Date(dateStr).toLocaleString();

// Methods
const updateStatus = async () => {
  try {
    const updated = await updateTask(props.task.id, { status: props.task.status });
    emit('updated', updated);
  } catch (err) {
    console.error(err);
    emit('error', 'Failed to update task status.');
  }
};

const deleteTaskItem = async () => {
  try {
    await deleteTask(props.task.id);
    emit('deleted', props.task.id);
  } catch (err) {
    console.error(err);
    emit('error', 'Failed to delete task.');
  }
};

const startEdit = () => {
  emit('start-edit', props.task);
};

const cancelEdit = () => {
  emit('cancel-edit');
};

const saveEdit = async () => {
  try {
    const updated = await updateTask(props.task.id, {
      title: editTitle.value,
      description: editDescription.value || undefined,
    });
    emit('updated', updated);
  } catch (err) {
    console.error(err);
    emit('error', 'Failed to save changes.');
  }
};
</script>
