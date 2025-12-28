namespace BlazorTodoApp.Shared;

public class TodoItem
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public bool IsDone { get; set; }
}
