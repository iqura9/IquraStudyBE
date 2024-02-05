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
