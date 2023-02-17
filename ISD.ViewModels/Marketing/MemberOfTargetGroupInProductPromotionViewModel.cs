using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class MemberOfTargetGroupInProductPromotionViewModel
    {
        public Guid? ProfileId { get; set; }
        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public List<ProfileAddressProductPromotionViewModel> ProfileAddress { get; set; }
        public List<ProfileContactProductPromotionViewModel> ProfileContact { get; set; }
    }
}
