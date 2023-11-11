using System.Web.Mail;

namespace WebLib
{
    public class Email
    {

        #region "Property"

        string _From;
        string _MailTo;
        string _Cc;
        string _BCc;
        string _Subject;
        string _Body;
        string _Attachment;
        string _SMTPServer;

        string _Priority;
        public string SMTPServer
        {
            get { return _SMTPServer; }
            set { _SMTPServer = value; }
        }
        public string MailFrom
        {
            get { return _From; }
            set { _From = value; }
        }
        public string MailTo
        {
            get { return _MailTo; }
            set { _MailTo = value; }
        }
        public string Cc
        {
            get { return _Cc; }
            set { _Cc = value; }
        }
        public string BCc
        {
            get { return _BCc; }
            set { _BCc = value; }
        }
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        public string Attachment
        {
            get { return _Attachment; }
            set { _Attachment = value; }
        }
        #endregion

        public void Send()
        {
                MailMessage Msg = new MailMessage();

                Msg.Headers.Clear();
                Msg.BodyFormat = MailFormat.Html;
                Msg.From = MailFrom;
                Msg.To = MailTo;
                Msg.Cc = Cc;
                Msg.Bcc = BCc;
                Msg.Subject = Subject;
                Msg.Body = Body;

                System.Web.Mail.MailAttachment fileAttachment = new System.Web.Mail.MailAttachment(_Attachment);
                Msg.Attachments.Add(fileAttachment);
                System.Web.Mail.SmtpMail.SmtpServer = SMTPServer;
                //SMTP.UobBuana.com
                System.Web.Mail.SmtpMail.Send(Msg);
        }
    }
}
