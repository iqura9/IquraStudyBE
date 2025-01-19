using System.ComponentModel.DataAnnotations;

namespace IquraStudyBE.Models;

public class Participation
{
    public int Id { get; set; }
    public int CompetitionId { get; set; }
    public int GroupId { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    [Range(0, double.MaxValue)]
    public double Score { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Active"; // Default to "Active"
    
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public virtual Competition Competition { get; set; }
    public virtual Group Group { get; set; }
    
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
