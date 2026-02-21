using TodoAppTakeHome.Api.Entities;

namespace TodoAppTakeHome.Api.DTOs;

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus? Status { get; set; }
    public DateTime? DueDate { get; set; }
}
