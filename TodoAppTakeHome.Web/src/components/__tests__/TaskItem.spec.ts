import { render, fireEvent, waitFor } from '@testing-library/vue';
import TaskItem from '../TaskItem.vue';
import type { TaskResponse } from '../../types/tasks';

vi.mock('../../api/tasks', () => ({
  updateTask: vi.fn(),
  deleteTask: vi.fn(),
}));

import { updateTask, deleteTask } from '../../api/tasks';

describe('TaskItem.vue', () => {
  const task: TaskResponse = {
    id: '1',
    title: 'Test Task',
    description: 'Test Description',
    status: 'Todo',
    createdAt: new Date().toISOString(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('emits "start-edit" when Edit button clicked', async () => {
    const { getByText, emitted } = render(TaskItem, {
      props: { task, editingTaskId: null },
    });

    const editButton = getByText('Edit');
    await fireEvent.click(editButton);

    const events = emitted()['start-edit'];
    expect(events).toHaveLength(1);
    expect(events![0][0]).toEqual(task);
  });

  it('emits "cancel-edit" when Cancel button clicked in edit mode', async () => {
    const { getByText, emitted } = render(TaskItem, {
      props: { task, editingTaskId: task.id },
    });

    const cancelButton = getByText('Cancel');
    await fireEvent.click(cancelButton);

    const events = emitted()['cancel-edit'];
    expect(events).toHaveLength(1);
  });

  it('emits "updated" after status change', async () => {
    (updateTask as any).mockResolvedValue({ ...task, status: 'Done' });

    const { getByRole, emitted } = render(TaskItem, {
      props: { task, editingTaskId: null },
    });

    const select = getByRole('combobox') as HTMLSelectElement;
    await fireEvent.update(select, 'Done');

    await waitFor(() => {
      expect(updateTask).toHaveBeenCalledWith(task.id, { status: 'Done' });
    });

    const events = emitted()['updated'];
    expect(events).toHaveLength(1);
    expect(events![0][0].status).toBe('Done');
  });

  it('emits "deleted" when Delete button clicked', async () => {
    (deleteTask as any).mockResolvedValue(undefined);

    const { getByText, emitted } = render(TaskItem, {
      props: { task, editingTaskId: null },
    });

    const deleteButton = getByText('Delete');
    await fireEvent.click(deleteButton);

    await waitFor(() => {
      expect(deleteTask).toHaveBeenCalledWith(task.id);
    });

    const events = emitted()['deleted'];
    expect(events).toHaveLength(1);
    expect(events![0][0]).toBe(task.id);
  });

  it('emits "error" if updateTask fails', async () => {
    (updateTask as any).mockRejectedValue(new Error('API failed'));

    const { getByRole, emitted } = render(TaskItem, {
      props: { task, editingTaskId: null },
    });

    const select = getByRole('combobox') as HTMLSelectElement;
    await fireEvent.update(select, 'Done');

    await waitFor(() => {
      const errorEvents = emitted()['error'];
      expect(errorEvents).toHaveLength(1);
      expect(errorEvents![0][0]).toBe('Failed to update task status.');
    });
  });

  it('emits "error" if deleteTask fails', async () => {
    (deleteTask as any).mockRejectedValue(new Error('API failed'));

    const { getByText, emitted } = render(TaskItem, {
      props: { task, editingTaskId: null },
    });

    const deleteButton = getByText('Delete');
    await fireEvent.click(deleteButton);

    await waitFor(() => {
      const errorEvents = emitted()['error'];
      expect(errorEvents).toHaveLength(1);
      expect(errorEvents![0][0]).toBe('Failed to delete task.');
    });
  });
});
