using Microsoft.EntityFrameworkCore;

namespace Todo.Data.Repository
{
    public class TodoRepository(TodoDbContext dbContext) : ITodoRepository
    {
        public async Task<Models.Todo> CreateTodoAsync(Models.Todo todo, CancellationToken cancellationToken = default)
        {
            await dbContext.Todos.AddAsync(todo, cancellationToken);
            var tt = await dbContext.SaveChangesAsync(cancellationToken);

            return todo;
        }

        public async Task<Models.Todo> GetTodoAsync(string name, CancellationToken cancellationToken = default)
        {
            var item = await dbContext.Todos.Where(todo => todo.Equals(name)).SingleOrDefaultAsync(cancellationToken);

            return item ?? throw new NullReferenceException();
        }
    }
}