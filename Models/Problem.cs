using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class Problem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<GroupTaskProblem> GroupTaskProblems { get; } = new List<GroupTaskProblem>();

    public virtual ICollection<ProblemRelatedCategory> ProblemRelatedCategories { get; } = new List<ProblemRelatedCategory>();

    public virtual ICollection<Submittion> Submittions { get; } = new List<Submittion>();

    public virtual ICollection<TestCase> TestCases { get; } = new List<TestCase>();
    
    
}
