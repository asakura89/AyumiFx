using System;

namespace WebLib.Extensions.Model
{
    public class ADM_ERROR_LOG
    {
        private string _id_error_log;
        private string _id_user;
        private string _id_module;
        private DateTime _error_date;
        private string _error_message;

        public string id_error_log
        {
            get { return _id_error_log; }
            set { _id_error_log = value; }
        }

        public string id_user
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        public string id_module
        {
            get { return _id_module; }
            set { _id_module = value; }
        }

        public DateTime error_date
        {
            get { return _error_date; }
            set { _error_date = value; }
        }

        public string error_message
        {
            get { return _error_message; }
            set { _error_message = value; }
        }

        public ADM_ERROR_LOG(string pIdErrorLog, string pIdUser, string pIdModule, DateTime pErrorDate, string pErrorMsg)
        {
            this.id_error_log = pIdErrorLog;
            this.id_user = pIdUser;
            this.id_module = pIdModule;
            this.error_date = pErrorDate;
            this.error_message = pErrorMsg;
        }

        public ADM_ERROR_LOG()
            : this(string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty) { }

    }
}
