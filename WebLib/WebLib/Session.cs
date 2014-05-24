using System;
using WebLib.Security;

namespace WebLib
{
   public class cSession
    {
       private string _sessUserID = string.Empty;
       private string _sessPassword = string.Empty;
       private string _SessionID = string.Empty;


       public string UserID
       {
           set { _sessUserID = value; }
           get { return _sessUserID; }
       }

       public string Password
       {
           set { _sessPassword = value; }
           get { return _sessPassword; }
       }

       public string SessionID
       {
           set { _SessionID = value; }
           get { return _SessionID; }
       }
       public cSession()
       {
           WebSecurity Enc = new WebSecurity();
            _sessUserID = "";
            _sessPassword = "";
            _SessionID = Enc.EncryptTripleDES( _sessUserID + _sessPassword + DateTime.Now.ToShortDateString(), true);
       }
    }
}
