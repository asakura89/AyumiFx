using System;
using WebLib.Data;

namespace WebLib
{
    public class cUserData : System.Web.UI.Page
    {
        private string _UserID = string.Empty;
        private string _UserName;
        private string _Password;
        private string _SessionID;
        private string _Email;
        private string _OfficeCode;
        private string _OfficeName;
        private MSSQL mssql = new MSSQL();
        private string _BranchCode = string.Empty;

        //public string UserID
        //{
           
        //    set 
        //    {               
        //        _UserID = value; 
        //    }
        //    get
        //    {              
        //        return _UserID;
        //    }
        //}

        public string UserID
        {
            get
            {
                try
                {
                    _UserID = Session["UserID"].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return _UserID;
            }
            set
            {
                _UserID = Session["UserID"].ToString();
                _UserID = value;
            }

        }
        public string SessionID
        {
            get
            {
                _SessionID = Session.SessionID;
                return _SessionID;
            }
            set
            {
                _SessionID = value;
            }
        }
        //public string BranchCode
        //{
        //    get
        //    {
        //        _BranchCode = Session["Branch"].ToString();
        //    }
        //    set
        //    {
        //        _BranchCode = Session["Branch"].ToString();
        //        _BranchCode = value;
        //    }
        //}
        public string Username
        {
            get
            {
                mssql.Create();
                _Email = mssql.ExecuteDataScallar("isNull(SELECT Email FROM Users WHERE UserID='" + UserID + "'),''").ToString();
                return _Email;
            }
        }
        public string UserOfficeCode
        {
            get
            {
                 mssql.Create();
                 _OfficeCode = mssql.ExecuteDataScallar("SELECT OfficeCode FROM Users WHERE UserID='" + UserID + "'").ToString();
                return _OfficeCode;
            }
        }
        public string UserOfficeName
        {
            get
            {
                mssql.Create();
                _OfficeName = mssql.ExecuteDataScallar("SELECT BranchName FROM Branch WHERE BranchCode='" + UserOfficeCode + "'").ToString();
                return _OfficeName;
            }
        }
        public string UserRegionCode
        {
            get
            {
                mssql.Create();
                return mssql.ExecuteDataScallar("SELECT Region FROM Branch WHERE BranchCode='" + UserOfficeCode + "'").ToString();
            }
        }
        public string UserGroupCode
        {
            get
            {
                mssql.Create();
                return mssql.ExecuteDataScallar("SELECT TOP 1 GroupID FROM GroupMembers WHERE UserID='" + UserID + "'").ToString();
            }
        }
        public string UserGroupName
        {
            get
            {
                mssql.Create();
                return mssql.ExecuteDataScallar("SELECT [Description] FROM Groups WHERE GroupID='" + UserGroupCode + "'").ToString();
            }
        }
        public string UserGroupDefaultLink
        {
            get
            {
                mssql.Create();
                return mssql.ExecuteDataScallar("SELECT DefaultLink FROM Groups WHERE Groupid = '" + UserGroupCode + "'").ToString();
            }
        }
        public bool hasChangedPassword
        {
            get
            {
                mssql.Create();
                return Convert.ToBoolean(mssql.ExecuteDataScallar("SELECT hasChangedPassword FROM Users WHERE UserID='" + UserID + "'"));
            }
        }
    }
}
