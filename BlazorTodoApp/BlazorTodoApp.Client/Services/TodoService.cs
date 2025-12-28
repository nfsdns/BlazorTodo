using BlazorTodoApp.Shared;

namespace BlazorTodoApp.Client.Services;


public class TodoService
{

    private List<TodoItem> _todos = new();
    public IReadOnlyList<TodoItem> Todos => _todos;

    public void Add(string title)
    {
        var newItem = new TodoItem
        {
            Id = _todos.Any() ? _todos.Max(t => t.Id) + 1 : 1,
            Title = title,
            IsDone = false
        };
        _todos.Add(newItem);
    }

    public void Toggle(int id)
    {
        var item = _todos.FirstOrDefault(t => t.Id == id);
        if (item != null)
        {
            item.IsDone = !item.IsDone;
        }
    }

    public void Remove(int id)
    {
        var item = _todos.FirstOrDefault(t => t.Id == id);
        if (item != null)
        {
            _todos.Remove(item);
        }
    }
}
