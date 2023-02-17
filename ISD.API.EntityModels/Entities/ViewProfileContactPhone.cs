using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileContactPhone
    {
        public Guid? CompanyId { get; set; }
        public Guid ProfileId { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public int ProfileCode { get; set; }
        [StringLength(255)]
        public string ProfileName { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string DepartmentName { get; set; }
        [StringLength(1000)]
        public string PositionName { get; set; }
    }
}
