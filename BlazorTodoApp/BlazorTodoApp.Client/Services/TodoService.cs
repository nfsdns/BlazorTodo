using BlazorTodoApp.Shared;
using Microsoft.JSInterop;

namespace BlazorTodoApp.Client.Services;

public class TodoService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "todos";

    /// <summary>
    /// List of todos in memory.
    /// </summary>
    public List<TodoItem> Todos { get; private set; } = new List<TodoItem>();

    public TodoService(IJSRuntime js)
    {
        _js = js;
    }

    /// <summary>
    /// Load todos from browser localStorage.
    /// </summary>
    public async Task LoadAsync()
    {
        var loaded = await _js.InvokeAsync<List<TodoItem>>("todoStorage.load", StorageKey);
        Todos = loaded ?? new List<TodoItem>();
    }

    /// <summary>
    /// Add a new todo.
    /// </summary>
    public async Task AddAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return;

        Todos.Add(new TodoItem { Id = Guid.NewGuid(), Title = title.Trim() });
        await SaveAsync();
    }

    /// <summary>
    /// Toggle the IsDone status of a todo by Id.
    /// </summary>
    public async Task ToggleAsync(Guid id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo != null)
        {
            todo.IsDone = !todo.IsDone;
            await SaveAsync();
        }
    }

    /// <summary>
    /// Remove a todo by Id.
    /// </summary>
    public async Task RemoveAsync(Guid id)
    {
        Todos.RemoveAll(t => t.Id == id);
        await SaveAsync();
    }

    /// <summary>
    /// Save todos to browser localStorage.
    /// </summary>
    private async Task SaveAsync()
    {
        await _js.InvokeVoidAsync("todoStorage.save", StorageKey, Todos);
    }
}
