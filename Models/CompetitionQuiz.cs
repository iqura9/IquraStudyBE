namespace IquraStudyBE.Models;

public class CompetitionQuiz
{
    public int Id { get; set; }
    
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }

    public int QuizId { get; set; }
    public virtual Quiz Quiz { get; set; }
}