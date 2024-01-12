using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class ProblemRelatedCategory
{
    public int Id { get; set; }

    public int? ProblemId { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Problem? Problem { get; set; }
}
