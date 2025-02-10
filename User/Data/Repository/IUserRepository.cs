namespace User.Data.Repository
{
    public interface IUserRepository
    {
        Task<Models.User> GetUserByIdAsync(Int64 id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Models.User>> GetUsersAsync(string name, CancellationToken cancellationToken = default);

        Task<Models.User> CreateUserAsync(Models.User user, CancellationToken cancellationToken = default);
    }
}