using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PersonInChargeDeletedModel", Schema = "Customer")]
    public partial class PersonInChargeDeletedModel
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
    }
}
