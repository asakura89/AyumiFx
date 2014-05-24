using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using WebLib.Data;
using WebLib.Extensions.Model;
using WebLib.Security;
using WebLib.Web;

namespace WebLib.Extensions
{
    public class Controller : BaseController, IExceptionCapture
    {
        private readonly ADM_R_USERRepository userRepo = new ADM_R_USERRepository();
        private readonly ADM_R_LOG_LOGINRepository logLoginRepo = new ADM_R_LOG_LOGINRepository();
        private readonly ADM_R_ERROR_LOGRepository errorLogRepo = new ADM_R_ERROR_LOGRepository();
        private readonly ADM_R_GROUP_ACCESSRepository groupAccessRepo = new ADM_R_GROUP_ACCESSRepository();
        private readonly ADM_R_MODULERepository moduleRepo = new ADM_R_MODULERepository();
        private readonly ADM_R_GROUP_MEMBERSRepository groupMembersRepo = new ADM_R_GROUP_MEMBERSRepository();

        protected override void OnLogin(String userId, String passwordString)
        {
            ADM_R_USER currentUser = userRepo.GetById(userId);
            try
            {
                if (currentUser != null)
                {
                    if (currentUser.counter >= 5)
                        throw new HttpException(403, ErrorConstant.ERR_CONTACT_ADMIN);

                    Int32 loggedInUserInfoCount = GetLoggedInUserInfoCount(userId);
                    if (loggedInUserInfoCount > 0)
                        KickAllThisLoggedUser(userId);

                    LoginToActiveDirectory(userId, passwordString);
                    CurrentUserId = userId;
                    CreateLoginLog(currentContext.Session.SessionID);
                }
                else
                    throw new Exception("Logon failure: unknown user name or bad password.");
            }
            catch (Exception)
            {
                if (currentUser != null)
                    LogFailedLogin(currentUser);
                throw;
            }
        }

        protected Boolean IsLoggedIn(String userId)
        {
            return GetLoggedInUserInfoCount(userId) > 0;
        }

        private Int32 GetLoggedInUserInfoCount(String userId)
        {
            List<ADM_R_LOG_LOGIN> logLoginList = logLoginRepo.GetLoggedUserInfoList(userId);

            return logLoginList.Count;
        }

        private void KickAllThisLoggedUser(String userId)
        {
            List<ADM_R_LOG_LOGIN> logLoginList = logLoginRepo.GetLoggedUserInfoList(userId);
            foreach (ADM_R_LOG_LOGIN logLogin in logLoginList)
            {
                logLogin.logout_time = DateTime.Now;
                logLoginRepo.Update(logLogin);
            }
        }

        private void LoginToActiveDirectory(String userId, String passwordString)
        {
            LDAP activeDirectory = new LDAP();
            String domain = ConfigurationManager.AppSettings[AppConstant.DOMAIN].ToString();
            String activeDirectoryPath = ConfigurationManager.AppSettings[AppConstant.AD_PATH].ToString();
            if (activeDirectory.IsAuthenticated(domain, activeDirectoryPath, userId, passwordString))
                return;
        }

        private void CreateLoginLog(String sessionId)
        {
            ADM_R_LOG_LOGINRepository logLoginRepo = new ADM_R_LOG_LOGINRepository();
            ADM_R_LOG_LOGIN logLogin = new ADM_R_LOG_LOGIN();
            logLogin.log_id = Guid.NewGuid().ToString("N");
            logLogin.id_user = CurrentUserId;
            logLogin.ip_address = CurrentUserIP;
            logLogin.login_time = DateTime.Now;
            logLogin.id_session = sessionId;
            logLogin.extension = String.Empty; // TODO: masih dikosongin

            logLoginRepo.Insert(logLogin);
        }

        private void LogFailedLogin(ADM_R_USER currentUser)
        {
            currentUser.counter += 1;
            userRepo.Update(currentUser);
        }

        protected override Boolean OnAuthorization()
        {
            return groupAccessRepo.IsAuthorized(CurrentUserId, CurrentModule);
        }

        protected override void OnLogout()
        {
            ADM_R_LOG_LOGIN logLogin = logLoginRepo.GetLoggedUserInfoList(CurrentUserId, currentContext.Session.SessionID);
            if (logLogin != null)
            {
                logLogin.logout_time = DateTime.Now;
                logLoginRepo.Update(logLogin);
            }
        }

        public void OnException(TemplateControl control, Exception ex)
        {
            try
            {
                String formattedErrorMessage = ex.Message.Trim().Replace(Environment.NewLine, "\\n\\n");
                String javascriptErrorAlert = "alert(\"The following errors have occurred: \\n\\n " + formattedErrorMessage + " \");";
                RegisterJavascript(control, "Exception", javascriptErrorAlert);
                CaptureExceptionIntoDatabase(ex);
            }
            catch (Exception exx)
            {
                try
                {
                    CaptureExceptionIntoWindowsLog(ex);
                    CaptureExceptionIntoWindowsLog(exx);
                }
                catch
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                    Debug.WriteLine(exx.Message + Environment.NewLine + Environment.NewLine + exx.StackTrace);
                }
            }
        }

        private void CaptureExceptionIntoDatabase(Exception ex)
        {
            ADM_R_ERROR_LOG errLog = new ADM_R_ERROR_LOG();
            errLog.id_user = CurrentUserId;
            errLog.id_module = CurrentModule;
            errLog.error_date = DateTime.Now;
            errLog.error_message = ex.Message;

            errorLogRepo.Insert(errLog);
        }

        private void CaptureExceptionIntoWindowsLog(Exception ex)
        {
            EventLog errEventLog = new EventLog("Application", Environment.MachineName, "WebLib");
            if (!EventLog.SourceExists(errEventLog.Source, errEventLog.MachineName))
                EventLog.CreateEventSource(errEventLog.Source, errEventLog.Log);

            errEventLog.WriteEntry(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
        }

        protected virtual DataTable GetGridViewData(String tableName, String defaultSortedColumn, String columnToSort, String sortDirection, String condition)
        {
            PagingParameter pagingParam = InitPagingParameter(tableName, defaultSortedColumn, columnToSort, sortDirection, condition);
            DataSet ds = GetPagingDataSet(pagingParam);

            if (ds != null)
            {
                CurrentTotalRow = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                DataView dv = ds.Tables[1].DefaultView;
                if (columnToSort != String.Empty)
                    dv.Sort = pagingParam.sort;
                CurrentSort = pagingParam.sort;

                return dv.ToTable();
            }

            return new DataTable();
        }

        protected DataSet GetPagingDataSet(PagingParameter pagingParam)
        {
            String pagingQuery = String.Format("EXEC {0} {1},{2},{3},'{4}','{5}'",
                SPName.SP_PAGING, pagingParam.source, pagingParam.activePage,
                pagingParam.pagingSize, pagingParam.condition, pagingParam.sort);
            Repository repo = new Repository();
            return repo.ExecDataSet(pagingQuery);
        }

        private PagingParameter InitPagingParameter(String tableName, String defaultSortedColumn, String columnToSort, String sortDirection, string condition)
        {
            PagingParameter pagingParam = new PagingParameter();
            pagingParam.source = tableName;
            pagingParam.sort = columnToSort == String.Empty ? defaultSortedColumn : columnToSort + " " + sortDirection;
            pagingParam.condition = condition;
            pagingParam.pagingSize = CurrentPagingSize;
            pagingParam.activePage = CurrentActivePage;

            return pagingParam;
        }

        public String GetSearchString(List<SearchParameter> searchParamList)
        {
            List<String> searchStringList = new List<String>();
            foreach (SearchParameter searchParam in searchParamList)
            {
                if (String.IsNullOrEmpty(searchParam.columnName))
                    continue;

                String searchOperator = searchParam.searchOperator == OperatorConstant.OP_LIKE.Trim() ? "%" : String.Empty;
                searchStringList.Add(String.Format("{0} {1} ''{2}{3}{2}''", searchParam.columnName, searchParam.searchOperator,
                    searchOperator, searchParam.keyword));
            }
            if (searchStringList == null || searchStringList.Count == 0)
                return " 1=1 ";
            return String.Join(" AND ", searchStringList.ToArray());
        }

        public String CurrentModule
        {
            get { return moduleRepo.GetByLink(CurrentPage); }
        }

        public List<String> GetAccessList()
        {
            List<String> groupIdList = new List<String>(CollectionExtended.Select(groupMembersRepo.GetByUser(CurrentUserId),
                delegate(ADM_R_GROUP_MEMBERS groupMembers) { return groupMembers.id_group; }));
            
            var accessAttributeRepo = new RepoManager<VIEW_GROUP_ACCESS_ATTRIBUTE>();
            String combinedGroupId = String.Empty;
            for (int i = 1; i <= groupIdList.Count; i++)
                combinedGroupId += "@" + i + ", ";
            String query = String.Format("SELECT * FROM VIEW_GROUP_ACCESS_ATTRIBUTE WHERE ID_MODULE = @0 AND ID_GROUP IN ({0})", combinedGroupId.TrimEnd(',', ' '));
            String[] paramArray = new String[groupIdList.Count+1];
            for (int i = 1; i <= groupIdList.Count; i++)
                paramArray[i] = groupIdList[i-1];

            paramArray[0] = CurrentModule;
            List<String> accessList = new List<String>();
            foreach (String groupId in groupIdList)
            {
                List<String> accessAttributeList = new List<String>(CollectionExtended.Select(accessAttributeRepo.GetDataList(query, paramArray),
                    delegate(VIEW_GROUP_ACCESS_ATTRIBUTE accessAttribute) { return accessAttribute.description; }));
                accessList.AddRange(accessAttributeList);
            }

            return accessList;
        }

        public Boolean CheckUserAuth(TemplateControl control)
        {
            if (String.IsNullOrEmpty(CurrentUserId))
                return !RedirectPage(control, "~/");

            try
            {
                if (!IsAuthorized())
                    throw new Exception("You're not authorized to access this module.");
            }
            catch (Exception ex)
            {
                OnException(control, ex);
                return !RedirectPageByClientScript(control, "~/");
            }

            return true;
        }
    }
}
