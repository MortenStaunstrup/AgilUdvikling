using API.Models;

namespace API.Repositories;

public interface IUserRepository
{
    public User? Login(string username, string password);
    public User? Register(string username, string password, string email);
}