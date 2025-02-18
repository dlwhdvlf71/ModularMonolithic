using Mapster;
using MediatR;
using Shared.CQRS;
using Shared.Messaging.Events;
using User.Data.Repository;
using User.Dtos;

namespace User.Features.CreateUser
{
    public record CreateUserResult(string id);
    public record CreateUserCommand(UserDto userDto) : ICommand<CreateUserResult>;

    internal class CreateUserHandler(IUserRepository repository, IMediator mediator) : ICommandHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = command.userDto.Adapt<Models.User>();
            await repository.CreateUserAsync(user, cancellationToken);

            await mediator.Publish(user.Adapt<CreateUserEvent>(), cancellationToken);

            return new CreateUserResult(user.Id.ToString());
        }
    }
}