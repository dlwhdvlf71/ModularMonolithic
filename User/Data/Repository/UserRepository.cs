using Microsoft.EntityFrameworkCore;

namespace User.Data.Repository
{
    public class UserRepository(UserDbContext dbContext) : IUserRepository
    {
        public async Task<Models.User> CreateUserAsync(Models.User user, CancellationToken cancellationToken = default)
        {
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<Models.User> GetUserByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var user = await dbContext.Users.FindAsync(id, cancellationToken);
            return user ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Models.User>> GetUsersAsync(string name, CancellationToken cancellationToken = default)
        {
            var items = await dbContext.Users.Where(user => user.FirstName.Contains(name) || user.LastName.Contains(name)).ToListAsync(cancellationToken);

            return items ?? throw new NullReferenceException();
        }

        public async Task<Models.User> GetUserByEmailAndPassword(string email, string password, CancellationToken cancellationToken = default)
        {
            //var user = await dbContext.Users.Where(x => x.Email.Equals(email) && x.Password.Equals(password)).SingleOrDefaultAsync(cancellationToken);
            var user = await dbContext.Users.Where(x => x.Email.Equals(email)).SingleOrDefaultAsync(cancellationToken);

            return user ?? throw new NullReferenceException();
        }
    }
}