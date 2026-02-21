using Microsoft.AspNetCore.Mvc;
using TodoAppTakeHome.Api.DTOs;
using TodoAppTakeHome.Api.Services;

namespace TodoAppTakeHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    [HttpGet]
    public async Task<IEnumerable<TaskResponse>> GetAll() => await _taskService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var created = await _taskService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> Update(Guid id, UpdateTaskRequest request)
    {
        var updated = await _taskService.UpdateAsync(id, request);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _taskService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
