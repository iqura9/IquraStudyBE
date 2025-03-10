namespace IquraStudyBE.Models;

public class CompetitionQuiz
{
    public int Id { get; set; }
    
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }

    public int QuizId { get; set; }
    public virtual Quiz Quiz { get; set; }
    public int Score { get; set; }
    public double? MaxScore { get; set; }
    public DateTime? SubmittedAt { get; set; }
}