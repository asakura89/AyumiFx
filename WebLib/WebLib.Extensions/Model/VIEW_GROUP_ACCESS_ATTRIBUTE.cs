using System;

namespace WebLib.Extensions.Model
{
    public class VIEW_GROUP_ACCESS_ATTRIBUTE
    {
        private String _id_group_access;
        private String _id_group;
        private String _group_name;
        private String _id_module;
        private String _module_name;
        private String _description;
        private Boolean _active;

        public String id_group_access
        {
            get { return _id_group_access; }
            set { _id_group_access = value; }
        }

        public String id_group
        {
            get { return _id_group; }
            set { _id_group = value; }
        }

        public String group_name
        {
            get { return _group_name; }
            set { _group_name = value; }
        }

        public String id_module
        {
            get { return _id_module; }
            set { _id_module = value; }
        }

        public String module_name
        {
            get { return _module_name; }
            set { _module_name = value; }
        }

        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Boolean active
        {
            get { return _active; }
            set { _active = value; }
        }

        public VIEW_GROUP_ACCESS_ATTRIBUTE()
        {
            _id_group_access = String.Empty;
            _id_group = String.Empty;
            _group_name = String.Empty;
            _id_module = String.Empty;
            _module_name = String.Empty;
            _description = String.Empty;
            _active = false;
        }
    }
}