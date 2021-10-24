using System;
using System.Configuration;

namespace WebApp.Config {
    public class Section : ConfigurationSection {
        [ConfigurationProperty("ConnectionStringName")]
        public String ConnectionStringName {
            get { return this["ConnectionStringName"].ToString(); }
            set { this["ConnectionStringName"] = value; }
        }

        [ConfigurationProperty("Tenant")]
        public String Tenant {
            get { return this["Tenant"].ToString(); }
            set { this["Tenant"] = value; }
        }

        [ConfigurationProperty("SecuritySalt")]
        public String SecuritySalt {
            get { return this["SecuritySalt"].ToString(); }
            set { this["SecuritySalt"] = value; }
        }

        [ConfigurationProperty("NewUserInitialPassword")]
        public String NewUserInitialPassword {
            get { return this["NewUserInitialPassword"].ToString(); }
            set { this["NewUserInitialPassword"] = value; }
        }

        [ConfigurationProperty("AdminGroupName")]
        public String AdminGroupName {
            get { return this["AdminGroupName"].ToString(); }
            set { this["AdminGroupName"] = value; }
        }

        [ConfigurationProperty("AppFullName")]
        public String AppFullName {
            get { return this["AppFullName"].ToString(); }
            set { this["AppFullName"] = value; }
        }

        [ConfigurationProperty("AppFirstName")]
        public String AppFirstName {
            get { return this["AppFirstName"].ToString(); }
            set { this["AppFirstName"] = value; }
        }

        [ConfigurationProperty("AppLastName")]
        public String AppLastName {
            get { return this["AppLastName"].ToString(); }
            set { this["AppLastName"] = value; }
        }

        [ConfigurationProperty("AppShortFirstName")]
        public String AppShortFirstName {
            get { return this["AppShortFirstName"].ToString(); }
            set { this["AppShortFirstName"] = value; }
        }

        [ConfigurationProperty("AppShortLastName")]
        public String AppShortLastName {
            get { return this["AppShortLastName"].ToString(); }
            set { this["AppShortLastName"] = value; }
        }

        [ConfigurationProperty("Company")]
        public String Company {
            get { return this["Company"].ToString(); }
            set { this["Company"] = value; }
        }

        [ConfigurationProperty("CompanyWeb")]
        public String CompanyWeb {
            get { return this["CompanyWeb"].ToString(); }
            set { this["CompanyWeb"] = value; }
        }

        [ConfigurationProperty("ProjectStarted")]
        public String ProjectStarted {
            get { return this["ProjectStarted"].ToString(); }
            set { this["ProjectStarted"] = value; }
        }
    }
}