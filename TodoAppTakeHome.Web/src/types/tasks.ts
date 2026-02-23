export const TaskItemStatus = {
  Todo: 'Todo',
  InProgress: 'InProgress',
  Done: 'Done',
} as const;

export type TaskItemStatus = (typeof TaskItemStatus)[keyof typeof TaskItemStatus];

export interface TaskResponse {
  id: string;
  title: string;
  description?: string;
  status: TaskItemStatus;
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
  status?: TaskItemStatus;
  dueDate?: string;
}
