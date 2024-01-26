using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class Answer
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int QuestionId { get; set; }
    public bool IsCorrect { get; set; } = false;
    public virtual Question? Question { get; set; }
}
