using System;

namespace WebLib.Extensions.Model.Object
{
    public interface IUser
    {
        String UserId { get; set; }
        String BranchId { get; set; }
        String RegionId { get; set; }
        String PositionId { get; set; }
        String NIK { get; set; }
        String Username { get; set; }
        String Email { get; set; }
        Int32 Counter { get; set; }
    }
}