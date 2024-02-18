namespace IquraStudyBE.Classes;

public class CreateQuizDto
{
    public string Title { get; set; }
}

public class CreateQuizTaskDto
{
    
    public int GroupTasksId { get; set; }
    public int[] QuizIds { get; set; }
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

public class CreateQuestionWithAnswersDto
{
    public string Title { get; set; }
    public List<CreateAnswerDto> Answers { get; set; }
}


public class UpdateQuestionWithAnswersDto
{
    public string Title { get; set; }
    public List<UpdateAnswerDto> Answers { get; set; }
}

public class UpdateAnswerDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCorrect { get; set; } = false;
}


public class QuizVerificationRequest
{
    public int QuizId { get; set; }
    public int TaskId { get; set; }
    public List<QuestionAnswer> Questions { get; set; }
}

public class QuestionAnswer
{
    public int QuestionId { get; set; }
    public List<int> Answers { get; set; }
}

public class QuizVerificationResponse
{
    public double Result { get; set; }
}