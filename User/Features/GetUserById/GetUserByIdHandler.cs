using Mapster;
using Shared.CQRS;
using User.Data.Repository;
using User.Dtos;

namespace User.Features.GetUserById
{
    public record GetUserByIdResult(UserDto userDto);
    public record GetUserByIdQuery(Int64 UserId) : IQuery<GetUserByIdResult>;

    internal class GetUserByIdHandler(IUserRepository repository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await repository.GetUserByIdAsync(request.UserId, cancellationToken);

            return new GetUserByIdResult(user.Adapt<UserDto>());
        }
    }
}