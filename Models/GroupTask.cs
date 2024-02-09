using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class GroupTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? GroupId { get; set; }
    [ForeignKey("CreatedByUser")]
    public string? CreateByUserId { get; set; }
    public virtual Group? Group { get; set; }
    public virtual User? CreatedByUser { get; set; }
    public virtual ICollection<GroupTaskProblem> GroupTaskProblems { get; } = new List<GroupTaskProblem>();
    public virtual ICollection<GroupTaskQuiz> GroupTaskQuizzes { get; } = new List<GroupTaskQuiz>();

}
