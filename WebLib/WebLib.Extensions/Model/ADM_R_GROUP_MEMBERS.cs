using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUP_MEMBERS : AuditTrailData
    {
        private String _id_group_members;
        private String _id_group;
        private String _id_user;
        private String _description;

        [Column("Group Members")]
        public String id_group_members
        {
            get { return _id_group_members; }
            set { _id_group_members = value; }
        }

        [Column("Group Id")]
        public String id_group
        {
            get { return _id_group; }
            set { _id_group = value; }
        }

        [Column("User Id")]
        public String id_user
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        [Column("Description")]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ADM_R_GROUP_MEMBERS(String idGroupMembers)
        {
            _id_group_members = idGroupMembers;
            _id_group = String.Empty;
            _id_user = String.Empty;
            _description = String.Empty;
        }

        public ADM_R_GROUP_MEMBERS() : this(String.Empty) { }
    } 
}
