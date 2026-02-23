import { render, fireEvent, waitFor } from '@testing-library/vue';
import TaskForm from '../TaskForm.vue';
import type { TaskResponse } from '../../types/tasks';

vi.mock('../../api/tasks', () => ({
  createTask: vi.fn(),
}));

import { createTask } from '../../api/tasks';

describe('TaskForm.vue', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  const taskMock: TaskResponse = {
    id: '1',
    title: 'Test Task',
    description: 'Test Description',
    status: 'Todo',
    createdAt: new Date().toISOString(),
  };

  it('emits "task-added" after successful submission', async () => {
    (createTask as any).mockResolvedValue(taskMock);

    const { getByPlaceholderText, getByText, emitted } = render(TaskForm);

    const titleInput = getByPlaceholderText('Task title') as HTMLInputElement;
    const descriptionInput = getByPlaceholderText('Description') as HTMLTextAreaElement;
    const addButton = getByText('Add Task');

    await fireEvent.update(titleInput, 'Test Task');
    await fireEvent.update(descriptionInput, 'Test Description');
    await fireEvent.click(addButton);

    await waitFor(() => {
      const events = emitted()['task-added'];
      expect(events).toHaveLength(1);
      expect(events![0][0].title).toBe('Test Task');
      expect(events![0][0].description).toBe('Test Description');
    });
  });

  it('emits "error" if createTask fails', async () => {
    (createTask as any).mockRejectedValue(new Error('API failed'));

    const { getByPlaceholderText, getByText, emitted } = render(TaskForm);

    await fireEvent.update(getByPlaceholderText('Task title'), 'Test Task');
    await fireEvent.click(getByText('Add Task'));

    await waitFor(() => {
      const errorEvents = emitted()['error'];
      expect(errorEvents).toHaveLength(1);
      expect(errorEvents![0][0]).toBe('Failed to add task item.');
    });
  });
});
