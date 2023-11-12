using System;

namespace WebLib.Extensions.Model.Object
{
    public interface IGroupMember
    {
        String GroupMemberId { get; set; }
        String GroupId { get; set; }
        String UserId { get; set; }
        String Description { get; set; }
    }
}