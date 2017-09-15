using System;

namespace Dfy
{
    [Serializable]
    public class DownloadFileInfo
    {
        public Byte[] FileByteArray { get; set; }
        public String MimeType { get; set; }
        public String Filename { get; set; }
        public String DocumentFullPath { get; set; }
        public String DocumentNo { get; set; }
        public String DocumentName { get; set; }
    }
}
