﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }
    [ForeignKey("User")]
    public string CreatedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsArchived { get; set; }
    [ForeignKey("User")]
    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();

    public virtual ICollection<GroupTask> GroupTasks { get; } = new List<GroupTask>();
}
