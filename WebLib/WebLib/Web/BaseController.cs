﻿using System;
using System.IO;
using System.Web;
using System.Web.UI;
using WebLib.Security;

namespace WebLib.Web
{
    public abstract class BaseController : IAuthorization
    {
        private readonly WebCookie cookie;
        protected readonly HttpContext currentContext;

        private const String USER_SESSION_KEY = "weblib.session.userid";
        private const String IP_SESSION_KEY = "weblib.session.clientip";
        private const String MODULE_SESSION_KEY = "weblib.session.module";
        private const String LOGIN_PAGE = "LoginPage";
        private const String USER_SESSION = "UserSession";

        protected const String ACTION_SESSION_KEY = "weblib.session.action";
        protected const String PAGING_SESSION_KEY = "weblib.session.paging";
        protected const String ACTIVEPAGE_SESSION_KEY = "weblib.session.activepage";
        protected const String TOTALROW_SESSION_KEY = "weblib.session.totalrow";
        protected const String SORT_SESSION_KEY = "weblib.session.sort";
        protected const String SORT_DIRECTION_SESSION_KEY = "weblib.session.sortdir";

        public Int32 CurrentTotalPage
        {
            get
            {
                Double totalPage = CurrentTotalRow / CurrentPagingSize;
                Int32 result = Convert.ToInt32(Math.Round(totalPage, 0));
                if (CurrentTotalRow - (result * CurrentPagingSize) > 0)
                    result += 1;

                return result;
            }
        }

        public String CurrentUserId
        {
            get { return currentContext.Session[USER_SESSION_KEY] == null ? String.Empty : currentContext.Session[USER_SESSION_KEY].ToString(); }
            set { currentContext.Session[USER_SESSION_KEY] = value; }
        }

        public String CurrentUserIP
        {
            get { return currentContext.Session[IP_SESSION_KEY] == null ? String.Empty : currentContext.Session[IP_SESSION_KEY].ToString(); }
            set { currentContext.Session[IP_SESSION_KEY] = value; }
        }

        public String CurrentPage
        {
            get { return Path.GetFileName(currentContext.Request.Path); }
        }

        public String CurrentAction
        {
            get { return currentContext.Session[ACTION_SESSION_KEY].ToString(); }
            set { currentContext.Session[ACTION_SESSION_KEY] = value; }
        }

        public Int32 CurrentPagingSize
        {
            get { return Convert.ToInt32(currentContext.Session[PAGING_SESSION_KEY]); }
            set { currentContext.Session[PAGING_SESSION_KEY] = value; }
        }

        public String CurrentSort
        {
            get { return currentContext.Session[SORT_SESSION_KEY].ToString(); }
            set { currentContext.Session[SORT_SESSION_KEY] = value; }
        }

        public String CurrentSortDirection
        {
            get { return currentContext.Session[SORT_DIRECTION_SESSION_KEY].ToString(); }
            set { currentContext.Session[SORT_DIRECTION_SESSION_KEY] = value; }
        }

        public Int32 CurrentActivePage
        {
            get { return Convert.ToInt32(currentContext.Session[ACTIVEPAGE_SESSION_KEY]); }
            set { currentContext.Session[ACTIVEPAGE_SESSION_KEY] = value; }
        }

        public Int32 CurrentTotalRow
        {
            get { return Convert.ToInt32(currentContext.Session[TOTALROW_SESSION_KEY]); }
            set { currentContext.Session[TOTALROW_SESSION_KEY] = value; }
        }

        protected BaseController()
        {
            cookie = new WebCookie(currentContext.Request, currentContext.Response);
        }

        private class WebCookie
        {
            private const String WEB_APPS_COOKIE_KEY = "weblib.cookie";
            private const String COOKIE_VALUE_ID = "cookieId";
            private const String COOKIE_VALUE_EXP = "cookieExp";
            private static WebSecurity security = new WebSecurity();
            private readonly HttpRequest request;
            private readonly HttpResponse response;

            public WebCookie(HttpRequest request, HttpResponse response)
            {
                this.request = request;
                this.response = response;
            }

            private HttpCookie FindCookie()
            {
                HttpCookie webAppsCookie = request.Cookies[WEB_APPS_COOKIE_KEY];
                return webAppsCookie;
            }

            /*public String GetCurrentSessionId()
            {
                HttpCookie currentCookie = FindCookie();
                return currentCookie.Values[COOKIE_VALUE_ID];
            }*/

            public Boolean IsCookieExists()
            {
                if (FindCookie() == null)
                    return false;

                return true;
            }

            public Boolean IsCookieExpired()
            {
                DateTime cookieExpDate = GetCookieExpiringDate();
                if (cookieExpDate < DateTime.Now)
                    return true;

                return false;
            }

            public DateTime GetCookieExpiringDate()
            {
                HttpCookie currentCookie = FindCookie();
                DateTime cookieExpDate;
                String cookieExp = security.DecryptTripleDes(currentCookie.Values[COOKIE_VALUE_EXP], true);
                DateTime.TryParse(cookieExp, out cookieExpDate);

                return cookieExpDate;
            }

            public void CreateCookie(String sessionId)
            {
                DateTime cookieExpDate = DateTime.Now.AddDays(1);
                HttpCookie newCookie = response.Cookies[WEB_APPS_COOKIE_KEY];
                String cookieId = sessionId;
                String cookieExp = security.EncryptTripleDES(cookieExpDate.ToString(), true);
                newCookie.Values.Add(COOKIE_VALUE_ID, cookieId);
                newCookie.Values.Add(COOKIE_VALUE_EXP, cookieExp);
                newCookie.Expires = cookieExpDate;
            }

            public void RemoveCookie()
            {
                if (IsCookieExists())
                    response.Cookies[WEB_APPS_COOKIE_KEY].Expires = DateTime.Now.AddDays(-1);
            }
        }

        protected abstract void OnLogin(String userId, String passwordString);
        protected abstract Boolean OnAuthorization();
        protected abstract void OnLogout();

        public Boolean IsAuthorized()
        {
            if (!cookie.IsCookieExists())
                return false;

            return !cookie.IsCookieExpired() && OnAuthorization();
        }

        public void Login(String userId, String passwordString)
        {
            OnLogin(userId, passwordString);
            cookie.CreateCookie(currentContext.Session.SessionID);
        }

        public void Logout()
        {
            OnLogout();
            currentContext.Session.RemoveAll();
            currentContext.Session.Timeout = 1;
            cookie.RemoveCookie();
        }

        public void SetCurrentPageTo(Int32 pageNumber)
        {
            CurrentActivePage = pageNumber;
        }

        public void SetCurrentPageToLast()
        {
            CurrentActivePage = CurrentTotalPage;
        }

        public void SetCurrentPageToFirst()
        {
            CurrentActivePage = 1;
        }

        public void SetCurrentPageToPrevious()
        {
            if (CurrentActivePage > 1) CurrentActivePage -= 1;
        }

        public void SetCurrentPageToNext()
        {
            Int32 totalPage = CurrentTotalPage;

            if (CurrentActivePage < totalPage && totalPage > 1)
                CurrentActivePage += 1;
            else if (CurrentActivePage > 1 && totalPage == 1)
                CurrentActivePage = 1;
        }

        public Boolean RedirectPageByClientScript(TemplateControl control, String url)
        {
            url = control.Page.ResolveUrl(url);
            RegisterJavascript(control, "Redirect", url);

            return true;
        }

        // http://www.codeproject.com/Tips/561490/ASP-NET-Response-Redirect-without-ThreadAbortExcep
        public Boolean RedirectPage(TemplateControl control, String url, Boolean ignoreIfInvisible)
        {
            Page page = control.Page;
            if (!ignoreIfInvisible || page.Visible)
            {
                // Sets the page for redirect, but does not abort.
                page.Response.Redirect(url, false);
                // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline
                // chain of execution and directly execute the EndRequest event.
                currentContext.ApplicationInstance.CompleteRequest();

                // By setting this to false, we flag that a redirect is set,
                // and to not render the page contents.
                page.Visible = false;
            }

            return true;
        }

        public Boolean RedirectPage(TemplateControl control, String url)
        {
            return RedirectPage(control, url, true);
        }

        public void RegisterJavascript(TemplateControl control, String script)
        {
            RegisterJavascript(control, "Script", script);
        }

        public void RegisterJavascript(TemplateControl control, String scriptTag, String script)
        {
            ScriptManager.RegisterClientScriptBlock(control, control.GetType(), scriptTag, script, true);
        }
    }
}