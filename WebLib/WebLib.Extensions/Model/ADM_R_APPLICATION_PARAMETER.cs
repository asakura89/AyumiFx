using System;

namespace WebLib.Extensions.Model
{
    public class ADM_R_APPLICATION_PARAMETER
    {
        private String _id_param;
        private String _param_desc;

        public String id_param
        {
            get { return _id_param; }
            set { _id_param = value; }
        }

        public String param_desc
        {
            get { return _param_desc; }
            set { _param_desc = value; }
        }

        public ADM_R_APPLICATION_PARAMETER(String paramId)
        {
            _id_param = paramId;
            _param_desc = String.Empty;
        }

        public ADM_R_APPLICATION_PARAMETER() : this(String.Empty) { }
    }
}
