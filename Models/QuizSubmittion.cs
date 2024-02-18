using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class QuizSubmittion
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int QuizId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual User User { get; set; }
    public virtual Quiz Quiz { get; set; }
    
}
