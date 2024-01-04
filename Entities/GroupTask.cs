using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class GroupTask
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? GroupId { get; set; }

    public int? CreateByUserId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<GroupTaskProblem> GroupTaskProblems { get; } = new List<GroupTaskProblem>();
}
