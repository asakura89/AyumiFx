using System;

namespace WebLib.Data
{
    public interface IAuditTrail
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
        String CreatedBy { get; set; }
        String UpdatedBy { get; set; }
        String LastModule { get; set; }
        String LastOperation { get; set; }
        Boolean IsDeleted { get; set; }
        Boolean IsActive { get; set; }
    }
}