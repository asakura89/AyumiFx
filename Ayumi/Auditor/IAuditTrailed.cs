using System;

namespace Ayumi.Auditor {
    public interface IAuditTrailed {
        String CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        String UpdatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        String AccessedBy { get; set; }
        String Operation { get; set; }
        String Module { get; set; }
        Boolean Archived { get; set; }
        Boolean Deleted { get; set; }
    }
}