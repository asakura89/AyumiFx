using System;

namespace WebLib.Extensions.Model
{
    public class ADM_R_LOG_LOGIN
    {
        private String _log_id;
        private String _id_user;
        private String _id_session;
        private DateTime _login_time;
        private String _ip_address;
        private DateTime _logout_time;
        private String _extension;
        private String _description;

        public String log_id
        {
            get { return _log_id; }
            set { _log_id = value; }
        }

        public String id_user
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        public String id_session
        {
            get { return _id_session; }
            set { _id_session = value; }
        }

        public DateTime login_time
        {
            get { return _login_time; }
            set { _login_time = value; }
        }

        public String ip_address
        {
            get { return _ip_address; }
            set { _ip_address = value; }
        }

        public DateTime logout_time
        {
            get { return _logout_time; }
            set { _logout_time = value; }
        }

        public String extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ADM_R_LOG_LOGIN()
        {
            _log_id = String.Empty;
            _id_user = String.Empty;
            _id_session = String.Empty;
            _login_time = CommonFunction.GetMinSQLDatetime();
            _ip_address = String.Empty;
            _logout_time = CommonFunction.GetMinSQLDatetime();
            _extension = String.Empty;
            _description = String.Empty;
        }
    }
}