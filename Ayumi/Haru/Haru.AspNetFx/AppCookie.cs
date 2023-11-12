using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Haru.AspNetFx {
    public class AppCookie : IAppCookie {
        public AppCookie(IHttpRequestAdapter request, IHttpResponseAdapter response) {
            HttpContext.Current.Request.Cookies
        }

        public IEnumerable<String> Keys => throw new NotImplementedException();

        public void Clear() {
            throw new NotImplementedException();
        }

        public Boolean Exists(String key) {
            throw new NotImplementedException();
        }

        public String Get(String key) {
            throw new NotImplementedException();
        }

        public void Remove(String key) {
            throw new NotImplementedException();
        }

        public void Set(String key, String value) {
            throw new NotImplementedException();
        }
    }
}
