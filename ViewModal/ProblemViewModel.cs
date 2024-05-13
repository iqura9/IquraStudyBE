using IquraStudyBE.Models;

namespace IquraStudyBE.ViewModal;

public class ProblemViewModel
{
    public string? Title { get; set; }

    public string? Description { get; set; }
    public string? InitFunc { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public List<TestCase> TestCases { get; set; } = new List<TestCase>();
}


public class PostProblemSubmittionDTO
{
    public string SourceCode { get; set; }
    public double Score { get; set; }
    public int GroupTaskId { get; set; }
    public int ProblemId { get; set; }
    
}
