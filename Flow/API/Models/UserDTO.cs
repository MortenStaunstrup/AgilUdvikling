using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class UserDTO
{ 
    [Required] 
    public string Name { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required]
    public string Email { get; set; } = "";
}