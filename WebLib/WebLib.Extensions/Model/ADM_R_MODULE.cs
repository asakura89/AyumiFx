using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_MODULE : AuditTrailData
    {
        private String _id_module;
        private String _module_name;
        private String _module_parent;
        private String _description;
        private String _link;
        private Int32 _priority;
        private String _link_target;
        private Int32 _icon;
        private Boolean _popup;

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

        public String module_parent
        {
            get { return _module_parent; }
            set { _module_parent = value; }
        }

        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String link
        {
            get { return _link; }
            set { _link = value; }
        }

        public Int32 priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public String link_target
        {
            get { return _link_target; }
            set { _link_target = value; }
        }

        public Int32 icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public Boolean popup
        {
            get { return _popup; }
            set { _popup = value; }
        }

        public ADM_R_MODULE()
        {
            _id_module = String.Empty;
            _module_name = String.Empty;
            _module_parent = String.Empty;
            _description = String.Empty;
            _link = String.Empty;
            _priority = 0;
            _link_target = String.Empty;
            _icon = 0;
            _popup = false;
        }
    } 
}
