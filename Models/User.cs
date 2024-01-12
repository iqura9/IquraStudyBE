using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IquraStudyBE.Models;

public partial class User : IdentityUser
{
    public string? Image { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();
    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();
    public virtual ICollection<Group> Groups { get; } = new List<Group>();
    public virtual ICollection<Problem> Problems { get; } = new List<Problem>();
    public virtual ICollection<Submittion> Submittions { get; } = new List<Submittion>();
}
