import { render, fireEvent } from '@testing-library/vue';
import TaskList from '../TaskList.vue';
import type { TaskResponse } from '../../types/tasks';

vi.mock('../../api/tasks', () => {
  const mockTasks: TaskResponse[] = [
    { id: '1', title: 'Task 1', status: 'Todo', createdAt: '2026-01-01', description: '' },
    { id: '2', title: 'Task 2', status: 'InProgress', createdAt: '2026-01-02', description: '' },
    { id: '3', title: 'Task 3', status: 'Done', createdAt: '2026-01-03', description: '' },
  ];

  return {
    getAllTasks: vi.fn().mockResolvedValue(mockTasks),
  };
});

describe('TaskList', () => {
  it('filters tasks by status', async () => {
    const { getByLabelText, findByText, queryByText } = render(TaskList);

    // Wait for tasks to load
    await findByText('Task 1');

    const filterSelect = getByLabelText('Filter by Status') as HTMLSelectElement;

    await fireEvent.update(filterSelect, 'InProgress');

    expect(await findByText('Task 2')).toBeTruthy();
    expect(queryByText('Task 1')).toBeNull();
    expect(queryByText('Task 3')).toBeNull();
  });
});
