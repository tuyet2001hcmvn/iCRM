using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductPromotionUpdateViewModel
    {
        public int? STT { get; set; }
        public Nullable<System.Guid> ProductPromotionDetailId { get; set; }
        public Nullable<System.Guid> ProductPromotionContactId { get; set; }
        public Nullable<System.Guid> ProductPromotionId { get; set; }
        public Nullable<System.Guid> ProfileId { get; set; }
        public int? ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string ProfileShortName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public bool? CheckContact { get; set; }
        public Guid? ProfileContactId { get; set; }
        public string Address { get; set; }
        public Nullable<bool> CheckAddress { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
