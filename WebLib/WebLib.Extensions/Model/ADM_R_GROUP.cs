using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUP : AuditTrailData
    {
        private String _id_group;
        private String _group_code;
        private String _group_name;
        private Boolean _all_area;
        private Boolean _all_region;
        private Boolean _all_branch;
        private String _description;
        private String _default_link;

        public String id_group
        {
            get { return _id_group; }
            set { _id_group = value; }
        }

        [Column("Group Code")]
        public String group_code
        {
            get { return _group_code; }
            set { _group_code = value; }
        }

        [Column("Group Name")]
        public String group_name
        {
            get { return _group_name; }
            set { _group_name = value; }
        }

        [Column("All Area")]
        public Boolean all_area
        {
            get { return _all_area; }
            set { _all_area = value; }
        }

        [Column("By Region")]
        public Boolean all_region
        {
            get { return _all_region; }
            set { _all_region = value; }
        }

        [Column("By Branch")]
        public Boolean all_branch
        {
            get { return _all_branch; }
            set { _all_branch = value; }
        }

        [Column("Description")]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String default_link
        {
            get { return _default_link; }
            set { _default_link = value; }
        }

        public ADM_R_GROUP(String idGroup)
        {
            _id_group = idGroup;
            _group_code = String.Empty;
            _group_name = String.Empty;
            _all_area = false;
            _all_region = false;
            _all_branch = false;
            _description = String.Empty;
            _default_link = String.Empty;
        }

        public ADM_R_GROUP() : this(String.Empty) { }
    } 
}
