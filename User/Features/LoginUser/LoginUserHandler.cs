using Shared.CQRS;
using User.Data.Repository;
using User.Infrastructure.Authentication;

namespace User.Features.LoginUser
{
    public record LoginResult(string token);
    public record LoginCommand(string Email, string password) : ICommand<LoginResult>;

    internal class LoginUserHandler(IUserRepository repository, IJwtProvider jwtProvider) : ICommandHandler<LoginCommand, LoginResult>
    {
        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            Models.User? user = await repository.GetUserByEmailAndPassword(command.Email, command.Email, cancellationToken);

            if (user is null)
                throw new ArgumentNullException("User not found");

            return new LoginResult(jwtProvider.Create(user));
        }
    }
}