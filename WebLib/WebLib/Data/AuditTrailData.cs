using System;

namespace WebLib.Data
{
    [Serializable]
    public class AuditTrailData
    {
        private DateTime _created_date;
        private DateTime _last_update;
        private string _created_by;
        private string _last_access;
        private string _last_operation;
        private string _last_modules;
        private bool _deleted;
        private bool _active;

        [Column("Created Date")]
        public DateTime created_date
        {
            get
            {
                return _created_date;
            }
            set
            {
                _created_date = value;
            }
        }

        [Column("Last Update")]
        public DateTime last_update
        {
            get
            {
                return _last_update;
            }
            set
            {
                _last_update = value;
            }
        }

        [Column("Created By")]
        public string created_by
        {
            get
            {
                return _created_by;
            }
            set
            {
                _created_by = value;
            }
        }

        [Column("Last Access")]
        public string last_access
        {
            get
            {
                return _last_access;
            }
            set
            {
                _last_access = value;
            }
        }

        [Column("Last Module")]
        public string last_modules
        {
            get
            {
                return _last_modules;
            }
            set
            {
                _last_modules = value;
            }
        }

        [Column("Last Operation")]
        public string last_operation
        {
            get
            {
                return _last_operation;
            }
            set
            {
                _last_operation = value;
            }
        }

        [Column("Deleted")]
        public bool deleted
        {
            get
            {
                return _deleted;
            }
            set
            {
                _deleted = value;
            }
        }

        [Column("Active")]
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public AuditTrailData()
        {
            created_date = DateTime.Now;
            last_update = DateTime.Now;
            last_access= string.Empty;
            created_by = string.Empty;
            last_modules = string.Empty;
            last_operation = string.Empty;
            deleted = false;
            active = true;
        }
    }
}
