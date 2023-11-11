using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_MODULE_ATTRIBUT : AuditTrailData
    {
        private String _aid;
        private String _id_module;
        private String _description;
        private Int32 _priority;
        private String _def_value;
        private String _def_value2;

        public String aid
        {
            get { return _aid; }
            set { _aid = value; }
        }

        public String id_module
        {
            get { return _id_module; }
            set { _id_module = value; }
        }

        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Int32 priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public String def_value
        {
            get { return _def_value; }
            set { _def_value = value; }
        }

        public String def_value2
        {
            get { return _def_value2; }
            set { _def_value2 = value; }
        }

        public ADM_R_MODULE_ATTRIBUT()
        {
            _aid = String.Empty;
            _id_module = String.Empty;
            _description = String.Empty;
            _priority = 0;
            _def_value = String.Empty;
            _def_value2 = String.Empty;
        }
    } 
}
