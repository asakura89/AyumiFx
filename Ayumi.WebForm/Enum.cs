namespace WebLib
{

    public class Enum
    {
        public enum EnumStatusDominate
        {
            ANDOR = 1,
            DOMINATE = 2
        }
        public enum EnumCardType
        {
            KTP,
            SIM,
            PASSPORT,
            Lainnya
        }
        public enum EnumPaymentType
        {
            Cash,
            NonCash,
            AutoDebet
        }

        public enum EnumSessionRule
        {
            Admin,
            AdminCabang,
            User,
            Supervisor
        }
        public class EnumSessionProfile
        {
            private static readonly string _currentniksession = "currentuserIDsession";
            
           
        }

    }
}
