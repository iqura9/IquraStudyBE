using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int? ProblemId { get; set; }

    public string? UserId { get; set; }

    public int? GradeScore { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Problem? Problem { get; set; }

    public virtual User? User { get; set; }
}
