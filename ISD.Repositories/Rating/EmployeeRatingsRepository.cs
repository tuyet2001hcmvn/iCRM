using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class EmployeeRatingsRepository
    {
        EntityDataContext _context;
        /// <summary>
        /// Khởi tạo account repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public EmployeeRatingsRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }
        public void SendMail(string EmailContent, string Subject, string FromMail, string Account, string FromEmailPassword, string Host, int Port, bool EnableSsl, string ToMail)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(FromMail);
            email.Sender = new MailAddress(FromMail);
            List<string> toEmailList = ToMail.Split(';').ToList();
            foreach (var toEmail in toEmailList.Distinct())
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    email.To.Add(new MailAddress(toEmail.Trim()));
                }
            }
            //email.CC.Add(new MailAddress(FromMail.Trim()));
            email.Body = EmailContent;
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = Subject;

            string message = "";
            using (var smtp = new SmtpClient())
            {
                smtp.Host = Host;
                smtp.Port = Port;
                smtp.EnableSsl = EnableSsl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Account, FromEmailPassword);
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
            }
        }
    }
}
