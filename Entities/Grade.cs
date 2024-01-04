using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Grade
{
    public int Id { get; set; }

    public int? ProblemId { get; set; }

    public int? UserId { get; set; }

    public int? Grade1 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Problem? Problem { get; set; }

    public virtual User? User { get; set; }
}
