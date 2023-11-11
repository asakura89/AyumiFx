using System;
using System.Net.Mail;

namespace WebLib
{
    public class fMailer
    {
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
            get
            {
                return _SMTPServer;
            }
            set
            {
                _SMTPServer = value;
            }
        }
        public string MailTo
        {
            get
            {
                return _MailTo;
            }
            set
            {
                _MailTo = value;
            }
        }
        public string MailFrom
        {
            get
            {
                return _From;
            }
            set
            {
                _From = value;
            }
        }
        public string Cc
        {
            get
            {
                return _Cc;
            }
            set
            {
                _Cc = value;
            }
        }
        public string BCc
        {
            get
            {
                return _BCc;
            }
            set
            {
                _BCc = value;
            }
        }
        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }

        }
        public string Body
        {
            get
            {
                return _Body;
            }
            set
            {
                _Body = value;
            }
        }

        /// <summary>
        /// Function to send email
        /// </summary>
        public void Send()
        {
            try
            {
                MailMessage Msg = new MailMessage(MailFrom, MailTo, Subject, Body);
                Msg.IsBodyHtml = true;
                Msg.Headers.Clear();
                Msg.Headers.Add("Reply-To", MailFrom);
                //Mail.SmtpMail.SmtpServer = SMTPServer; //SMTP.UobBuana.com
                //Mail.SmtpMail.Send(Msg.From.ToString, Msg.To.ToString, Msg.Subject, Msg.Body);
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Send(Msg);
            }
            catch(Exception ex)
            {
            }
        }
    }
}
