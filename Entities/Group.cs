using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsArchived { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();

    public virtual ICollection<GroupTask> GroupTasks { get; } = new List<GroupTask>();
}
