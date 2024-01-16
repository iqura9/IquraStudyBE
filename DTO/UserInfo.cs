namespace IquraStudyBE.Classes;

public class UserInfo
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}