using MediatR;
using Shared.CQRS;

namespace Todo.Features.GetTodo
{
    public record GetTodoCommand(string Name, string IsComplete) : ICommand<GetTodoResult>;
    public record GetTodoResult(Guid id);

    internal class GetTodoHandler(ISender sender) : ICommandHandler<GetTodoCommand, GetTodoResult>
    {
        public Task<GetTodoResult> Handle(GetTodoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}