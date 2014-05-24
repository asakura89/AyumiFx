using System;

namespace WebLib.Extensions.Model
{
    public class ADM_R_ERROR_LOG
    {
        private String _id_user;
        private String _id_module;
        private DateTime _error_date;
        private String _error_message;

        public String id_user
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        public String id_module
        {
            get { return _id_module; }
            set { _id_module = value; }
        }

        public DateTime error_date
        {
            get { return _error_date; }
            set { _error_date = value; }
        }

        public String error_message
        {
            get { return _error_message; }
            set { _error_message = value; }
        }

        public ADM_R_ERROR_LOG(string pIdUser, string pIdModule, DateTime pErrorDate, string pErrorMsg)
        {
            this.id_user = pIdUser;
            this.id_module = pIdModule;
            this.error_date = pErrorDate;
            this.error_message = pErrorMsg;
        }

        public ADM_R_ERROR_LOG()
            : this(string.Empty, string.Empty, DateTime.Now, string.Empty) { }

    }
}
