namespace IquraStudyBE.Models;

public enum SubmissionType
{
    Quiz = 1,
    Problem = 2
}

public class Submission
{
    public int Id { get; set; }
    
    public int ParticipationId { get; set; }
    public virtual Participation Participation { get; set; }
    
    public SubmissionType Type { get; set; } // Quiz or Problem
    
    public int? QuizId { get; set; }
    public virtual Quiz Quiz { get; set; }
    
    public int? ProblemId { get; set; }
    public virtual Problem Problem { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public double Score { get; set; } 
}