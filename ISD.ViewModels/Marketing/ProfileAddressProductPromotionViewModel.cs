using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileAddressProductPromotionViewModel
    {
        public Nullable<System.Guid> ProfileId { get; set; }
        public Nullable<System.Guid> ProductPromotionContactId { get; set; }
        public string Address { get; set; }
        public Nullable<bool> CheckAddress { get; set; }
    }
}
