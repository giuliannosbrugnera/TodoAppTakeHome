using Microsoft.EntityFrameworkCore;
using TodoAppTakeHome.Api.Data;
using TodoAppTakeHome.Api.DTOs;
using TodoAppTakeHome.Api.Entities;

namespace TodoAppTakeHome.Api.Services;

public class TaskService(AppDbContext context) : ITaskService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<TaskResponse>> GetAllAsync() =>
        await _context.Tasks.Select(t => ToResponse(t)).ToListAsync();

    public async Task<TaskResponse?> GetByIdAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        return task == null ? null : ToResponse(task);
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
        };
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return ToResponse(task);
    }

    public async Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return null;

        if (!string.IsNullOrEmpty(request.Title))
            task.Title = request.Title;
        if (request.Description != null)
            task.Description = request.Description;
        if (request.Status.HasValue)
            task.Status = request.Status.Value;
        if (request.DueDate.HasValue)
            task.DueDate = request.DueDate;

        await _context.SaveChangesAsync();
        return ToResponse(task);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return false;
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    private static TaskResponse ToResponse(TaskItem task) =>
        new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
        };
}
