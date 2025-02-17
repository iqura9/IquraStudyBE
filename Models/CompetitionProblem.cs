namespace IquraStudyBE.Models;

public class CompetitionProblem
{
    public int Id { get; set; }
    
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }

    public int ProblemId { get; set; }
    public virtual Problem Problem { get; set; }
    public int Score { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public double MaxScore { get; set; }
}