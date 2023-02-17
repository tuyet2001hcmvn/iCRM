using System;

namespace ISD.API.ViewModels.MarketingViewModels.MemberOfTargetGroupViewModels
{
    public class MemberOfTargetGroupViewViewModel
    {
        public Guid ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
