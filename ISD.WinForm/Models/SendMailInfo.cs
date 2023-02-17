using System;

namespace ISD.WinForm.Models
{
    public class SendMailInfo
    {
        public Guid SendMailCalendarId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid TargetGroupId { get; set; }
        public Guid ContentId { get; set; }
        public string FromAddress { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }     
        public string Subject { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string OutgoingHost { get; set; }
        public int OutgoingPort { get; set; }
        public string Type { get; set; }
        public string CatalogCode { get; set; }
        public string EmailType { get; set; }
    }
}
