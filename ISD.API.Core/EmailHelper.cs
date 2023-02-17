
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ISD.API.Core
{
    public interface IEmailHelper
    {
        void Send(string host, int port, string emailAccount, string emailPassword,MailMessage message);
    }
    public class EmailHelper : IEmailHelper
    {
        public void Send(string host, int port,string emailAccount,string emailPassword, MailMessage message)
        {
            //BodyBuilder bodyBuilder = new BodyBuilder();
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(from));
            //email.To.Add(MailboxAddress.Parse(to));
            //email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

           
            using (var smtp = new SmtpClient())
            {
                smtp.Host = host;
                smtp.Port = port;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(emailAccount, emailPassword);              
                smtp.Send(message);
            }
           
        }
    }
}
