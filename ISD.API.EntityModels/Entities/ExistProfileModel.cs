using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ExistProfileModel", Schema = "Customer")]
    public partial class ExistProfileModel
    {
        [Key]
        [StringLength(50)]
        public string ProfileForeignCode { get; set; }
        [Key]
        [StringLength(50)]
        public string CompanyCode { get; set; }
    }
}
