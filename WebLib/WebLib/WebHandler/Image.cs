using System;
using System.IO;
using System.Text;
using System.Web;

namespace WebLib.WebHandler
{
    public class Image : IHttpHandler
    {
        private String filename = String.Empty;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                String queryString = context.Request.QueryString["name"];
                if (String.IsNullOrEmpty(queryString))
                    throw new ArgumentNullException("Download failed: imagename is not provided.");

                filename = Encoding.Default.GetString(Convert.FromBase64String(queryString));
                if (!File.Exists(filename))
                    throw new ArgumentNullException("Download failed: image is not exists in server.");

                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                Byte[] imageByteArray = null;
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (var binaryReader = new BinaryReader(stream))
                    imageByteArray = binaryReader.ReadBytes((Int32)stream.Length);

                context.Response.BinaryWrite(imageByteArray);
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
    }
}
