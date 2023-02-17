using ISD.SendMailCalendar.Data;
using ISD.SendMailCalendar.Models;
using MailKit;
using MailKit.Net.Pop3;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISD.SendMailCalendar
{
    public partial class frmSendMailCalendar : Form
    {
        private readonly SendMailCalendarEntities1 _dbContext;
        private readonly AppConfig appConfig;
        public frmSendMailCalendar()
        {
            InitializeComponent();
            _dbContext = new SendMailCalendarEntities1();
            appConfig = GetSetting();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            AddSendingEffect();
            DateTime now = DateTime.Now;
            TimeSpan start = TimeSpan.Parse(appConfig.StartTime);
            TimeSpan end = TimeSpan.Parse(appConfig.EndTime);
            if (now.TimeOfDay >= start && now.TimeOfDay <= end)
            {
                var sendMailCalendarWithoutUnsubscribe = from sendMailCalendar in _dbContext.SendMailCalendarModels
                                                         join unsubscribe in _dbContext.Unfollows on sendMailCalendar.ToEmail equals unsubscribe.Email
                                                         into temp
                                                         from mail in temp.DefaultIfEmpty()
                                                         where mail.Email == null
                                                         select sendMailCalendar;

                var emails = from sendMailCalendar in sendMailCalendarWithoutUnsubscribe
                             join campaign in _dbContext.CampaignModels on sendMailCalendar.CampaignId equals campaign.Id
                             where sendMailCalendar.IsSend == false
                                   && campaign.ScheduledToStart <= now
                             orderby campaign.CampaignCode
                             select new SendMailInfo
                             {
                                 Id = sendMailCalendar.Id,
                                 CampaignId = campaign.Id,
                                 TargetGroupId=campaign.TargetGroupId,
                                 ContentId=campaign.ContentId,
                                // FromAccountAddress = emailAccount.Account,
                                // Password = emailAccount.Password,
                                ToName = sendMailCalendar.Param,
                                // SenderName = content.SenderName,
                                // Content = content.Content,
                                // Subject = content.Subject,
                                 ToEmail = sendMailCalendar.ToEmail,
                                // OutgoingHost = provider.OutgoingHost,
                                // OutgoingPort = provider.OutgoingPort,
                             };             
                var list = emails.Take(appConfig.NumOfMailSend).ToList();               
                if (list.Count != 0)
                {
                    foreach (var email in list)
                    {
                        ContentModel content = _dbContext.ContentModels.Where(s => s.Id == email.ContentId).FirstOrDefault();
                        EmailAccountModel emailAccount = _dbContext.EmailAccountModels.Where(s => s.Id == content.FromEmailAccountId).FirstOrDefault();
                        MailServerProviderModel provider = _dbContext.MailServerProviderModels.Where(s => s.Id == emailAccount.ServerProviderId).FirstOrDefault();
                        TargetGroupModel targetGroup = _dbContext.TargetGroupModels.Where(s => s.Id == email.CampaignId).FirstOrDefault();
                        email.FromAccountAddress = emailAccount.Account;
                        email.Password = emailAccount.Password;
                        email.SenderName = content.SenderName;
                        email.Content = content.Content;
                        email.Subject = content.Subject;
                        email.OutgoingHost = provider.OutgoingHost;
                        email.OutgoingPort = provider.OutgoingPort;
                        _ = SendEmailAsync(email);
                    }
                }
            }

            timerSend.Start();
            AddWaitingEffect();
        }
        private void AddSendingEffect()
        {
            btnSend.Text = " The service is running \n Sending email...";
            Console.WriteLine("chagne");
            btnSend.BackColor = Color.LightGreen;
        }
        private void AddWaitingEffect()
        {
            btnSend.Text = " The service is running \n Waiting for the next mailing time";
            btnSend.BackColor = Color.LightBlue;
        }
        private async Task SendEmailAsync(SendMailInfo mailInfo)
        {

            using (SmtpClient client = new SmtpClient())
            {
                client.Host = mailInfo.OutgoingHost;
                client.Port = mailInfo.OutgoingPort;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailInfo.FromAccountAddress, mailInfo.Password);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Headers.Add(Constants.TrackingId, mailInfo.Id.ToString());
               // mailMessage.Headers.Add("Return-Receipt-To", mailInfo.FromAccountAddress);
                mailMessage.From = new MailAddress(mailInfo.FromAccountAddress, mailInfo.SenderName);
                mailMessage.Sender = new MailAddress(mailInfo.FromAccountAddress, mailInfo.SenderName);
                mailMessage.ReplyToList.Add(new MailAddress(appConfig.EmailToReply));
                mailMessage.To.Add(new MailAddress(mailInfo.ToEmail));
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = mailInfo.Content.Replace("unsubscribe", "<a href=\""+appConfig.UnsubscribePageUrl + mailInfo.Id + "\">unsubscribe</a>").Replace("##FullName##", mailInfo.ToName);
                //mailMessage.Body = mailInfo.Content.Replace("##FullName##", mailInfo.SenderName);
                //  "+appConfig.NET5ApiDomain+"
                // https://localhost:44367/

                var img = "<img alt=\"isd\" title=\"isd\" style=\"display: block\" width=\"1\" height=\"1\" src=\" " + appConfig.NET5ApiDomain + "api/Marketing/Emails/TrackingOpenedEmail/" + mailInfo.Id + ".png\"/>";
                mailMessage.Body += img;             
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = mailInfo.Subject;
                try
                {
                    await client.SendMailAsync(mailMessage);
                    WriteLogFile(appConfig.LogPath, string.Format("send email  to {0} at {1}", mailInfo.ToEmail, DateTime.Now));
                    var send = _dbContext.SendMailCalendarModels.FirstOrDefault(s => s.Id == mailInfo.Id);
                    send.IsSend = true;                
                    _dbContext.SaveChanges();
                    TrackingCampaignStatus(mailInfo.Id, mailInfo.CampaignId);
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
                    WriteLogFile(appConfig.LogPath, string.Format("Can not send mail to {0}. {1}", mailInfo.ToEmail, exMess));
                    return;
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
                    WriteLogFile(appConfig.LogPath, string.Format("Can not send mail to {0}. {1}", mailInfo.ToEmail, exMess));
                    return;
                }
            }
        }
        private void TrackingCampaignStatus(Guid mailId, Guid campaignId)
        {
            var listStatus = _dbContext.CatalogModels.Where(s => s.CatalogTypeCode == Constants.CampaignStatusTypeCode && s.Actived == true).ToList();
            var campaign = _dbContext.CampaignModels.FirstOrDefault(s => s.Id == campaignId);
            var planMailCountInCampaign = _dbContext.SendMailCalendarModels.Where(s => s.CampaignId == campaignId && s.IsSend == false).Count();
            var mailIsUnsubscribeInCampaign = from sendMailCalendar in _dbContext.SendMailCalendarModels
                                         join unsubscribe in _dbContext.Unfollows on sendMailCalendar.ToEmail equals unsubscribe.Email
                                         where sendMailCalendar.CampaignId == campaignId
                                         select sendMailCalendar;
                                                
            if ((planMailCountInCampaign - mailIsUnsubscribeInCampaign.Count()) > 0)
            {
                campaign.Status = listStatus.FirstOrDefault(s => s.CatalogCode == Constants.CampaignStatus_Active).CatalogId;
            }
            else
            {
                campaign.Status = listStatus.FirstOrDefault(s => s.CatalogCode == Constants.CampaignStatus_Finnished).CatalogId;
            }
            _dbContext.SaveChanges();
        }
        public static void WriteLogFile(string filePath, string message)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Create(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Close();
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
                sw.WriteLine(lastRecordText);
                sw.Close();
            }
        }

        private AppConfig GetSetting()
        {
            AppConfig config = new AppConfig();
            config.NumOfMailSend = Int32.Parse(ConfigurationManager.AppSettings["NumOfMailSend"]);
            config.ResendAfterMinutes = Int32.Parse(ConfigurationManager.AppSettings["ResendAfterMinutes"]);
            config.LogPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["LogFolderPath"] + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            config.BounceMailPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["BounceMailFolderPath"];
            config.TrackingBounceMailAfterMinutes = Int32.Parse(ConfigurationManager.AppSettings["TrackingBounceMailAfterMinutes"]);
            config.NET5ApiDomain =ConfigurationManager.AppSettings["NET5ApiDomain"];
            config.UnsubscribePageUrl = ConfigurationManager.AppSettings["UnsubscribePageUrl"];
            config.EmailToReply = ConfigurationManager.AppSettings["EmailToReply"];
            timerSend.Interval = config.ResendAfterMinutes * 60000;
            timerTrackingBounceMail.Interval = config.TrackingBounceMailAfterMinutes * 60000;
            config.StartTime= ConfigurationManager.AppSettings["StartTime"];
            config.EndTime = ConfigurationManager.AppSettings["EndTime"];
            return config;
        }

        private void timerSend_Tick(object sender, EventArgs e)
        {
            btnSend_Click(sender, e);
        }

       private void TrackingBounceMail()
        {
            DownloadBounceMail();
            var listBounceId = GetBounceMailId();
            UpdateSendMailCalendarStatus(listBounceId);
        }

        private void UpdateSendMailCalendarStatus(List<Guid> listBounceId)
        {
            foreach(var id in listBounceId)
            {
                Console.WriteLine(id);
                var sendEmailCalendar = _dbContext.SendMailCalendarModels.FirstOrDefault(e => e.Id == id);
                if(sendEmailCalendar!=null)
                {
                    sendEmailCalendar.IsBounce = true;
                }
            }
            _dbContext.SaveChanges();
        }

        private void DownloadBounceMail()
        {
            int totalBounceEmail = 0;
            var listMarketingMail = _dbContext.EmailAccountModels.Where(s=>s.IsSender==true).ToList();   
            foreach(var marketingEmail in listMarketingMail)
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
        private List<Guid> GetBounceMailId()
        {
            List<Guid> list = new List<Guid>();
            string[] bouncePath = Directory.GetFiles(appConfig.BounceMailPath);
            for(int i=0;i<bouncePath.Length;i++)
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
        private void timerTrackingBounceMail_Tick(object sender, EventArgs e)
        {
            btnTrackingBounceMail_Click(sender, e);
        }

        private void btnTrackingBounceMail_Click(object sender, EventArgs e)
        {
            TrackingBounceMail();
            timerTrackingBounceMail.Start();
        }

        private void frmSendMailCalendar_Shown(object sender, EventArgs e)
        {
            btnSend_Click(sender, e);
            btnTrackingBounceMail_Click(sender, e);
        }
    }
}
