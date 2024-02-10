using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    [ForeignKey("CreatedByUser")]
    public string CreatedByUserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual User? CreatedByUser { get; set; }
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
