using System;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_USER : AuditTrailData
    {
        private String _user_id;
        private String _id_branch;
        private String _id_region;
        private String _id_position;
        private String _nik;
        private String _username;
        private String _email;
        private Int32 _counter;

        public String user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }

        [Column("Branch Id")]
        public String id_branch
        {
            get { return _id_branch; }
            set { _id_branch = value; }
        }

        [Column("Region Id")]
        public String id_region
        {
            get { return _id_region; }
            set { _id_region = value; }
        }

        [Column("Position Id")]
        public String id_position
        {
            get { return _id_position; }
            set { _id_position = value; }
        }

        [Column("NIK")]
        public String nik
        {
            get { return _nik; }
            set { _nik = value; }
        }

        [Column("Username")]
        public String username
        {
            get { return _username; }
            set { _username = value; }
        }

        [Column("Email")]
        public String email
        {
            get { return _email; }
            set { _email = value; }
        }

        public Int32 counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public ADM_R_USER()
        {
            _id_branch = String.Empty;
            _id_region = String.Empty;
            _id_position = String.Empty;
            _nik = String.Empty;
            _username = String.Empty;
            _email = String.Empty;
            _counter = 0;
        }

        public ADM_R_USER(String nik)
        {
            _id_branch = String.Empty;
            _id_region = String.Empty;
            _id_position = String.Empty;
            _nik = nik;
            _username = String.Empty;
            _email = String.Empty;
            _counter = 0;
        }
    }
}