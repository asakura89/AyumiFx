using System;
using System.Data;
using WebLib.Data;
using WebLib.Security;
using WebLib.Security.Cryptography;

namespace WebLib
{
    public class Base : System.Web.UI.Page
    {
        #region DeclareBaseProperty
        private string _UserDB = string.Empty;
        private string _PassDB = string.Empty;
        private string _DBName = string.Empty;
        private string _DBServer = string.Empty;
        private string _Domain = string.Empty;
        private string _LDAPPath = string.Empty;
        private string _Url = string.Empty;
        private string _LoginType = string.Empty;
        
        private bool _isLogin;
        private bool _hasChangedPassword;
        public MSSQL mssql = new MSSQL(new MSSQL.DefaultMSSQLConfiguration());       
        public fMailer Mailer = new fMailer();
        public LDAP AD = new LDAP();
        //public WebSecurity security = new WebSecurity();
        public TripleDESEncryptor tripleDESEncryptor = new TripleDESEncryptor();
        public JavaScript JS = new JavaScript();
        // private cSession SessionHandling = new cSession();
        // private cMSSQLSession MSSQLSession = new cMSSQLSession();
        public MSSQLParameter cMSSQLParameter = new MSSQLParameter();        
        public cUserData UserData = new cUserData();
        // private cSecurity.IMAWA2G Enc = new cSecurity.IMAWA2G();
        public cNavigation Navigation = new cNavigation();
        //private Conversion cConvert = new Conversion();
        //private Reporting cReporting = new Reporting();
        //private AjaxControlToolkit.ToolkitScriptManager scm = new AjaxControlToolkit.ToolkitScriptManager();

        public Base()
        {
            //mssql.InitDatabase();
            //mssql.UpdateConnection();
        }
        public string UserDB
        {

            get { return String.Empty; } //mssql._UserID; }
        }
        public string PassDB
        {
            //get { return mssql._Password; }
            get { return String.Empty; }
        }
        public string DBName
        {
            //get { return mssql._Database; }
            get { return String.Empty; }
        }
        public string DBServer
        {
            //get { return mssql._Server; }
            get { return String.Empty; }
        }
        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }
        public string LDAPPath
        {
            get { return _LDAPPath; }
            set { _LDAPPath = value; }
        }
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }
        public string LoginType
        {
            get { return _LoginType; }
            set { _LoginType = value; }
        }
        public bool isLogin
        {
            get { return _isLogin; }
            set { _isLogin = value; }
        }
        public bool hasChangedPassword
        {
            get { return _hasChangedPassword; }
            set { _hasChangedPassword = value; }
        }
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            //mssql.Create();            
            //ProtectPage();
            Navigation.PagingSize = Convert.ToInt32(mssql.ExecuteDataScalar("SELECT ParamValue FROM SystemParameter WHERE ParamID='PagingSize'"));
            Session.Timeout = Convert.ToInt32(mssql.ExecuteDataScalar("SELECT ParamValue FROM SystemParameter WHERE ParamID='sessiontime'"));
            _Domain = System.Configuration.ConfigurationManager.AppSettings.Get("Domain");
            _LDAPPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAPPath");
            if (!IsPostBack)
            {
                Session["search"] = "";
            }
        }

        public DataSet GetPaging(string _TableOrViewOrSQL, string _Filter, string _Sort, int _ActivePage, int _NumOfPaging)
        {
            System.Data.SqlClient.SqlParameter[] SQLParam = new System.Data.SqlClient.SqlParameter[6];
            SQLParam[0] = new System.Data.SqlClient.SqlParameter("@SQLSyntax", SqlDbType.VarChar, 8000, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, _TableOrViewOrSQL);
            SQLParam[1] = new System.Data.SqlClient.SqlParameter("@CurrentPage", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, _ActivePage);
            SQLParam[2] = new System.Data.SqlClient.SqlParameter("@PageSize", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, _NumOfPaging);
            SQLParam[3] = new System.Data.SqlClient.SqlParameter("@FilterClause", SqlDbType.VarChar, 1000, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, _Filter);
            SQLParam[4] = new System.Data.SqlClient.SqlParameter("@OrderBy", SqlDbType.VarChar, 1000, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, _Sort);
            SQLParam[5] = new System.Data.SqlClient.SqlParameter("@SystemUser", SqlDbType.VarChar, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, "sa");
            DataSet ds = new DataSet();
            try
            {
                ds = mssql.ExecuteDataSet("dbo.USP_getPaging", CommandType.StoredProcedure, SQLParam);
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message);
                ds = null;
            }
            return ds;
        }

        #region "Database Tools"
        private string GetParameterValue(string _Name, string _Parameter)
        {
            int i;
            string _return = "";

            if (_Parameter.Split(';').GetUpperBound(0) > 0)
            {
                for (i = 0; i < _Parameter.Split(';').GetUpperBound(0); i++)
                {
                    if (_Parameter.Split(';')[i].Split('=').GetUpperBound(0) > 0 )
                    {
                        if ((_Parameter.Split(';')[i].Split('=')[0].ToString().Trim().ToLower() == _Name.ToLower().Trim()) || (_Parameter.Split(';')[i].Split('=')[0].ToString().Trim() == "@" + _Name.ToLower().Trim()))
                            _return = _Parameter.Split(';')[i].Split('=')[1].ToString();
                    }
                }
            }
            return _return;
        }
        public System.Data.SqlClient.SqlParameter[] ExecuteMSSQLSP(string _SPName, System.Data.SqlClient.SqlParameter[] MP, string Flag) 
        {
            string Status = Flag;
            System.Data.SqlClient.SqlParameter[] Params = null;
            System.Data.SqlClient.SqlParameter[] TempParams = new System.Data.SqlClient.SqlParameter[MP.GetUpperBound(0) + 1];
            int I, C=0;
            for (I = 0; I <= MP.GetUpperBound(0);I++ )
            {
                /*if ((MP[I].InInsert = true) && (Status == "insert"))
                {
                }*/
            }
            return Params;
        }
        private string GenerateStoredProcedure(string _TableName,string _SPName,System.Data.SqlClient.SqlParameter[] MP)
        {
            string SP = "CREATE PROCEDURE "+_SPName+"\r (\r@@Variable\r) AS \r BEGIN \r @@Script \r END \r GO";
            string variable = "";
            for (int i = 0; i <= MP.GetUpperBound(0); i++)
            {
                variable = "" + variable + "       " + MP[i].ParameterName + "  " + MP[i].SqlDbType.ToString() + "(" + MP[i].Size.ToString().Trim() + ") = Null ";
                if ((MP[i].Direction == ParameterDirection.InputOutput) || (MP[i].Direction == ParameterDirection.Output))
                {
                    variable = "" + variable + "OUTOUT";
                }
                variable= ""+variable+",\r";
            }
            variable = "" + variable + "       @Status  Varchar(30)=Null \r";
            SP = SP.Replace("@@Variable", variable);
            string ErrorTrap = "/*ErrorTrap*/ IF @@ERROR <> 0 BEGIN  RAISERROR('Proses @NamaProses gagal!', 16, -1)  RETURN  END";
            string Insert = "IF @Status = 'insert' \r Begin \r INSERT INTO [" + _TableName + "] (@@Variable) \r VALUES \r(@@Value) \r " + ErrorTrap.Replace("@NamaProses", "Insert") + " \r  End ";
            string Update = "IF @Status = 'update' \rBegin \r UPDATE [" + _TableName + "] \r SET \r@@Variable \r WHERE \r@@Condition \r " + ErrorTrap.Replace("@NamaProses", "Update") + " \r  End ";
            string Delete = "IF @Status = 'delete' \rBegin \r DELETE FROM [" + _TableName + "]\r WHERE \r@@Condition \r " + ErrorTrap.Replace("@NamaProses", "Delete") + " \r  End ";
            string VarInsert = "";
            string VarValues = "";
            string VarUpdate = "";
            for (int i = 0; i <= MP.GetUpperBound(0); i++)
            {
                VarInsert =""+VarInsert+" "+MP[i].ParameterName.Replace("@", "")+""; 
                VarValues =""+VarValues+" "+MP[i].ParameterName+"";
                VarUpdate=""+VarUpdate+" "+MP[i].ParameterName.Replace("@", "")+" = "+MP[i].ParameterName+"";
                if(i < MP.GetUpperBound(0))
                {
                    VarInsert =""+VarInsert+",\r";
                    VarValues =""+VarValues+",\r";
                    VarUpdate =""+VarUpdate+",\r";
                }
            }
            Insert = Insert.Replace("@@Variable", VarInsert).Replace("@@Value", VarValues);
            Update = Update.Replace("@@Variable", VarUpdate);
            SP = SP.Replace("@@Script",""+Insert+"\r"+Update+"\r"+Delete+"\r" );
            return SP;
        }
        public void DatabaseSPHandling(string _TableName, string _SPName, System.Data.SqlClient.SqlParameter[] SQLParam)
        {
            string buf;
            Page.Response.ClearContent();
            buf = "<HTML><BODY>" + GenerateStoredProcedure(_TableName, _SPName, SQLParam).Replace("\r", " <br> ") + "</BODY></HTML>";
            Response.Write("<HTML><BODY>" + GenerateStoredProcedure(_TableName, _SPName, SQLParam).Replace("\r", " <br> ") + "</BODY></HTML>");
            Page.Response.End();
        }
        #endregion

        #region "Dialog & Page Handing"
        public void ShowMessage(string Value)
        {
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "ShowMessage", "Javascript:{ alert('" + Value.Replace("@Message", Value.Replace("\r", "").Replace("\r", "").Replace("'", "")) + "');} ", true);
        }
       
        public void NavigateURL(string _Location, string _Target)
        {
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "_window", "Javascript:{ window.open('" + _Location + "','" + _Target + "');} ", true);
        }
        public void NavigateMultiURL(string _Name, string _Location, string _Target)
        {
            if (_Location.IndexOf("?") >= 0)
                ClientScript.RegisterClientScriptBlock(Page.GetType(), _Name, "<script>" + JS.ScriptWindowOpen.Value.Replace("@Location", _Location).Replace("@Target", _Target) + "</script>", true);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), _Name, "<script>" + JS.ScriptWindowOpen.Value.Replace("@Location", _Location + "?TimeStamp=" + DateTime.Now.ToShortDateString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()).Replace("@Target", _Target) + "</script>", true);
        }
        public void DownloadURLFile(string _Location, string _Target)
        {
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "_window", "", true);
         }
        public string WindowStatus()
        {
            string value = string.Empty;
            return value;
            //SetKey("winstatus", Value)
        }
        public void BlankPage(string Msg)
        {
            Page.Response.ClearContent();
            Response.Write("<HTML><BODY>" + Msg + "</BODY></HTML>");
            Page.Response.End();
        }
        public void ShowModalDialog(string _URL, string _InitValue, int _Height, int _Width, bool _EnableStatus)
        {
            if (_EnableStatus)
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "ModalDialog", "<script>window.showModalDialog('" + _URL + "','" + _InitValue + "','dialogHeight:" + _Height.ToString().Trim() + "px;dialogWidth:" + _Width.ToString().Trim() + "px;status=yes');</script>", true);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "ModalDialog", "<script>window.showModalDialog('" + _URL + "','" + _InitValue + "','dialogHeight:" + _Height.ToString().Trim() + "px;dialogWidth:" + _Width.ToString().Trim() + "px;status=no');</script>", true);
        }
        public string WebFormName()
        {
            //return Request.Path.Split('/')(Request.Path.Split('/').GetUpperBound(0));            
            return "";
        }
        #endregion

        #region "Login Handling"        
        public bool CheckUserAndLoginType(string username)
        {
            DataSet ds = new DataSet();
            bool result;
            try
            {
                ds = mssql.ExecuteDataSet("Select LoginType from users where userid = '" + username + "'", CommandType.Text);                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LoginType = ds.Tables[0].Rows[0][0].ToString();
                    result = true;
                }
                else
                    result = false;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                result = false;
            }
            return result;
        }
        public bool IsAuthenticatedLocal(string username, string pwd)
        {
            DataSet ds = new DataSet();
            string usrpwd;           
            try
            {
                usrpwd = mssql.ExecuteDataScalar("select Password from users where userid='" + username + "'").ToString();

                string test = tripleDESEncryptor.Encrypt("password");

                if (tripleDESEncryptor.Decrypt(usrpwd) == pwd)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                return false;
            }
        }
        public string CheckLogin(string UserName, string Password, string IP)
        {
            string ret="";
            if (CheckUserAndLoginType(UserName))
            {
                if (LoginType == "1")
                {
                    if (IsAuthenticatedLocal(UserName, Password))
                    {
                        if (UserName.ToLower() == "mossinst" || UserName.ToLower() == "admin")
                            isLogin = false;
                        else if (mssql.ExecuteDataScalar("SELECT isnull(SessionID,'') as SessionID FROM Users WHERE UserID='" + UserName + "'") != null)
                            isLogin = false;
                        else
                            isLogin = false;


                        if (mssql.ExecuteDataScalar("SELECT Active FROM Users WHERE UserID='" + UserName + "'").ToString().ToLower() != "true")
                        {
                            isLogin = true;
                            ShowMessage("User ID is not Active");
                        }
                        else
                            isLogin = false;


                        if (isLogin == true)
                        {
                            ret = "User already login or User Not Logout Correctly Or User is not Active\r ";
                            ret =""+ret+"\rPlease contact System Administrator to Activated User ID";
                            //ShowMessage("User already login");
                            Session["isLogin"] = "False";
                        }
                        else
                        {
                            DataSet dsBranch = new DataSet();
                            dsBranch = mssql.ExecuteDataSet("SELECT OfficeCode FROM Users WHERE UserID='" + UserName + "'", CommandType.Text);
                            if (dsBranch.Tables[0].Rows.Count > 0)
                            {
                                string officeCode = dsBranch.Tables[0].Rows[0][0].ToString();
                                Session["Branch"] = officeCode;
                            }
                            Session["isLogin"] = isLogin;
                            Session["UserID"] = UserName;
                            UserData.UserID = Session["UserID"].ToString();
                            Session["Password"] = Password;
                            mssql.ExecuteDataSet("UPDATE Users SET isLogin=1, SessionID='" + Session.SessionID + "' , LastLogin='" + DateTime.Now + "' WHERE UserID='" + UserData.UserID + "'", System.Data.CommandType.Text);
                            mssql.ExecuteDataSet("INSERT INTO LogLogin (SessionID, LoginTime, UserID, IPAddress) VALUES ('" + Session.SessionID + "'," + "'" + DateTime.Now + "', '" + UserData.UserID + "', '" + IP + "')", CommandType.Text);
                            if (Convert.ToBoolean(mssql.ExecuteDataScalar("SELECT hasChangedPassword FROM Users WHERE UserID='" + UserData.UserID + "'")) == true)
                                _hasChangedPassword = true;
                            else
                                _hasChangedPassword = false;
                        }
                    }
                    else
                    {
                        isLogin = false;
                        Session["isLogin"] = false;
                        ret = "Invalid User ID or Password";
                    }
                }
                else if (LoginType == "2")
                {
                    if (AD.IsAuthenticated(Domain, LDAPPath, UserName, Password))
                    {
                        if (UserName.ToLower() == "mossinst" || UserName.ToLower() == "admin")
                        {
                            isLogin = false;
                            
                        }
                        else if (mssql.ExecuteDataScalar("SELECT isnull(SessionID,'') as SessionID FROM Users WHERE UserID='" + UserName + "'").ToString() != "")
                            isLogin = true;
                        else if (mssql.ExecuteDataScalar("SELECT Active FROM Users WHERE UserID='" + UserName + "'").ToString() != "true")
                            isLogin = true;
                        else
                            isLogin = false;

                        if (isLogin == true)
                        {
                            ret = "User already login or User Not Logout Correctly Or User is not Active\r";
                            ret = "" + ret + "\rPlease contact System Administrator to Activated User ID";
                            //ShowMessage("User already login");
                            Session["isLogin"] = "False";
                        }
                        else
                        {
                            DataSet dsBranch = new DataSet();
                            dsBranch = mssql.ExecuteDataSet("SELECT OfficeCode FROM Users WHERE UserID='" + UserName + "'", CommandType.Text);
                            if (dsBranch.Tables[0].Rows.Count > 0)
                            {
                                string officeCode = dsBranch.Tables[0].Rows[0][0].ToString();
                                Session["Branch"] = officeCode;
                            }
                            Session["isLogin"] = isLogin;
                            Session["UserID"] = UserName;
                            UserData.UserID = Session["UserID"].ToString();
                            Session["Password"] = Password;
                            mssql.ExecuteDataSet("UPDATE Users SET isLogin=1, SessionID='" + Session.SessionID + "' , LastLogin='" + DateTime.Now + "' WHERE UserID='" + UserData.UserID + "'", System.Data.CommandType.Text);
                            mssql.ExecuteDataSet("INSERT INTO LogLogin (SessionID, LoginTime, UserID, IPAddress) VALUES ('" + Session.SessionID + "'," + "'" + DateTime.Now + "', '" + UserData.UserID + "', '" + IP + "')", CommandType.Text);
                            _hasChangedPassword = true;
                        }
                    }
                    else
                    {
                        isLogin = false;
                        Session["isLogin"] = "False";
                        ret = "Invalid User ID or Password";
                    }
                }
                else
                {
                    isLogin = false;
                    Session["isLogin"] = "False";
                    ret = "Invalid User ID or Password";
                }
            }
            else
              ret = "Invalid User ID or Password";
            return ret;
        }
        public string CheckLoginAgent(string UserName, string Password, string Ext, string IP)
        {
            string ret = "" ;
            if (CheckUserAndLoginType(UserName))
            {
                if (LoginType == "1")
                {
                    if (IsAuthenticatedLocal(UserName, Password))
                    {
                        if (UserName.ToLower() == "mossinst" || UserName.ToLower() == "admin" || UserName.ToLower() == "lcsadmin")
                            isLogin = false;
                        else if (mssql.ExecuteDataScalar("SELECT isnull(SessionID,'') as SessionID FROM Users WHERE UserID='" + UserName + "'").ToString() != "")
                            isLogin = false;
                        else if (mssql.ExecuteDataScalar("SELECT Active FROM Users WHERE UserID='" + UserName + "'").ToString() != "true")
                            isLogin = false;
                        else
                            isLogin = false;


                        if (isLogin == true)
                        {
                            ret = "User already login or User Not Logout Correctly Or User is not Active\r ";
                            ret = "" + ret + "\rPlease contact System Administrator to Activated User ID";
                            //ShowMessage("User already login");
                            Session["isLogin"] = "False";
                        }
                        else
                        {
                            Session["isLogin"] = isLogin;
                            Session["UserID"] = UserName;
                            UserData.UserID = Session["UserID"].ToString();
                            Session["Password"] = Password;
                            mssql.ExecuteDataSet("UPDATE Users SET isLogin=1, SessionID='" + Session.SessionID + "' , LastLogin='" + DateTime.Now + "' WHERE UserID='" + UserData.UserID + "'", System.Data.CommandType.Text);
                            mssql.ExecuteDataSet("INSERT INTO LogLogin (SessionID, LoginTime, UserID, IPAddress, Extension) VALUES ('" + Session.SessionID + "'," + "'" + DateTime.Now + "', '" + UserData.UserID + "', '" + IP + "','" + Ext + "')", CommandType.Text);
                        }
                    }
                    else
                    {
                        isLogin = false;
                        Session["isLogin"] = false;
                        ret = "Invalid User ID or Password";
                    }
                }
                else if (LoginType == "2")
                {
                    if (AD.IsAuthenticated(Domain, LDAPPath, UserName, Password))
                    {
                        if (UserName.ToLower() == "mossinst"  || UserName.ToLower() == "admin" || UserName.ToLower() == "lcsadmin")
                            isLogin = false;
                        else if (mssql.ExecuteDataScalar("SELECT isnull(SessionID,'') as SessionID FROM Users WHERE UserID='" + UserName + "'").ToString() != "")
                            isLogin = true;
                        else if (mssql.ExecuteDataScalar("SELECT Active FROM Users WHERE UserID='" + UserName + "'").ToString() != "true")
                            isLogin = true;
                        else
                            isLogin = false;


                        if (isLogin == true)
                        {
                            ret = "User already login or User Not Logout Correctly Or User is not Active\r" ;
                            ret = ""+ret+"\rPlease contact System Administrator to Activated User ID";
                            //ShowMessage("User already login")
                            Session["isLogin"] = "False";
                        }
                        else
                        {
                            Session["isLogin"] = isLogin;
                            Session["UserID"] = UserName;
                            UserData.UserID = Session["UserID"].ToString();
                            Session["Password"] = Password;
                            mssql.ExecuteDataSet("UPDATE Users SET isLogin=1, SessionID='" + Session.SessionID + "' , LastLogin='" + DateTime.Now + "' WHERE UserID='" + UserData.UserID + "'", System.Data.CommandType.Text);
                            mssql.ExecuteDataSet("INSERT INTO LogLogin (SessionID, LoginTime, UserID, IPAddress, Extension) VALUES ('" + Session.SessionID + "'," + "'" + DateTime.Now + "', '" + UserData.UserID + "', '" + IP + "','" + Ext + "')", CommandType.Text);
                        }
                    }
                    else
                    {
                        isLogin = false;
                        Session["isLogin"] = false;
                        ret = "Invalid User ID or Password";
                    }
                }

            }
            else
            {
                isLogin = false;
                Session["isLogin"] = false;
                ret = "Invalid User ID or Password";
            }
            return ret;
        }
        public bool isValidLogin()
        {
            string ValidSession = "";            
            try
            {
                if (UserData.UserID.ToLower() == "mossinst" || UserData.UserID.ToLower() == "admin" || UserData.UserID.ToLower() == "lcsadmin")
                    return true;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                ValidSession = "";
            }
            
            try
            {
                if(UserData.UserID!="")
                    ValidSession = mssql.ExecuteDataScalar("SELECT SessionID FROM Users WHERE UserID='" + UserData.UserID + "'").ToString();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                ValidSession = "";
            }

            if (ValidSession == UserData.SessionID)
                return true;
            else
            {
                LogOut();
                return false;
            }
        }
        public void LogOut()
        {
            mssql.ExecuteDataSet("UPDATE Users SET isLogin=0, SessionID='' WHERE UserID='" + UserData.UserID + "'", System.Data.CommandType.Text);
            mssql.ExecuteDataSet("UPDATE LogLogin SET LogoutTime='" + DateTime.Now + "' WHERE SessionID='" + Session.SessionID + "'" + "And UserID='" + UserData.UserID + "'", CommandType.Text);
            UserData.UserID = null;
            Session.Clear();
        }
        #endregion

        #region "JS"
        public void SetObjectInputToLowerCase(object Obj)
        {
            //Obj.Attributes.Add("OnChange", JS.EventLowercase.Value);
        }
        public void SetObjectInputToNumeric(object Obj)
        {
            //Page.ClientScript.RegisterStartupScript("NumericVal", "<Script> " + JS.FuncNumericValidation.Value + " </Script>");
            //Obj.Attributes.Add("OnChange", JS.EventNumeric.Value)
            //ClientScript.RegisterStartupScript(Obj, "NumericVal", "<Script> " + JS.FuncNumericValidation.Value + " </Script>")
            //Obj.Attributes.Add("OnChange", JS.EventNumeric.Value);
        }

        public void SetObjectInputToNumeric(object Obj, int Min, int Max)
        {
            //RegisterStartupScript("NumericRange", "<Script> " + JS.FuncFormatNumericRange.Value + " </Script>");
            //Obj.Attributes.Add("OnChange", JS.EventFormatNumericRange.Value.Replace("@min", Min.ToString.Trim).Replace("@max", Max.ToString.Trim));
        }
        public void SetObjectInputToEmailAddress(object Obj)
        {
            //RegisterStartupScript("EmailVal", "<Script> " + JS.FuncEmailValidation.Value + " </Script>");
            //Obj.Attributes.Add("OnChange", JS.EventEmailAddress.Value);
        }
        public void SetObjectInputToCurrency(object Obj)
        {
            //RegisterStartupScript("CurrencyFmt", "<Script> " + JS.FuncFormatCurrency.Value + " </Script>");
            //Obj.Attributes.Add("OnChange", JS.EventCurrency.Value);
        }
        public void SetObjectInputToCurrency(object Obj, int Min, int Max)
        {
            //RegisterStartupScript("CurrencyFmt", "<Script> " + JS.FuncFormatCurrencyRange.Value + " </Script>");
            //Obj.Attributes.Add("OnChange", JS.EventCurrencyRange.Value.Replace("@min", Min.ToString.Trim).Replace("@max", Max.ToString.Trim));
        }
        #endregion

        #region "MSSQL"
        public class MSSQLParameter
        {
            private string _Parameter = string.Empty;
            private SqlDbType DataType = new SqlDbType();
            public int Length = 0;
            private System.Data.ParameterDirection Direction = new System.Data.ParameterDirection();
            public bool NullAble = true;
            public int Precision = 0;
            public Object Value = new Object();
            public bool Insert = false;
            public bool Update = false;
            public bool Delete = false;
            public bool AsKey = false;

            public string Parameter
            {
                set
                {
                    if (value.IndexOf("@") >= 0)
                    {
                        _Parameter = value;
                    }
                    else _Parameter = "@" + value;
                }
            }
            public System.Data.SqlClient.SqlParameter SQLParameter
            {
                get
                {
                    System.Data.SqlClient.SqlParameter SP = new System.Data.SqlClient.SqlParameter();
                    return SP;
                }
            }
            public void FillParameter(string _Name, System.Data.SqlDbType _DataType, int _Length, System.Data.ParameterDirection _Direction, bool _Nullable, int _Precision, object _Value)
            {
                Parameter = _Name;
                DataType = _DataType;
                Length = _Length;
                Direction = _Direction;
                NullAble = _Nullable;
                Precision = _Precision;
                Value = _Value;
            }
            public void FillParameter(string _Name, System.Data.SqlDbType _DataType, int _Length, System.Data.ParameterDirection _Direction, object _Value)
            {
                FillParameter(_Name, _DataType, _Length, _Direction, true, 0, _Value);
            }
            public void FillParameter(string _Name, System.Data.SqlDbType _DataType, int _Length, object _Value)
            {
                FillParameter(_Name, _DataType, _Length, ParameterDirection.Input, true, 0, _Value);
            }
            public void FillCommand(bool _Insert, bool _Update, bool _Delete, bool _AsKey)
            {
                Insert = _Insert;
                Update = _Update;
                Delete = _Delete;
                AsKey = _AsKey;
            }
            public void FillCommand(bool _Insert, bool _Edit)
            {
                FillCommand(_Insert, _Edit, false, false);
            }
        }
        #endregion

    }
}
