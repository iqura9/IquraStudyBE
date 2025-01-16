using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraStudyBE.Models;

public partial class GroupCompetition
{
    public int Id { get; set; }
    public int? GroupId { get; set; }
    [ForeignKey("CreatedByUser")]
    public string? CreateByUserId { get; set; }
    public int? CompetitionId { get; set; }
    
    public virtual Group? Group { get; set; }
    public virtual User? CreatedByUser { get; set; }
    public virtual Competition? Competition { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
