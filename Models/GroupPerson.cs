using System;
using System.Collections.Generic;

namespace IquraStudyBE.Models;

public partial class GroupPerson
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int? GroupId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual User? User { get; set; }
}
