using Mapster;
using Shared.CQRS;
using Todo.Data.Repository;
using Todo.Dtos;

namespace Todo.Features.CreateTodo
{
    public record CreateTodoCommand(TodoDto dto) : ICommand<CreateTodoResult>;
    public record CreateTodoResult(string id);

    internal class CreateTodoHandler(ITodoRepository repository) : ICommandHandler<CreateTodoCommand, CreateTodoResult>
    {
        public async Task<CreateTodoResult> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = command.dto.Adapt<Models.Todo>();

            await repository.CreateTodoAsync(todo, cancellationToken);

            return new CreateTodoResult(todo.Id.ToString());
        }
    }
}