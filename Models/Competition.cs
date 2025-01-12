namespace IquraStudyBE.Models;

public class Competition
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty; // IOI, ICPC
    public string ParticipantMode { get; set; } = string.Empty; // Virtual, Online
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? Duration { get; set; } // Nullable, only required for Virtual mode
    public string Difficulty { get; set; } = string.Empty; // Easy, Medium, Hard
    
    public virtual ICollection<CompetitionProblem> CompetitionProblems { get; } = new List<CompetitionProblem>();

}