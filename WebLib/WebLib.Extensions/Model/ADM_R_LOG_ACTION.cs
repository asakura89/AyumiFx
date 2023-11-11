using System;

namespace WebLib.Extensions.Model
{
    public class ADM_R_LOG_ACTION
    {
        private String _id_user;
        private String _action;
        private String _action_desc;
        private DateTime _action_time;
        private String _ip_address;
        private String _session_id;

        public String id_user
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        public String action
        {
            get { return _action; }
            set { _action = value; }
        }

        public String action_desc
        {
            get { return _action_desc; }
            set { _action_desc = value; }
        }

        public DateTime action_time
        {
            get { return _action_time; }
            set { _action_time = value; }
        }

        public String ip_address
        {
            get { return _ip_address; }
            set { _ip_address = value; }
        }

        public String session_id
        {
            get { return _session_id; }
            set { _session_id = value; }
        }
    }
}
