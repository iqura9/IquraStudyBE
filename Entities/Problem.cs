using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Problem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<GroupTaskProblem> GroupTaskProblems { get; } = new List<GroupTaskProblem>();

    public virtual ICollection<ProblemRelatedCategory> ProblemRelatedCategories { get; } = new List<ProblemRelatedCategory>();

    public virtual ICollection<SimilarProblem> SimilarProblemProblems { get; } = new List<SimilarProblem>();

    public virtual ICollection<SimilarProblem> SimilarProblemRelatedProblems { get; } = new List<SimilarProblem>();

    public virtual ICollection<Submittion> Submittions { get; } = new List<Submittion>();

    public virtual ICollection<TestCase> TestCases { get; } = new List<TestCase>();

    public virtual User? User { get; set; }
}
