using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int QuizId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual Quiz? Quiz { get; set; }
    public virtual ICollection<Answer> Answers { get; } = new List<Answer>();
}
