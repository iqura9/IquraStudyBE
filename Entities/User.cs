using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Problem> Problems { get; } = new List<Problem>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Submittion> Submittions { get; } = new List<Submittion>();
}
