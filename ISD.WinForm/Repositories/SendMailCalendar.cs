using ISD.WinForm.Data;
using ISD.WinForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using MailKit.Security;
using System.Threading;
using System.Collections.Concurrent;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ISD.WinForm.Repositories
{
    public class SendMailCalendar
    {
        private static string appPath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static string logFilePath = System.IO.Path.Combine(appPath, DateTime.Now.ToString("yyyyMMdd") + "-SendMail" + ".log");

        //task
        CancellationTokenSource Source = new CancellationTokenSource();
        CancellationToken Token;
        ConcurrentQueue<Task> ToDoQueue = new ConcurrentQueue<Task>();




        /// <summary>
        /// Khởi tạo repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public SendMailCalendar()
        {
            Token = Source.Token;
        }

        public void SendMail(AppConfig appConfig)
        {
            try
            {
                var _context = new SendMailCalendarEntityDataContext();
                DateTime now = DateTime.Now;
                TimeSpan start = TimeSpan.Parse(appConfig.StartTime);
                TimeSpan end = TimeSpan.Parse(appConfig.EndTime);
                //if (now.TimeOfDay >= start && now.TimeOfDay <= end)
                if (true)
                {
                    //#region Email without Unsubscribe
                    //var sendMailCalendarWithoutUnsubscribe = from sendMailCalendar in _context.SendMailCalendarModel
                    //                                         join unsubscribe in _context.Unfollow on sendMailCalendar.ToEmail equals unsubscribe.Email
                    //                                         into temp
                    //                                         from mail in temp.DefaultIfEmpty()
                    //                                         where mail.Email == null
                    //                                         select sendMailCalendar;
                    //#endregion

                    #region Email needs to be sent
                    //Using table: SendMailCalendarModel
                    var emailList = (from sendMailCalendar in _context.SendMailCalendarModel
                                     join campaign in _context.CampaignModel on sendMailCalendar.CampaignId equals campaign.Id
                                     join status in _context.CatalogModel on campaign.Status equals status.CatalogId
                                     where sendMailCalendar.IsSend == false //chưa gửi
                                           && sendMailCalendar.isError != true // chưa có lỗi
                                           && campaign.ScheduledToStart <= now // thời gian gửi của chiến dịch trước thời gian hiện tại
                                           && (status.CatalogCode == Constants.CampaignStatus_Planned || status.CatalogCode == Constants.CampaignStatus_Active) // trạng thái là chưa gửi hoặc đang gửi
                                     orderby campaign.CampaignCode
                                     select new SendMailInfo
                                     {
                                         SendMailCalendarId = sendMailCalendar.Id,
                                         CampaignId = campaign.Id,
                                         TargetGroupId = campaign.TargetGroupId,
                                         ContentId = campaign.ContentId,
                                         ToName = sendMailCalendar.Param,
                                         ToEmail = sendMailCalendar.ToEmail,
                                         Type = campaign.Type,
                                     }).Take(appConfig.NumOfMailSend).ToList();
                    #endregion

                    if (emailList.Count != 0)
                    {
                        List<Guid> campaignIdList = new List<Guid>();
                        //Lấy toàn bộ các danh sách về C#
                        var emailAccountList = _context.EmailAccountModel.ToList();
                        var providerList = _context.MailServerProviderModel.ToList();
                        var unfollowList = _context.Unfollow.ToList();
                        var contentList = _context.ContentModel.ToList();

                        foreach (var email in emailList)
                        {
                            // TODO: 1000 email cần gửi => call db 4000 lần, trong khi đó có thể mấy cái này trùng hết
                            ContentModel content = contentList.Where(s => s.Id == email.ContentId).FirstOrDefault();
                            //nếu nội dung tồn tại kiểm tra email cần gửi có trong list unsubcribe của công ty dựa vào mã công ty của từng nội dung gửi
                            if (content != null)
                            {
                                //check xem có phải là unfollow email không
                                Unfollow unsubscribe = null;
                                if (unfollowList != null && unfollowList.Count > 0)
                                {
                                    //unsubscribe = unfollowList.FirstOrDefault(s => s.Email == email.ToEmail && s.CompanyCode == content.CompanyCode);
                                    unsubscribe = unfollowList.FirstOrDefault(s => s.Email == email.ToEmail);// && s.CompanyCode == content.CompanyCode
                                }

                                //nếu email không có trong list unsubscribe của công ty thì get setting
                                if (unsubscribe == null)
                                {
                                    //GET email account
                                    EmailAccountModel emailAccount = emailAccountList.Where(s => s.Id == content.FromEmailAccountId).FirstOrDefault();
                                    //get mail server provider
                                    MailServerProviderModel provider = providerList.Where(s => s.Id == emailAccount.ServerProviderId).FirstOrDefault();
                                    email.FromAddress = emailAccount.Address;
                                    if (content.CatalogCode == "Email" && content.EmailType == "PersonalEmail" && !string.IsNullOrEmpty(content.SentFrom))
                                    {
                                        email.FromAddress = content.SentFrom + emailAccount.Domain;
                                    }
                                    email.Account = emailAccount.Account;
                                    email.Password = emailAccount.Password;
                                    email.EnableSsl = emailAccount.EnableSsl ?? false;
                                    email.SenderName = content.SenderName;
                                    email.Content = content.Content;
                                    email.Subject = content.Subject;
                                    email.OutgoingHost = provider.OutgoingHost;
                                    email.OutgoingPort = provider.OutgoingPort;
                                    email.CatalogCode = content.CatalogCode;

                                    //add task to ToDoQueue
                                    ToDoQueue.Enqueue(Execute(email, appConfig));

                                    //var threadThree = new Thread(() => SendEmailAsync(email, appConfig));
                                    //threadThree.Start();


                                    //while (ToDoQueue.Count > 0 && !Token.IsCancellationRequested)
                                    //{
                                    //    Task task;
                                    //    if (ToDoQueue.TryDequeue(out task))
                                    //    {
                                    //        task.Wait(Token);
                                    //    }
                                    //}
                                }
                                //nếu email unsubscribe thì update lỗi 
                                else
                                {
                                    var unsubscribeMail = _context.SendMailCalendarModel.FirstOrDefault(s => s.Id == email.SendMailCalendarId);
                                    unsubscribeMail.isError = true;
                                    unsubscribeMail.IsBounce = true;
                                    unsubscribeMail.ErrorMessage = "Unsubscribe email";
                                    _context.SaveChanges();
                                }
                                //thêm mã chiến dịch cần update trang thái sau khi gửi mail
                                var isExistCampaignId = campaignIdList.FirstOrDefault(s => s == email.CampaignId);
                                if (isExistCampaignId == null || isExistCampaignId == Guid.Empty)
                                {
                                    campaignIdList.Add(email.CampaignId);
                                }
                            }
                        }

                        while (ToDoQueue.Count > 0 && !Token.IsCancellationRequested)
                        {
                            Task task;
                            if (ToDoQueue.TryDequeue(out task))
                            {
                                task.Wait(Token);
                            }
                        }

                        //update trạng thái của những chiến dịch có email vừa được gửi
                        //if (campaignIdList.Count > 0)
                        //{
                        //    TrackingCampaignStatus(campaignIdList);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLogFile(logFilePath, ex.Message);
                throw ex;
            }
        }
        static async Task Execute(SendMailInfo mailInfo, AppConfig appConfig)//, CancellationToken token
        {
            using (SendMailCalendarEntityDataContext context = new SendMailCalendarEntityDataContext())
            {
                try
                {
                    #region Send Email
                    //var apiKey = Environment.GetEnvironmentVariable("AnCuongCRM_NET_APIKEY");
                    var client = new SendGridClient(mailInfo.Password);
                    var from = new EmailAddress(mailInfo.FromAddress, mailInfo.SenderName);
                    var subject = mailInfo.Subject;
                    var to = new EmailAddress(mailInfo.ToEmail, mailInfo.ToName);
                    var plainTextContent = string.Empty;
                    var htmlContent = string.Empty;

                    #region Replace and Tracking
                    //Tạo 1 hình ảnh 1px dùng để tracking: người nhận mở email
                    //link hình có lưu thông tin SendMailCalendarId: ID email gửi đi 
                    //khi người dùng mở email, sẽ request vào link hình và biết được ai đã mở
                    var img = "<img alt style=\"height:1px!important; width:1px!important; border-width:0!important; margin-top:0!important; margin-bottom:0!important; margin-right:0!important; margin-left:0!important; padding-top:0!important; padding-bottom:0!important; padding-right:0!important; padding-left:0!important\" width=\"1\" height=\"1\" border=\"0\" src=\" " + appConfig.NET5ApiDomain + "api/Marketing/Emails/TrackingOpenedEmail/" + mailInfo.SendMailCalendarId + ".png\"/>";
                    //Loại Email
                    if (mailInfo.CatalogCode == "Email")
                    {
                        //Loại Marketing
                        if (mailInfo.Type == "Marketing")
                        {
                            htmlContent = mailInfo.Content
                                                   //Replate unsubscribe link
                                                   .Replace("unsubscribe", "<a href=\"" + appConfig.UnsubscribePageUrl + mailInfo.SendMailCalendarId + "\">unsubscribe</a>")
                                                   //Replate ##FullName## = tên người nhận email
                                                   .Replace("##FullName##", mailInfo.ToName);
                        }
                        else if (mailInfo.Type == "Event") //Loại Event
                        {

                            htmlContent = mailInfo.Content
                                                        //Replate unsubscribe link
                                                        .Replace("unsubscribe", "<a href=\"" + appConfig.UnsubscribePageUrl + mailInfo.SendMailCalendarId + "\">unsubscribe</a>")
                                                        //Replate ##FullName## = tên người nhận email
                                                        .Replace("##FullName##", mailInfo.ToName)
                                                        //Replate ###ConfirmLink### = Đường đẫn confirm
                                                        .Replace("##ConfirmLink##", appConfig.ConfirmLinkUrl + mailInfo.SendMailCalendarId + "?Type=Confirm")
                                                        //##RejectLink## = đường dẫn từ chối
                                                        .Replace("##RejectLink##", appConfig.ConfirmLinkUrl + mailInfo.SendMailCalendarId + "?Type=Reject")
                                                        .Replace("##QRCode##", appConfig.QRCodeUrl + mailInfo.SendMailCalendarId);
                        }
                        //Gán thêm 1 hình 1px vào để tracking, khi người nhận mở email
                        htmlContent += img;
                        #endregion
                        //Tạo email
                        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                        //Gửi mail thông qua sendgrid
                        var response = client.SendEmailAsync(msg).Result;

                        #endregion

                        if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            // Setting isSend
                            var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                            send.IsSend = true;
                            send.SendTime = DateTime.Now;
                            context.SaveChanges();
                        }
                        else
                        {
                            var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                            send.isError = true;
                            send.IsBounce = true;
                            if (response != null)
                            {
                                send.ErrorMessage = response.StatusCode.ToString();
                            }
                            context.SaveChanges();
                        }
                    }
                    //Loại SMS
                    else if (mailInfo.CatalogCode == "SMS")
                    {

                    }

                }
                catch (SmtpException ex)
                {
                    var exMess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            exMess = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            exMess = ex.InnerException.Message;
                        }
                    }
                    var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                    send.isError = true;
                    send.IsBounce = true;
                    send.ErrorMessage = exMess;
                    context.SaveChanges();
                    if (ex.Message == "The operation has timed out.")
                    {
                        //var timeOut = true;
                    }
                    //WriteLogFile(appConfig.LogPath, string.Format("Can not send mail to {0}. {1}", mailInfo.ToEmail, exMess));
                }
                catch (Exception ex)
                {
                    var exMess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            exMess = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            exMess = ex.InnerException.Message;
                        }
                    }
                    var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                    send.isError = true;
                    send.IsBounce = true;
                    send.ErrorMessage = exMess;
                    context.SaveChanges();
                }

            }
        }

        public static void SendEmailAsync(SendMailInfo mailInfo, AppConfig appConfig)
        {
            using (SmtpClient smtpClient = new SmtpClient(mailInfo.OutgoingHost, mailInfo.OutgoingPort))
            {
                using (SendMailCalendarEntityDataContext context = new SendMailCalendarEntityDataContext())
                {
                    try
                    {
                        // Email settings
                        #region
                        MailMessage mailMsg = new MailMessage();
                        // To                
                        mailMsg.To.Add(new MailAddress(mailInfo.ToEmail));
                        // From
                        mailMsg.From = new MailAddress(mailInfo.FromAddress, mailInfo.SenderName);
                        // Subject and multipart/alternative Body

                        mailMsg.Subject = mailInfo.Subject;
                        #region Email Body
                        mailMsg.IsBodyHtml = true;
                        //Body:
                        // + Replace link unsubscribe   
                        // + Replate FullName
                        // + Add Image check: Opening Email
                        var img = "<img alt style=\"height:1px!important; width:1px!important; border-width:0!important; margin-top:0!important; margin-bottom:0!important; margin-right:0!important; margin-left:0!important; padding-top:0!important; padding-bottom:0!important; padding-right:0!important; padding-left:0!important\" width=\"1\" height=\"1\" border=\"0\" src=\" " + appConfig.NET5ApiDomain + "api/Marketing/Emails/TrackingOpenedEmail/" + mailInfo.SendMailCalendarId + ".png\"/>";
                        if (mailInfo.Type == "Marketing")
                        {
                            mailMsg.Body = mailInfo.Content
                                               //Replate unsubscribe link
                                               .Replace("unsubscribe", "<a href=\"" + appConfig.UnsubscribePageUrl + mailInfo.SendMailCalendarId + "\">unsubscribe</a>")
                                               //Replate ##FullName## = tên người nhận email
                                               .Replace("##FullName##", mailInfo.ToName);
                        }
                        else if (mailInfo.Type == "Event")
                        {
                            mailMsg.Body = mailInfo.Content
                                            //Replate unsubscribe link
                                            .Replace("unsubscribe", "<a href=\"" + appConfig.UnsubscribePageUrl + mailInfo.SendMailCalendarId + "\">unsubscribe</a>")
                                            //Replate ##FullName## = tên người nhận email
                                            .Replace("##FullName##", mailInfo.ToName)
                                            //Replate ###ConfirmLink### = Đường đẫn confirm
                                            .Replace("##ConfirmLink##", appConfig.ConfirmLinkUrl + mailInfo.SendMailCalendarId + "?Type=Confirm")
                                            //##RejectLink## = đường dẫn từ chối
                                            .Replace("##RejectLink##", appConfig.ConfirmLinkUrl + mailInfo.SendMailCalendarId + "?Type=Reject")
                                            .Replace("##QRCode##", appConfig.QRCodeUrl + mailInfo.SendMailCalendarId);
                        }
                        mailMsg.Body += img;
                        mailMsg.BodyEncoding = Encoding.UTF8;
                        #endregion

                        //Add Tracking
                        mailMsg.Headers.Add(Constants.TrackingId, mailInfo.SendMailCalendarId.ToString());
                        #endregion

                        // Sending email
                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailInfo.Account, mailInfo.Password);
                        smtpClient.Credentials = credentials;
                        smtpClient.Send(mailMsg);

                        // Setting isSend
                        var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                        send.IsSend = true;
                        send.SendTime = DateTime.Now;
                        context.SaveChanges();
                        // Writing log
                        //WriteLogFile(appConfig.LogPath, string.Format("send email  to {0} at {1}", mailInfo.ToEmail, DateTime.Now));

                    }
                    catch (SmtpException ex)
                    {
                        var exMess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                exMess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                exMess = ex.InnerException.Message;
                            }
                        }
                        var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                        send.isError = true;
                        send.IsBounce = true;
                        send.ErrorMessage = exMess;
                        context.SaveChanges();
                        if (ex.Message == "The operation has timed out.")
                        {
                            // var timeOut = true;
                        }
                        //WriteLogFile(appConfig.LogPath, string.Format("Can not send mail to {0}. {1}", mailInfo.ToEmail, exMess));
                    }
                    catch (Exception ex)
                    {
                        var exMess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                exMess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                exMess = ex.InnerException.Message;
                            }
                        }
                        var send = context.SendMailCalendarModel.FirstOrDefault(s => s.Id == mailInfo.SendMailCalendarId);
                        send.isError = true;
                        send.IsBounce = true;
                        send.ErrorMessage = exMess;
                        context.SaveChanges();
                        //WriteLogFile(appConfig.LogPath, string.Format("Can not send mail to {0}. {1}", mailInfo.ToEmail, exMess));
                    }
                }
            }
        }

        private void TrackingCampaignStatus(List<Guid> campaignIdList)
        {
            SendMailCalendarEntityDataContext _context = new SendMailCalendarEntityDataContext();
            var listStatus = _context.CatalogModel.Where(s => s.CatalogTypeCode == Constants.CampaignStatusTypeCode && s.Actived == true).ToList();
            foreach (var campaignId in campaignIdList)
            {
                var campaign = _context.CampaignModel.FirstOrDefault(s => s.Id == campaignId);
                // đếm số email có issend != true của chiến dịch
                var planMailCountInCampaign = _context.SendMailCalendarModel.Where(s => s.CampaignId == campaignId && s.IsSend != true).Count();
                //đếm số email có issend != true và isError == true của chiến dịch
                var errorMails = _context.SendMailCalendarModel.Where(s => s.CampaignId == campaignId && s.IsSend != true && s.isError == true).Count();
                //nếu số email có trạng thái chưa gửi trừ số email có trạng thái chưa gửi nhưng có lỗi > 0 => chiến dịch đang gửi ngược lại kết thúc
                if ((planMailCountInCampaign - errorMails) > 0)
                {
                    campaign.Status = listStatus.FirstOrDefault(s => s.CatalogCode == Constants.CampaignStatus_Active).CatalogId;
                }
                else
                {
                    campaign.Status = listStatus.FirstOrDefault(s => s.CatalogCode == Constants.CampaignStatus_Finnished).CatalogId;
                }
            }
            _context.SaveChanges();
        }

        public void DownloadBounceMail(AppConfig appConfig)
        {
            var _context = new SendMailCalendarEntityDataContext();
            int totalBounceEmail = 0;
            var listMarketingMail = _context.EmailAccountModel.Where(s => s.IsSender == true).ToList();
            foreach (var marketingEmail in listMarketingMail)
            {
                using (var client = new Pop3Client())
                {
                    client.Connect("mail.ancuong.com", 110, SecureSocketOptions.None);
                    client.Authenticate(marketingEmail.Account, marketingEmail.Password);
                    int currentIndex = totalBounceEmail;
                    totalBounceEmail = totalBounceEmail + client.Count;

                    for (int i = currentIndex; i < totalBounceEmail; i++)
                    {
                        var message = client.GetMessage(i);
                        using (var fileStream = File.Create(appConfig.BounceMailPath + string.Format("bounce{0}", i + 1)))
                        {
                            message.WriteTo(fileStream);
                        }
                        client.DeleteMessage(i);
                    }
                    Console.WriteLine("Find {0} bounce emails ", client.Count);
                    client.Disconnect(true);
                }
            }
        }

        public List<Guid> GetBounceMailId(AppConfig appConfig)
        {
            List<Guid> list = new List<Guid>();
            string[] bouncePath = Directory.GetFiles(appConfig.BounceMailPath);
            for (int i = 0; i < bouncePath.Length; i++)
            {
                foreach (var line in File.ReadAllLines(bouncePath[i]))
                {
                    if (line.Contains(Constants.TrackingId))
                    {
                        string[] keyvalue = line.Split(':');
                        if (!string.IsNullOrWhiteSpace(keyvalue[1]))
                        {
                            list.Add(Guid.Parse(keyvalue[1].Trim()));
                        }
                    }
                }
                File.Delete(bouncePath[i]);
            }
            Console.WriteLine("Delete {0} bounce email in folder", bouncePath.Count());
            return list;
        }

        public void UpdateSendMailCalendarStatus(List<Guid> listBounceId)
        {
            var _context = new SendMailCalendarEntityDataContext();
            foreach (var id in listBounceId)
            {
                Console.WriteLine(id);
                var sendEmailCalendar = _context.SendMailCalendarModel.FirstOrDefault(e => e.Id == id);
                if (sendEmailCalendar != null)
                {
                    sendEmailCalendar.IsBounce = true;
                }
            }
            _context.SaveChanges();
        }

        //    public static void WriteLogFile(string filePath, string message)
        //    {

        //        var folder = System.IO.Directory.GetParent(filePath).FullName;
        //        bool exists = System.IO.Directory.Exists(folder);
        //        if (!exists)
        //            System.IO.Directory.CreateDirectory(folder);

        //        if (!System.IO.File.Exists(filePath))
        //            System.IO.File.Create(filePath);

        //        var maxRetry = 3;
        //        for (int retry = 0; retry < maxRetry; retry++)
        //        {
        //            try
        //            {
        //                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        //                {
        //                    fileStream.Flush();
        //                    fileStream.Close();
        //                }

        //                using (StreamWriter sw = new StreamWriter(filePath, true))
        //                {
        //                    string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
        //                    sw.WriteLine(lastRecordText);
        //                    sw.Close();
        //                    break; // you were successfull so leave the retry loop
        //                }
        //            }
        //            catch (IOException)
        //            {
        //                if (retry < maxRetry - 1)
        //                {
        //                    System.Threading.Thread.Sleep(2000); // Wait some time before retry (2 secs)
        //                }
        //                else
        //                {
        //                    // handle unsuccessfull write attempts or just ignore.
        //                }
        //            }
        //        }
        //    //}
    }
}
