using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileExtendInfo
    {
        public Guid ProfileId { get; set; }
        public int ProfileCode { get; set; }
        [StringLength(20)]
        public string ProfileForeignCode { get; set; }
        [StringLength(255)]
        public string ProfileName { get; set; }
        [StringLength(50)]
        public string PersonInChargeCompanyCode { get; set; }
        public string PersonInChargeCode { get; set; }
        public string PersonInCharge { get; set; }
        public string PersonInChargeFull { get; set; }
        public string RoleInCharge { get; set; }
        [StringLength(50)]
        public string CustomerGroupCompanyCode { get; set; }
        public string CustomerGroupName { get; set; }
    }
}
