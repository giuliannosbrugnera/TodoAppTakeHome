import { TaskItemStatus } from '../types/tasks';

const statusLabels: Record<TaskItemStatus, string> = {
  [TaskItemStatus.Todo]: 'Todo',
  [TaskItemStatus.InProgress]: 'In Progress',
  [TaskItemStatus.Done]: 'Done',
};

export const useTaskStatus = () => {
  const getStatusLabel = (status: TaskItemStatus) => statusLabels[status] ?? 'Unknown';
  const allStatuses = Object.values(TaskItemStatus);
  return { getStatusLabel, allStatuses };
};
