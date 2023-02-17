using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PersonInChargeModel", Schema = "Customer")]
    public partial class PersonInChargeModel
    {
        [Key]
        public Guid PersonInChargeId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        [StringLength(50)]
        public string RoleCode { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        public int? SalesEmployeeType { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.PersonInChargeModels))]
        public virtual ProfileModel Profile { get; set; }
        [ForeignKey(nameof(SalesEmployeeCode))]
        [InverseProperty(nameof(SalesEmployeeModel.PersonInChargeModels))]
        public virtual SalesEmployeeModel SalesEmployeeCodeNavigation { get; set; }
    }
}
