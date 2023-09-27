using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetTodoItems();
        Task<TodoItem> GetTodoItem(long id);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItem(TodoItem todoItem);
        Task<TodoItem> DeleteTodoItem(long id);
    }
}