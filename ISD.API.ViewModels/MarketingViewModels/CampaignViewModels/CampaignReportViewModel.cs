namespace ISD.API.ViewModels.MarketingViewModels.CampaignViewModels
{
    public class CampaignReportViewModel
    {
        public int TotalMember { get; set; }
        public int TotalMailSend { get; set; }
        public int TotalMailBounce { get; set; }
        public int TotalMailOpened { get; set; }
        public int TotalConfirm { get; set; }
        public int TotalReject { get; set; }
        public int TotalCheckin { get; set; }
        public int PeopleCheckin { get; set; }
    }
}
