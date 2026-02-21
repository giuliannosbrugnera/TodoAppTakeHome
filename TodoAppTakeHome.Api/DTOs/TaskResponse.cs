using TodoAppTakeHome.Api.Entities;

namespace TodoAppTakeHome.Api.DTOs;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
}
