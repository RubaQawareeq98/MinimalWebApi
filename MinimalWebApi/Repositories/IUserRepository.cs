using MinimalWebApi.Models;

namespace MinimalWebApi.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username, string password);
}
