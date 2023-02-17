using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ProfileContactProductPromotionViewModel
    {
        public Nullable<System.Guid> ProductPromotionContactId { get; set; }
        public Guid? ProfileId { get; set; }
        public int? ProfileCode { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public bool? CheckContact { get; set; }
        public Guid? ProfileContactId { get; set; }
    }
}
