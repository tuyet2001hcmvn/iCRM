using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileMainContact
    {
        public Guid? ProfileId { get; set; }
        public int ProfileCode { get; set; }
        [StringLength(255)]
        public string ProfileName { get; set; }
        public Guid ContactId { get; set; }
        public int ContactCode { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public string Phones { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string DepartmentName { get; set; }
        [StringLength(1000)]
        public string PositionName { get; set; }
        public bool? IsMain { get; set; }
    }
}
