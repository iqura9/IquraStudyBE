using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class TestCase
{
    public int Id { get; set; }

    public int? ProblemId { get; set; }

    /// <summary>
    /// file
    /// </summary>
    public string? Input { get; set; }

    /// <summary>
    /// file
    /// </summary>
    public string? ExpectedResult { get; set; }

    public virtual Problem? Problem { get; set; }
}
