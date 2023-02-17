using System;

namespace ISD.SendMailCalendar.Models
{
    public class SendMailInfo
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Guid TargetGroupId { get; set; }
        public Guid ContentId { get; set; }
        public string FromAccountAddress { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }     
        public string Subject { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string OutgoingHost { get; set; }
        public int OutgoingPort { get; set; }
    }
}
