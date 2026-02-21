namespace TodoAppTakeHome.Api.DTOs;

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public DateTime? DueDate { get; set; }
}
