using System.ComponentModel.DataAnnotations;
namespace IquraStudyBE.ViewModal;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool? RememberMe { get; set; } = false;
}