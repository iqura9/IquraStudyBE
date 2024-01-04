using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class SimilarProblem
{
    public int Id { get; set; }

    public int? ProblemId { get; set; }

    public int? RelatedProblemId { get; set; }

    public virtual Problem? Problem { get; set; }

    public virtual Problem? RelatedProblem { get; set; }
}
