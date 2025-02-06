using Mapster;
using Shared.CQRS;
using Todo.Data.Repository;
using Todo.Dtos;

namespace Todo.Features.GetTodo
{
    public record GetTodoCommand(TodoDto dto) : ICommand<GetTodoResult>;
    public record GetTodoResult(string id);

    internal class GetTodoHandler(ITodoRepository repository) : ICommandHandler<GetTodoCommand, GetTodoResult>
    {
        public async Task<GetTodoResult> Handle(GetTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = command.dto.Adapt<Models.Todo>();

            await repository.CreateTodoAsync(todo, cancellationToken);

            return new GetTodoResult(todo.Id.ToString());
        }
    }
}