using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Utilities.Controllers
{
    public class SendGirdController : Controller
    {
        // GET: SendGird
        public ActionResult Index()
        {
            //Execute().Wait();
            return View();
        }


        //static async Task Execute()
        //{
        //    try
        //    {
        //        MailMessage mailMsg = new MailMessage();

        //        // To
        //        mailMsg.To.Add(new MailAddress("nam.vh@isdcorp.vn", "To Name"));

        //        // From
        //        mailMsg.From = new MailAddress("info@isdcorp.vn", "From Name");

        //        // Subject and multipart/alternative Body
        //        mailMsg.Subject = "subject";
        //        string text = "text body";
        //        string html = @"<p>html body</p>";
        //        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
        //        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

        //        // Init SmtpClient and send
        //        SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        //        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", "send grid api key");
        //        smtpClient.Credentials = credentials;

        //        smtpClient.Send(mailMsg);
        //    }
        //    catch //(Exception ex)
        //    {
               
        //    }
        //}
        /*
        static async Task Execute()
        {
            var client = new SendGridClient("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");
            //Người gửi
            var from = new EmailAddress("info@isdcorp.vn", "ISD CORP");
            //Tiêu đề
            var subject = "Tiêu đề gửi email";
            //Người nhận
            var to = new EmailAddress("nam.vh@isdcorp.vn", "Vũ Hoài Nam");
            //Nội dung
            var plainTextContent = "Nội dung text";
            var htmlContent = "<strong>Gửi email nội dung in đậm</strong>";
            //Gửi mail
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //Kết quả
            var response = await client.SendEmailAsync(msg);

            var result = response;
        }*/
    }
}