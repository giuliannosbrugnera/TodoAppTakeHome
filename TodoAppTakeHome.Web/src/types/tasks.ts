export interface TaskResponse {
  id: string;
  title: string;
  description?: string;
  status: 'Todo' | 'InProgress' | 'Done';
  createdAt: string;
  dueDate?: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  dueDate?: string;
}

export interface UpdateTaskRequest {
  title?: string;
  description?: string;
  status?: 'Todo' | 'InProgress' | 'Done';
  dueDate?: string;
}
