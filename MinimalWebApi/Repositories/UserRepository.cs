using MinimalWebApi.Models;

namespace MinimalWebApi.Repositories;

public class UserRepository : IUserRepository
{
    private  readonly List<User> _users = [
        new(){
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
            Username = "john12",
            Password = "123456"
        }
        ];
    

    public Task<User?> GetUserAsync(string username, string password)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Username == username && u.Password == password));
    }
}
