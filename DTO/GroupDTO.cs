using IquraStudyBE.Models;

namespace IquraStudyBE.Classes;

public class CreateGroupDTO
{
    public string Name { get; set; }
}

public class CreateGroupTaskDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int GroupId { get; set; }
}


public class GroupTaskQuizzesDto
{

    public int GroupTaskId { get; set; }
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public double? Score { get; set; }
}
public class GetGroupTaskQuiz
{
    public int Id { get; set; }
    public User CreatedByUser { get; set; }
    public string CreateByUserId { get; set; }
    public string? Description { get; set; }
    public int? GroupId { get; set; }
    public List<GroupTaskQuizzesDto> GroupTaskQuizzes { get; set; }
    public string Title { get; set; }
    public double? AverageScore { get; set; }
    public DateTime? CreatedAt { get; set; }
}
