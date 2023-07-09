using System.ComponentModel.DataAnnotations;

namespace Pakosti.Api.Models.Identity;

public class RegisterRequest
{
    [Required] [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required] [Display(Name = "Birth date")]
    public DateTime BirthDate { get; set; }

    [Required] [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords not equal")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string PasswordConfirm { get; set; } = null!;

    [Required] [Display(Name = "Name")]
    public string FirstName { get; set; } = null!;

    [Required] [Display(Name = "Surname")]
    public string LastName { get; set; } = null!;
}