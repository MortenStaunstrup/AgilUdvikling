using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories;
using API.Repositories;
namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController
{
    
    private readonly IUserRepository _userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost(Name = "RegisterUser")]
    public  User RegisterUser(User user)
    {
        throw new InvalidOperationException();
    }

    [HttpPost]
    [Route("users/login")]
    public IResult Login(UserLogin loginRequest)
    {
        User? userExist = _userRepository.Login(loginRequest.username, loginRequest.password);
        if (userExist == null)
            return Results.Unauthorized();
        return Results.Ok(userExist);
    }
}

public record UserLogin(string username, string password);