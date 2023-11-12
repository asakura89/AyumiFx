using System;

namespace WebLib
{
    public class MailSender
    {
        static void SendMail(Email _email)
        {
            try
            {
                Console.WriteLine("Sending Email");
                _email.Send();
                Console.WriteLine("Mail sent");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void SendMail(string mailTo, string mailFrom, string cc, string smtp, string body, string subject, string attachment)
        {
            try
            {
                Email _email = new Email();
                _email.MailTo = mailTo;
                _email.Cc = cc;
                _email.MailFrom = mailFrom;
                _email.SMTPServer = smtp;
                _email.Body = body;
                _email.Subject = subject;
                _email.Attachment = attachment;

                Console.WriteLine("Sending Email");
                _email.Send();
                Console.WriteLine("Mail sent");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
    }
}
