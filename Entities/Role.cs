using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Role
{
    public int Id { get; set; }

    /// <summary>
    /// Teacher or Student
    /// </summary>
    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
