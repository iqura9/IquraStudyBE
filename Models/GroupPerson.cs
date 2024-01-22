using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IquraStudyBE.Models;
public enum UserStatus
{
    Success,
    Pending,
    Declined,
}
public partial class GroupPerson
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int? GroupId { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public UserStatus UserStatus { get; set; }

    public virtual Group? Group { get; set; }

    public virtual User? User { get; set; }
}
