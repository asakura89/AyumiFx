using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUP_ACCESS : AuditTrailData
    {
        private String _id_group_access;
        private String _id_group;
        private String _aid;
        private String _description;

        [Column("Group Access Id")]
        public String id_group_access
        {
            get { return _id_group_access; }
            set { _id_group_access = value; }
        }

        [Column("Group Id")]
        public String id_group
        {
            get { return _id_group; }
            set { _id_group = value; }
        }

        [Column("AID")]
        public String aid
        {
            get { return _aid; }
            set { _aid = value; }
        }

        [Column("Description")]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ADM_R_GROUP_ACCESS(String idGroupAccess)
        {
            _id_group_access = idGroupAccess;
            _id_group = String.Empty;
            _aid = String.Empty;
            _description = String.Empty;
        }

        public ADM_R_GROUP_ACCESS() : this(String.Empty) { }
    } 
}
