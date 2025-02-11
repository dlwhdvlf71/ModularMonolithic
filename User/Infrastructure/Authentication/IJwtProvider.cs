namespace User.Infrastructure.Authentication
{
    public interface IJwtProvider
    {
        string Create(Models.User user);
    }
}