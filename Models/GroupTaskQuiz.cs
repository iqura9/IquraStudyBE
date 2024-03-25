using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class GroupTaskQuiz
{
    public int Id { get; set; }

    public int GroupTaskId { get; set; }

    public int QuizId { get; set; }

    public virtual GroupTask? GroupTask { get; set; }

    public virtual Quiz? Quiz { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
