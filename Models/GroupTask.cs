﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class GroupTask
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? GroupId { get; set; }
    [ForeignKey("User")]
    public string? CreateByUserId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<GroupTaskProblem> GroupTaskProblems { get; } = new List<GroupTaskProblem>();
}
