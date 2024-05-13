using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class ProblemSubmittion
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ProblemId { get; set; }
    public string SourceCode { get; set; }
    public int GroupTaskId { get; set; }
    public double Score { get; set; } = 0;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual User User { get; set; }
    public virtual Problem Problem { get; set; }
    public virtual GroupTask GroupTask { get; set; }
    
}
