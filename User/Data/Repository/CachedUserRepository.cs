using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace User.Data.Repository
{
    public class CachedUserRepository(IUserRepository userRepository, IDistributedCache cache) : IUserRepository
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<Models.User> CreateUserAsync(Models.User user, CancellationToken cancellationToken = default)
        {
            await userRepository.CreateUserAsync(user, cancellationToken);

            await cache.SetStringAsync($"user-{user.Id}", JsonSerializer.Serialize(user, jsonSerializerOptions), cancellationToken);

            return user;
        }

        public Task<Models.User> GetUserByEmailAndPassword(string email, string password, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Models.User> GetUserByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var cachedUser = await cache.GetStringAsync($"user-{id}", cancellationToken);

                if (cachedUser is not null && !string.IsNullOrEmpty(cachedUser))
                    return JsonSerializer.Deserialize<Models.User>(cachedUser, jsonSerializerOptions)!;

                var user = await userRepository.GetUserByIdAsync(id, cancellationToken);

                if (user is null)
                    throw new NullReferenceException();

                await cache.SetStringAsync($"user-{id}", JsonSerializer.Serialize(user, jsonSerializerOptions), cancellationToken);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<Models.User>> GetUsersAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}