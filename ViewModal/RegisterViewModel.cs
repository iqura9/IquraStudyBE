using System.ComponentModel.DataAnnotations;

namespace IquraStudyBE.ViewModal;

public class RegisterViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("^(Student|Teacher)$", ErrorMessage = "Role must be either 'Student' or 'Teacher'")]
    public string Role { get; set; }
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}