using System;
using Ayumi.Data;
using Ayumi.Data.Db;

namespace Ayumi.Auditor {
    public abstract class AuditTrailedByAyumi : IAuditTrailed {
        [Column("CreatedBy", "Created By")]
        public String CreatedBy { get; set; }

        [Column("CreatedDate", "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedBy", "Updated By")]
        public String UpdatedBy { get; set; }

        [Column("UpdatedDate", "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Column("AccessedBy", "Accessed By")]
        public String AccessedBy { get; set; }

        [Column("Operation", "Operation")]
        public String Operation { get; set; }

        [Column("Module", "Module")]
        public String Module { get; set; }

        [Column("Archived", "Archived")]
        public Boolean Archived { get; set; }

        [Column("Deleted", "Deleted")]
        public Boolean Deleted { get; set; }
    }
}