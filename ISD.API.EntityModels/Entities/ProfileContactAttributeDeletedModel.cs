using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileContactAttributeDeletedModel", Schema = "Customer")]
    public partial class ProfileContactAttributeDeletedModel
    {
        [Key]
        public Guid ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(50)]
        public string DepartmentCode { get; set; }
        public bool? IsMain { get; set; }
    }
}
