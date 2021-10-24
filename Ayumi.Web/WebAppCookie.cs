using System;
using System.Web;

namespace WebApp {
    public static class WebAppCookie {
        const String WebAppsCookieId = "webapps.cookie";
        const String CookieSessionKey = "webapps.cookie.session";
        const String CookieExpiredKey = "webapps.cookie.expired";

        static HttpCookie FindCookie() {
            HttpCookie webAppsCookie = HttpContext.Current.Request.Cookies[WebAppsCookieId];
            return webAppsCookie;
        }

        public static String GetUserInfo() {
            HttpCookie currentCookie = FindCookie();
            String[] splittedCookie = currentCookie.Values[CookieSessionKey].Split(';');
            if (splittedCookie.Length == 1)
                return String.Empty;
            return splittedCookie[1];
        }

        public static Boolean IsCookieExists() {
            return FindCookie() != null;
        }

        public static Boolean IsCookieExpired() {
            HttpCookie currentCookie = FindCookie();
            /*DateTime cookieExpDate;
            DateTime.TryParse(currentCookie.Values[CookieExpiredKey], out cookieExpDate);
            return cookieExpDate < DateTime.Now;*/
            return currentCookie.Expires < DateTime.Now;
        }

        public static void CreateCookie(String userInfo) {
            DateTime cookieExpDate = DateTime.Now.AddDays(14);
            var newCookie = new HttpCookie(WebAppsCookieId); //HttpContext.Current.Response.Cookies[WebAppsCookieId];
            newCookie.Values.Add(CookieSessionKey,
                Guid.NewGuid().ToString("N") + Delimiter.Additional + userInfo);
            newCookie.Values.Add(CookieExpiredKey, cookieExpDate.ToString());
            newCookie.Expires = cookieExpDate;
            HttpContext.Current.Response.Cookies.Add(newCookie);
            //HttpContext.Current.Response.Cookies.Set(newCookie);
        }

        public static void RemoveCookie() {
            if (IsCookieExists())
                HttpContext.Current.Response.Cookies[WebAppsCookieId].Expires = DateTime.Now.AddDays(-1);
        }
    }
}