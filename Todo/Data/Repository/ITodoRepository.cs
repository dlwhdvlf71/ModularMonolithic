namespace Todo.Data.Repository
{
    public interface ITodoRepository
    {
        Task<Models.Todo> CreateTodoAsync(Todo.Models.Todo todo, CancellationToken cancellationToken = default);

        Task<Models.Todo> GetTodoAsync(Int64 id, CancellationToken cancellationToken = default);

        Task<Models.Todo> GetTodoAsync(string name, CancellationToken cancellationToken = default);
    }
}