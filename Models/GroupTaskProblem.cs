using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class GroupTaskProblem
{
    public int Id { get; set; }

    public int? GroupTaskId { get; set; }

    public int? ProblemId { get; set; }

    public virtual GroupTask? GroupTask { get; set; }

    public virtual Problem? Problem { get; set; }
}
