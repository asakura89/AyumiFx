using System;

namespace Maaya {
    public static class UrlExt {
        public static Boolean IsLocalUrl(this String url, Uri hostUri) {
            if (String.IsNullOrEmpty(url))
                return false;

            if (String.IsNullOrWhiteSpace(url))
                return false;

            if (url[0] == '/') {
                if (url.Length == 1) // "/" valid
                    return true;

                if (url.Length > 1 && (url[1] == '/' || url[1] == '\\')) // "//" or "/\" invalid
                    return false;

                return true;
            }

            if (url[0] == '~') {
                if (url.Length == 1) // "~" invalid
                    return false;

                if (url.Length == 2 && url[1] == '/') // "~/" valid
                    return true;

                if (url.Length > 2 && (url[2] == '/' || url[2] == '\\')) // "~//" or "~/\" invalid
                    return false;

                return true;
            }

            Boolean httpUrl = url.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase);
            Boolean httpsUrl = url.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase);
            if (httpUrl || httpsUrl) {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    return false;

                var absolute = new Uri(url);
                if (absolute.Host.Equals(hostUri.Host, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
