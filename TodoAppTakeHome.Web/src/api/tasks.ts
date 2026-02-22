import axios from 'axios';
import type { TaskResponse, CreateTaskRequest, UpdateTaskRequest } from '../types/tasks';

// Axios instance for backend API
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5274/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// -------------------
// Task API methods
// -------------------

export async function getAllTasks(): Promise<TaskResponse[]> {
  const response = await api.get<TaskResponse[]>('/tasks');
  return response.data;
}

export async function getTaskById(id: string): Promise<TaskResponse> {
  const response = await api.get<TaskResponse>(`/tasks/${id}`);
  return response.data;
}

export async function createTask(payload: CreateTaskRequest): Promise<TaskResponse> {
  const response = await api.post<TaskResponse>('/tasks', payload);
  return response.data;
}

export async function updateTask(id: string, payload: UpdateTaskRequest): Promise<TaskResponse> {
  const response = await api.put<TaskResponse>(`/tasks/${id}`, payload);
  return response.data;
}

export async function deleteTask(id: string): Promise<void> {
  await api.delete(`/tasks/${id}`);
}
