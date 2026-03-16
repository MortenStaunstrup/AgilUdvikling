using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories;
namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    // 1 = Malformed UserDTO data
    // 2 = Username Already exists
    // -1 = Server error
    // 0 = Accepted

    [HttpPost]
    [Route("api/register")]
    public IActionResult RegisterUser(UserDTO user)
    {
        if (user == null)
            return BadRequest();
        
        int repoResult = _userRepository.Register(user);

        switch (repoResult)
        {
            case 0:
                Console.WriteLine("User Registered");
                return Created();
            case -1:
                return StatusCode(500);
            case 1:
                return BadRequest();
            case 2:
                return BadRequest();
            default:
                Console.WriteLine("Unexpected return from UserRepository in UserController api/register");
                return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("users/login")]
    public IActionResult Login(UserLogin loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.username) || string.IsNullOrEmpty(loginRequest.password))
            return BadRequest();
        
        User? userExist = _userRepository.Login(loginRequest.username, loginRequest.password);
        if (userExist == null)
            return Unauthorized();
        return Ok(userExist);
    }
}

public record UserLogin(string username, string password);