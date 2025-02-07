using Mapster;
using Shared.CQRS;
using Todo.Data.Repository;
using Todo.Dtos;

namespace Todo.Features.GetTodo
{
    public record GetTodoQuery(Int64 id) : IQuery<GetTodoResult>;
    public record GetTodoResult(TodoDto todoDto);

    internal class GetTodoHandler(ITodoRepository repository) : IQueryHandler<GetTodoQuery, GetTodoResult>
    {
        public async Task<GetTodoResult> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var todo = await repository.GetTodoAsync(request.id, cancellationToken);

            return new GetTodoResult(todo.Adapt<TodoDto>());
        }
    }
}