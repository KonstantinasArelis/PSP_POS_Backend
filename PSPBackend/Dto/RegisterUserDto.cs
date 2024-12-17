using System.ComponentModel.DataAnnotations;
using PSPBackend.Model;

namespace PSPBackend.Dto;

public class RegisterUserDto
{
    [Required]
    [MaxLength(255)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public int BusinessId { get; set; }

    [Required]
    public string UserRole { get; set; }
}
