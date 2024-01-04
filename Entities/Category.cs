using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProblemRelatedCategory> ProblemRelatedCategories { get; } = new List<ProblemRelatedCategory>();
}
