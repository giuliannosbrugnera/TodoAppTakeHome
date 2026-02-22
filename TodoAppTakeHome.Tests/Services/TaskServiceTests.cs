using TodoAppTakeHome.Api.DTOs;
using TodoAppTakeHome.Api.Entities;
using TodoAppTakeHome.Api.Services;
using TodoAppTakeHome.Tests.Factories;

namespace TodoAppTakeHome.Tests.Services;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Add_New_Task()
    {
        using var context = InMemoryDbContextFactory.Create();
        var service = new TaskService(context);

        var request = new CreateTaskRequest { Title = "Test Task", Description = "Desc" };
        var result = await service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(TaskItemStatus.Todo, result.Status);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Tasks()
    {
        using var context = InMemoryDbContextFactory.Create();
        context.Tasks.Add(new TaskItem { Title = "Task 1" });
        context.Tasks.Add(new TaskItem { Title = "Task 2" });
        await context.SaveChangesAsync();

        var service = new TaskService(context);
        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Title == "Task 1");
        Assert.Contains(result, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Task_If_Found()
    {
        using var context = InMemoryDbContextFactory.Create();
        var task = new TaskItem { Title = "Task 1" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service = new TaskService(context);
        var result = await service.GetByIdAsync(task.Id);

        Assert.NotNull(result);
        Assert.Equal(task.Title, result!.Title);
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Existing_Task()
    {
        using var context = InMemoryDbContextFactory.Create();
        var task = new TaskItem { Title = "Old Title" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service = new TaskService(context);
        var updated = await service.UpdateAsync(
            task.Id,
            new UpdateTaskRequest { Title = "New Title", Status = TaskItemStatus.InProgress }
        );

        Assert.NotNull(updated);
        Assert.Equal("New Title", updated!.Title);
        Assert.Equal(TaskItemStatus.InProgress, updated.Status);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Task()
    {
        using var context = InMemoryDbContextFactory.Create();
        var task = new TaskItem { Title = "Task to delete" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service = new TaskService(context);
        var deleted = await service.DeleteAsync(task.Id);

        Assert.True(deleted);
        var exists = await context.Tasks.FindAsync(task.Id);
        Assert.Null(exists);
    }
}
