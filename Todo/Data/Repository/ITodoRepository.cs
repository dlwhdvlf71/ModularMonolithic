namespace Todo.Data.Repository
{
    public interface ITodoRepository
    {
        Task<Todo.Models.Todo> GetTodoAsync(string name, CancellationToken cancellationToken = default);

        Task<Todo.Models.Todo> CreateTodoAsync(Todo.Models.Todo todo, CancellationToken cancellationToken = default);
    }
}