using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class QuizSubmittion
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int QuizId { get; set; }
    public int GroupTaskId { get; set; }
    public double Score { get; set; } = 0;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public virtual User User { get; set; }
    public virtual Quiz Quiz { get; set; }
    public virtual GroupTask GroupTask { get; set; }
    
}
