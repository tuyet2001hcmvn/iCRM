namespace ISD.WinForm.Models
{
    public class AppConfig
    {
        public int NumOfMailSend { get; set; }
        public int ResendAfterMinutes { get; set; }
        public string LogPath { get; set; }
        public string BounceMailPath { get; set; }
        public int TrackingBounceMailAfterMinutes { get; set; }
        public string NET5ApiDomain { get; set; }
        public string UnsubscribePageUrl { get; set; }
        public string ConfirmLinkUrl { get; set; }
        public string QRCodeUrl { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
