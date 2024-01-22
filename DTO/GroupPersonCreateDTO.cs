using IquraStudyBE.Models;

namespace IquraStudyBE.Classes;

public class GroupPersonCreateDTO
{
    public int GroupId;
}

public class PutGroupPersonCreateDTO
{
    public int GroupId;
    public string UserId;
    public UserStatus UserStatus;
}
