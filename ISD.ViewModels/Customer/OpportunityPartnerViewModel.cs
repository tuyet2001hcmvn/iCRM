using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class OpportunityPartnerViewModel
    {
        public Guid OpportunityPartnerId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? PartnerId { get; set; }
        public int? PartnerType { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool? IsMain { get; set; }
        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string Address { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string CreateUser { get; set; }
    }
}
