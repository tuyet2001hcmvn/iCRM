using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileCAttributeModel", Schema = "Customer")]
    public partial class ProfileCattributeModel
    {
        [Key]
        public Guid ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
    }
}
