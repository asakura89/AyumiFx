using System;

namespace WebLib.Data
{
    [Serializable]
    public class AuditTrailedObject : IAuditTrail
    {
        public AuditTrailedObject()
        {
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
            UpdatedBy = String.Empty;
            CreatedBy = String.Empty;
            LastModule = String.Empty;
            LastOperation = String.Empty;
            IsDeleted = false;
            IsActive = true;
        }

        [Column("CREATED_DATE", "Created Date")]
        public DateTime CreatedOn { get; set; }

        [Column("LAST_UPDATE", "Last Update")]
        public DateTime UpdatedOn { get; set; }

        [Column("CREATED_BY", "Created By")]
        public String CreatedBy { get; set; }

        [Column("LAST_ACCESS", "Last Access")]
        public String UpdatedBy { get; set; }

        [Column("LAST_MODULE", "Last Module")]
        public String LastModule { get; set; }

        [Column("LAST_OPERATION", "Last Operation")]
        public String LastOperation { get; set; }

        [Column("DELETED", "Deleted")]
        public Boolean IsDeleted { get; set; }

        [Column("ACTIVE", "Active")]
        public Boolean IsActive { get; set; }
    }
}