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
    

    [HttpPost]
    [Route("api/register")]
    public IActionResult RegisterUser(UserDTO user)
    {
        if (user == null)
            return BadRequest();

        try
        {
            int repoResult = _userRepository.Register(user);

            switch (repoResult)
            {
                case 0:
                    Console.WriteLine("User Registered");
                    return Created();
                case 1:
                    return BadRequest();
                case 2:
                    return BadRequest();
                default:
                    Console.WriteLine("Unexpected return from UserRepository in UserController api/register");
                    return StatusCode(500);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception thrown in UserRepository in UserController api/register");
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("users/login")]
    public IActionResult Login(UserLogin loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.username) || string.IsNullOrWhiteSpace(loginRequest.password))
            return BadRequest();

        try
        {
            User? userExist = _userRepository.Login(loginRequest.username, loginRequest.password);
            if (userExist == null)
                return Unauthorized();
            return Ok(userExist);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
}

public record UserLogin(string username, string password);