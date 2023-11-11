using System;
using System.DirectoryServices;
using System.Text;

namespace WebLib.Security
{
    public class LDAP
    {
        string _path = "";
        string _filterAttribute;

        /// <summary>
        /// Function to authenticated user
        /// </summary>
        /// <param name="domain">for domain</param>
        /// <param name="path">for path</param>
        /// <param name="username">for username to authenticated</param>
        /// <param name="pwd">for password of user to authenticated</param>
        /// <returns>bool</returns>
        public bool IsAuthenticated(string domain, string path, string username, string pwd)
        {
            string domainAndUsername = String.Format(@"{0}\{1}", domain, username);
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, pwd, AuthenticationTypes.Secure);
            Object obj = entry.NativeObject;
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(sAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (result == null)
                return false;

            //Update the new path to the user in the directory.
            _path = result.Path;
            _filterAttribute = Convert.ToString(result.Properties["cn"][0]);

            return true;
        }
        
        /// <summary>
        /// Function to retrieve gropus
        /// </summary>
        /// <returns>string</returns>
        public string GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn="+_filterAttribute+")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();
            try
            {
                SearchResult result=search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;

                string dn;
                int equalsIndex, commaIndex, propertyCounter;
                for (propertyCounter = 0; propertyCounter <= propertyCount - 1; propertyCounter++)
                {
                    dn = Convert.ToString(result.Properties["memberOf"][propertyCounter]);
                    equalsIndex = dn.IndexOf("=",1);
                    commaIndex = dn.IndexOf(",",1);
                    if (equalsIndex == -1)
                        return null;

                    groupNames.Append(dn.Substring(equalsIndex+1,(commaIndex-equalsIndex)-1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
                
            }
            return groupNames.ToString();
        }
    }
}
