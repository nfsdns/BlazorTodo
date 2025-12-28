using BlazorTodoApp.Shared;
using Microsoft.JSInterop;

namespace BlazorTodoApp.Client.Services;

public class TodoService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "todos";

    public List<TodoItem> Todos { get; private set; } = new List<TodoItem>();

    public TodoService(IJSRuntime js)
    {
        _js = js;
    }

    // Load todos from localStorage
    public async Task LoadAsync()
    {
        var loaded = await _js.InvokeAsync<List<TodoItem>>("todoStorage.load", StorageKey);
        Todos = loaded ?? new List<TodoItem>();
    }

    // Add a new todo
    public async Task AddAsync(string title)
    {
        Todos.Add(new TodoItem { Id = Guid.NewGuid(), Title = title });
        await SaveAsync();
    }

    // Toggle done
    public async Task ToggleAsync(Guid id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo != null)
        {
            todo.IsDone = !todo.IsDone;
            await SaveAsync();
        }
    }

    // Remove a todo
    public async Task RemoveAsync(Guid id)
    {
        Todos.RemoveAll(t => t.Id == id);
        await SaveAsync();
    }

    // Save todos to localStorage
    private async Task SaveAsync()
    {
        await _js.InvokeVoidAsync("todoStorage.save", StorageKey, Todos);
    }
}
