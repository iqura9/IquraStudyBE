namespace IquraStudyBE.Classes;

public class CreateQuizDto
{
    public string Title { get; set; }
}

public class CreateQuestionDto
{
    public string Title { get; set; }
    public int QuizId { get; set; }
}

public class CreateAnswerDto
{
    public string Title { get; set; }
    public bool IsCorrect { get; set; } = false;
}