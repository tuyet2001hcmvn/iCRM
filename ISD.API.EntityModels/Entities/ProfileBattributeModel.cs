using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileBAttributeModel", Schema = "Customer")]
    public partial class ProfileBattributeModel
    {
        [Key]
        public Guid ProfileId { get; set; }
        [StringLength(50)]
        public string TaxNo { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(50)]
        public string CompanyNumber { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(50)]
        public string CustomerCareerCode { get; set; }
        [StringLength(50)]
        public string Website { get; set; }
    }
}
