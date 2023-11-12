using System;
using System.IO;
using System.Text;
using System.Web;

namespace WebLib.WebHandler
{
    public class Download : IHttpHandler
    {
        private String filename = String.Empty;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // TODO: check or not ?
                //if (!CheckAuth())
                //    throw new Exception("You're not authenticated.");

                String queryString = context.Request.QueryString["file"];
                if (String.IsNullOrEmpty(queryString))
                    throw new ArgumentNullException("Download failed: filename is not provided.");

                filename = Encoding.Default.GetString(Convert.FromBase64String(queryString));
                if (!File.Exists(filename))
                    throw new ArgumentNullException("Download failed: file is not exists in server.");

                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(filename));
                context.Response.WriteFile(filename);
                context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
        }

        public Boolean IsReusable
        {
            get { return false; }
        }

        private Boolean CheckAuth()
        {
            return true;
        }
    }
}
