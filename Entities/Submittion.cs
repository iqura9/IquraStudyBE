using System;
using System.Collections.Generic;

namespace IquraStudyBE.Entities;

public partial class Submittion
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProblemId { get; set; }

    public string? Code { get; set; }

    /// <summary>
    /// Accepted, Wrong Answer
    /// </summary>
    public string? Result { get; set; }

    public int? ResultPercent { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public virtual Problem? Problem { get; set; }

    public virtual User? User { get; set; }
}
